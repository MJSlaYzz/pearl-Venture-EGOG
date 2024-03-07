using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/* This object manages the inventory UI. */

public class InventoryUI : MonoBehaviour
{

    public GameObject inventoryUI;  // The entire UI
    public Transform itemsParent;   // The parent object of all the items

    Inventory inventory;    // Our current inventory

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
    }

    // Check to see if we should open/close the inventory
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("UI OPEN");
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            UpdateUI();
        }
    }

    // Update the inventory UI by:
    //		- Adding items
    //		- Clearing empty slots
    // This is called using a delegate on the Inventory.
    public void UpdateUI()
    {
        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                // Check if the item quantity is greater than 1 (indicating a stack)
                if (inventory.items[i].quantity > 1)
                {
                    // Update the quantity text to show "x{quantity}" format
                    slots[i].quantityText.text = "x" + inventory.items[i].quantity;
                    slots[i].quantityText.enabled = true; // Enable the quantity text
                }
                else
                {
                    // Quantity is 1, so display it as empty
                    slots[i].quantityText.text = "";
                    slots[i].quantityText.enabled = false; // Disable the quantity text
                }

                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

}