using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Collectables")]
public class CollectableProfile : ScriptableObject
{
    public string ItemName;
    [TextArea(5, 10)]
    public string ItemDescription;
    public Sprite icon;


    /*TO BE ADDED: 
    Scripts deriving from this one specifying the type of collectable- ex. Fish, Spice...etc + contain respective relavent info.
    */

}