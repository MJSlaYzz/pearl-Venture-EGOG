using UnityEngine;

public class NoDeathMode : MonoBehaviour
{
    public OxygenSystem oxygenSystem;
    public GameObject sharks;
    public GameObject[] sharksSprite;
    public SharkAI[] sharkAIScript;
    //public PolygonCollider2D[] movingObstaclesColliders;
    //public EdgeCollider2D[] movingObstaclesWholeColliders;
    public GameObject[] movingObstacles;
    [HideInInspector] public bool noDeathMode = false;
    public bool oldSharks = true; // set it false if you are using the new sharks.

    public void NoDeathModeTog(bool DeathModeON)
    {
        if (DeathModeON)
        {
            noDeathMode = true;
            TurnOffObstacles();
            oxygenSystem.enabled = false;
            //oxygenSystem.gameObject.SetActive(false);
            if (oldSharks) 
            {
                sharks.gameObject.SetActive(false);
            }
            else if (!oldSharks)
            {
                RestartSharkScript(false);
            }
            Debug.Log("No Death Mode is On!");

        }
        else if (!DeathModeON)
        {
            noDeathMode = false;
            TurnOnObstacles();
            oxygenSystem.enabled = true;
            //oxygenSystem.gameObject.SetActive(true);
            if (oldSharks)
            {
                sharks.gameObject.SetActive(true);
            }
            else if (!oldSharks)
            {
                RestartSharkScript(true);
            }
            Debug.Log("No Death Mode is Off!");

        }
        void TurnOffObstacles()
        {

            for (int i = 0; i < movingObstacles.Length; i++)
            {
                PolygonCollider2D pCollider = movingObstacles[i].GetComponentInChildren<PolygonCollider2D>();
                EdgeCollider2D eCollider = movingObstacles[i].GetComponentInChildren<EdgeCollider2D>();
                pCollider.enabled = false;
                eCollider.enabled = false;
            }
            /*
            if(movingObstaclesColliders != null)
            {
                foreach (PolygonCollider2D collider in movingObstaclesColliders)
                {
                    collider.enabled = false;
                }
            }
            if(movingObstaclesWholeColliders != null)
            {
                foreach(EdgeCollider2D collider in movingObstaclesWholeColliders)
                {
                    collider.enabled = false;
                }
            }
            */
        }
        void TurnOnObstacles()
        {
            for (int i = 0; i < movingObstacles.Length; i++)
            {
                PolygonCollider2D pCollider = movingObstacles[i].GetComponentInChildren<PolygonCollider2D>();
                EdgeCollider2D eCollider = movingObstacles[i].GetComponentInChildren<EdgeCollider2D>();
                pCollider.enabled = true;
                eCollider.enabled = true;
            }
            /*
            if (movingObstaclesColliders != null)
            {
                foreach (PolygonCollider2D collider in movingObstaclesColliders)
                {
                    collider.enabled = true;
                }
            }
            if (movingObstaclesWholeColliders != null)
            {
                foreach (EdgeCollider2D collider in movingObstaclesWholeColliders)
                {
                    collider.enabled = true;
                }
            }
            */
        }
        void RestartSharkScript(bool TurnOn)
        {
            if(sharksSprite != null && sharkAIScript != null)
            {
                if (TurnOn)
                {
                    for (int c = 0; c < sharkAIScript.Length; c++)
                    {
                        sharkAIScript[c].enabled = true;
                    }
                    for (int i = 0; i < sharksSprite.Length; i++)
                    {
                        sharksSprite[i].SetActive(true);
                    }
                }
                else if (!TurnOn)
                {
                    for (int c = 0; c < sharkAIScript.Length; c++)
                    {
                        sharkAIScript[c].enabled = false;
                    }
                    for (int i = 0; i < sharksSprite.Length; i++)
                    {
                        sharksSprite[i].SetActive(false);
                    }
                }
            }
            else
            {
                print("sharksSprite or/and sharkAIScript can't be found");
            }
            

        }
    }
}
