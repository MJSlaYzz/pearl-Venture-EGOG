using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CheckpointHandler : MonoBehaviour
{
    [HideInInspector] Animator animator;
    [HideInInspector] bool stillAnimationPlayed = false;
    [HideInInspector] bool stillAnimationCanBePlayed = false;
    [HideInInspector] bool flagAnimationCanBePlayed = false;

    [SerializeField] private SpriteRenderer minimapIconcurrenSprite;
    [SerializeField] private Sprite activeFlag;
    [SerializeField] private Sprite notActiveFlag;

    [SerializeField] bool mainmenu = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        minimapIconcurrenSprite.sprite = notActiveFlag;
        if (mainmenu)
        {
            StartStillAnimation();
        }
    }
    public void Update()
    {
        animator.SetBool("StillAnimation", stillAnimationCanBePlayed);
        animator.SetBool("FlagAnimation", flagAnimationCanBePlayed);

        MiniMapFlagController();
    }

    private void StartStillAnimation()
    {
        stillAnimationCanBePlayed = true;
        stillAnimationPlayed = true;
    }
    public void SwitchToFlagAnimation() // called from the event at the end of the still animation.
    {
        flagAnimationCanBePlayed = true;
        stillAnimationCanBePlayed = false;
    }
    public void ResetAnimation()
    {
        stillAnimationPlayed = false;
        stillAnimationCanBePlayed = false;
        flagAnimationCanBePlayed = false;
    }
    public void MiniMapFlagController()
    {
        if (minimapIconcurrenSprite != null)
        {
            if (flagAnimationCanBePlayed)
            {
                minimapIconcurrenSprite.sprite = activeFlag;
            }
            else
            {
                minimapIconcurrenSprite.sprite = notActiveFlag;
            }
        }
        else
        {
            Debug.Log("mini map sprite not found");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !stillAnimationPlayed)
        {
            //Debug.Log("Player colided");
            StartStillAnimation();
        }
    }
}
