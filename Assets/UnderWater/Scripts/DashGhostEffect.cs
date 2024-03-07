using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashGhostEffect : MonoBehaviour
{
    #region Variables
    [HideInInspector] public float ghostDelay = 0.1f;
    [HideInInspector] private float ghostDelaySeconds;

    [HideInInspector] private float dashCD = 0.09f;
    [HideInInspector] private float dashCDSeconds;

    [SerializeField] private GameObject ghostEffectPrefab;
    [HideInInspector] public bool instantiateGhostEffect = false;
    [HideInInspector] private int clonesCount;
    [HideInInspector] private int maxClones = 4;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        ghostDelaySeconds = ghostDelay;
        dashCDSeconds = dashCD;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(dashCDSeconds);
        GhostEffect();
    }
    void GhostEffect()
    {
        if (ghostDelaySeconds > 0)
        {
            ghostDelaySeconds -= Time.deltaTime;
        }
        else
        {
            if (instantiateGhostEffect && dashCDSeconds > 0f) 
            {
                // generate a ghost
                dashCDSeconds -= Time.deltaTime;
                if(clonesCount < maxClones)
                {
                    GameObject currenGhost = Instantiate(ghostEffectPrefab, transform.position, transform.rotation);
                    Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                    currenGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                    ghostDelaySeconds = ghostDelay;
                    Destroy(currenGhost, 1f);
                    clonesCount++;
                }
            }
        }
        if(dashCDSeconds <= 0f) 
        {
            instantiateGhostEffect = false;
            dashCDSeconds = dashCD;
            clonesCount = 0;
        }
    }
}
