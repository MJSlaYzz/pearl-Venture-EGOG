using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private GameObject itemToPickUp;
    public Item item;
    private bool isCollected = false;
    private void Start()
    {
        itemToPickUp = this.gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isCollected == false)
        {
            if (item != null)
            {
                Debug.Log("Picking up item: " + item.name);

                Item copyItem = Instantiate(item);
                bool wasCollected = Inventory.instance.Add(copyItem);
                isCollected = true;

                if (wasCollected)
                {
                    Destroy(itemToPickUp);
                    Debug.Log("Item collected and destroyed.");
                }
            }
        }
    }

}




/*
private void OnTriggerStay2D(Collider2D collision)
{
    var itemColor = item.GetComponent<SpriteRenderer>().color;
    if (collision.gameObject.tag == "Player" && isCollected == false)
    {

        //Here should adds 1+ mushrrom to the manager or inventory script.


        if (itemColor.a != 0f)
        {
            itemColor.a = 0f; //hides or destroy this object
            item.GetComponent<SpriteRenderer>().color = itemColor;
        }
        isCollected = true;
        Debug.Log("Add one more shroom!!");
    }
}
*/