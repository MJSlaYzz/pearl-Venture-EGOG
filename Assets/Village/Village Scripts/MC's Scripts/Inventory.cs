using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance;

    void Awake()
    {
        instance = this;

    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 8;  // Amount of item spaces

    public List<Item> items = new List<Item>();

    public bool Add(Item item)
    {
        if (item.showInInventory)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not enough room.");
                return false;
            }

            int existingItemIndex = -1;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].name == item.name && items[i].quantity < 3)
                {
                    existingItemIndex = i;
                    break;
                }
            }

            if (existingItemIndex != -1)
            {
                items[existingItemIndex].quantity++;
            }
            else
            {
                Item newItem = Instantiate(item);
                newItem.quantity = 1;
                items.Add(newItem);
            }

            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }

        return true;
    }


    // Remove an item
    public void Remove(Item item)
        {
            int existingItemIndex = items.FindIndex(i => i.name == item.name);

            if (existingItemIndex != -1)
            {
                // Decrease the quantity of the existing item
                items[existingItemIndex].quantity--;

                if (items[existingItemIndex].quantity <= 0)
                {
                    // Remove the item when quantity is less than or equal to 0
                    items.RemoveAt(existingItemIndex);
                }
            }

            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }
        public int FindItemIndexByName(string itemName)
        {
            return items.FindIndex(i => i.name == itemName);
        }


    }

/*public bool Add(Item item)
    {
        if (item.showInInventory)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not enough room.");
                return false;
            }

            int existingItemIndex = items.FindIndex(i => i.name == item.name);

            if (existingItemIndex != -1)
            {
                if (items[existingItemIndex].quantity < 3)
                {
                    items[existingItemIndex].quantity++;
                }
                else
                {
                    // If the current slot is full, try the next slot
                    if (items.Count < space)
                    {
                        Item newItem = Instantiate(item);
                        newItem.quantity = 1;
                        items.Add(newItem);

                    }
                }
            }
            else
            {
                // Item not in the inventory, add it as a new item
                Item newItem = Instantiate(item);
                newItem.quantity = 1;
                items.Add(newItem);
            }

            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }

        return true;
    }
*/

