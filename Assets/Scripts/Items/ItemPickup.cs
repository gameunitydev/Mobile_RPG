using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public WeaponStats weaponStats;

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    private void PickUp()
    {
        if (Inventory.Instance.weapons.Count >= Inventory.Instance.inventoryCapacity)
        {
            UIGamePlay.Instance.DisplayMessage("Inventory is full.", Colors.redMessage, 2f, false);
            return;
        }

        // Add to Inventory
        weaponStats = GetComponent<Weapon>().weaponStats;
        Inventory.Instance.Add(weaponStats);
        Destroy(gameObject);
    }
}