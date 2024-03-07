using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/* Sits on all InventorySlots. */

public class InventorySlot : MonoBehaviour
{

	public Image icon;
	public Button removeButton;
	public Text quantityText;
	Item item;  // Current item in the slot

	// Add item to the slot
	public void AddItem(Item newItem)
	{
		item = newItem;

		icon.sprite = item.icon;
		icon.enabled = true;
		removeButton.interactable = true;
		UpdateQuantity();
	}

	//Clear the slot
	public void ClearSlot()
	{
		if (icon)
		{
			icon.sprite = null;
			icon.enabled = false;
		}

		if (removeButton.interactable)
		{
			removeButton.interactable = false;
			quantityText.enabled = false;
		}

		
	}
	
	// If the remove button is pressed, this function will be called.
	public void RemoveItemFromInventory()
	{
		Inventory.instance.Remove(item);
		UpdateQuantity();
	}

	public void UpdateQuantity()
	{
		if (item != null)
		{
			if (item.quantity >= 1)
			{
				quantityText.text = "x" + item.quantity.ToString();
				quantityText.enabled = true; // Enable the quantity text
			}
			else
			{
				quantityText.text = "";
				quantityText.enabled = false; // Disable the quantity text
			}
		}
		else
		{
			quantityText.text = "";
			quantityText.enabled = false; // Disable the quantity text
		}
	}
}

