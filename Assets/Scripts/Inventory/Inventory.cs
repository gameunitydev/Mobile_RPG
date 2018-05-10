using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public delegate void OnInventoryChange();

    public int inventoryCapacity = 27;

    public OnInventoryChange onInventoryChangeCallback;
    public List<Item> items = new List<Item>();

    #region Gold

    private int gold;

    public void AddGold(int count)
    {
        gold += count;

        onInventoryChangeCallback?.Invoke();
    }

    public void RemoveGold(int count)
    {
        int goldValueAfterRemove = gold - count;

        if (goldValueAfterRemove < 0)
        {
            UIGamePlay.Instance.DisplayMessage("Not enough gold", Colors.redMessage, 2f, false);
        }
        else
        {
            gold = goldValueAfterRemove;
            onInventoryChangeCallback?.Invoke();
        }
    }

    public int GetGoldCount()
    {
        return gold;
    }

    #endregion

    #region ItemsInGeneral

    public void AddItem(Item item)
    {
        items.Add(item);
        
        onInventoryChangeCallback?.Invoke();
    }

    public void DestroyItem(int itemSlotIndex)
    {
        items.RemoveAt(itemSlotIndex);

        onInventoryChangeCallback?.Invoke();
    }

    public bool IsFull()
    {
        if (items.Count >= inventoryCapacity)
        {
            UIGamePlay.Instance.DisplayMessage("Inventory is full.", Colors.redMessage, 2f, false);
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Equipment

    [SerializeField] private List<Weapon> equippedWeapons = new List<Weapon>();
    public bool equipMode;

    public void EquipWeapon(int weaponSlotIndex, int equipSlotIndex)
    {
        var weaponToEquip = items[weaponSlotIndex] as Weapon;

        if (weaponToEquip != null)
        {
            equippedWeapons.Insert(equipSlotIndex, weaponToEquip);

            InventoryUI.Instance.equipSlots[equipSlotIndex].associatedWeapon = weaponToEquip;

            DestroyItem(weaponSlotIndex);

            // If we need to swap - SwapSlots()

            onInventoryChangeCallback.Invoke();

            PlayerData.Instance.AddToEquipSlot(weaponToEquip.weaponPrefab, equipSlotIndex);
            AudioManager.Instance.WeaponChangeSound();
        }
    }

    public void SwapSlots( /*WeaponSlot, InventorySlot*/)
    {
    }

    public void UnEquipWeapon(int slotIndex)
    {
        var weaponToUnEquip = equippedWeapons[slotIndex];
        equippedWeapons.RemoveAt(slotIndex);
        InventoryUI.Instance.equipSlots[slotIndex].associatedWeapon = null;
        PlayerData.Instance.RemoveFromEquipSlot(slotIndex);
        AddItem(weaponToUnEquip);
    }

    #endregion

    #region DropItems

    public List<Item> droppedItems = new List<Item>();

    public void AddToDropList(int itemSlotIndex)
    {
        if (droppedItems.Count == 10)
        {
            UIGamePlay.Instance.DisplayMessage("You can drop only 10 items at once", Colors.redMessage, 2f, false);
            return;
        }

        droppedItems.Add(items[itemSlotIndex]);
        items.RemoveAt(itemSlotIndex);
        InventoryUI.Instance.SelectNextSlot(true); // ToDo TEMP CONVINIENCE


        onInventoryChangeCallback?.Invoke();
    }

    public void GenerateIfDrop()
    {
        if (droppedItems.Count > 0)
        {
            ContainerGenerator.Instance.GenerateAndFillContainer(ContainerGenerator.Instance.containerJunkPrefab,
                PlayerData.Instance.transform, ContainerTypeEnum.Enum.Junk, droppedItems);
            droppedItems.Clear();
        }
    }

    #endregion
}