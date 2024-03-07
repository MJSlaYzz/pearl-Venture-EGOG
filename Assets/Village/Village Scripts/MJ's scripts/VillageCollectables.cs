using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collectable List", menuName = "Collectable List")]
public class VillageCollectables : ScriptableObject
{

    [System.Serializable]
    public class Details
    {
        public string collectableName;
        [TextArea(5, 10)] public string collectableDescrption;
        //public Sprite collectableSprite;
        public GameObject collectablePrefab;

    }

    [Header("Collectables List: ")]
    public Details[] details;

    void CollectableChecker()
    {
        for (int i = 0; i < details.Length; i++)
        {
            if(details[i].collectableName != string.Empty && details[i].collectableDescrption != string.Empty && details[i].collectablePrefab != null)
            {
                Debug.Log(details[i] + "Is a valid collectable!");
            }
            else
            {
                Debug.Log(details[i] + "Is not a valid collectable!");
            }
        }
    }

}
