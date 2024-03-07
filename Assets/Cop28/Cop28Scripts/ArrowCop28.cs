using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCop28 : MonoBehaviour
{
    #region
    [HideInInspector] private ShootingCop28 shootingCop28;
    [HideInInspector] private Rigidbody2D rb;
    [HideInInspector] public bool hasHit;
    [HideInInspector] private PlayerMovementFour playerMovementFour;
    [SerializeField] private bool grabbing = false;
    [SerializeField] private GameObject grabbedTrash;
    [HideInInspector] private AudioSource trashAudioSource;
    [HideInInspector] private Cop28DataManager cop28DataManager;
    //[HideInInspector] private BoxCollider2D collider2D;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        cop28DataManager = FindObjectOfType<Cop28DataManager>();
        rb = GetComponent<Rigidbody2D>();
        shootingCop28 = FindObjectOfType<ShootingCop28>();
        //PlayerMovementFour = FindObjectOfType<PlayerMovementFour>();
        //collider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GrabTrash();
        //Debug.Log("hasHit : " + hasHit);
        if (hasHit == false)
        {

            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            if (rb.velocity.x < 0)
            {
                angle += 180;
            }
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }

    }
    private void GrabTrash()
    {
        if (grabbing && grabbedTrash != null)
        {
            print("GRABBING!");
            if(shootingCop28.disToPlayer > 0.3)
            {
                grabbedTrash.gameObject.transform.position = transform.position;
            }
            else
            {
                grabbing = false;
                //Destroy(grabbedTrash.gameObject);
                trashAudioSource = GameObject.Find("Trash SFX").GetComponent<AudioSource>();
                if(trashAudioSource != null)
                {
                    trashAudioSource.Play();
                }
                else
                {
                    Debug.Log("ERORR: Can't Find TrashAudioSource");
                }
                grabbedTrash.SetActive(false);
                // add points
                cop28DataManager.AddPoints(10);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Call: Back to starting postion
        if (collision.gameObject.tag == "Trash")
        {
            // Grab the trash
            print("FOUND TRASH!");
            grabbedTrash = collision.gameObject;
            grabbing = true;
        }
        if (collision.gameObject.tag == "Wall")
        {
            if(collision.gameObject.layer != LayerMask.NameToLayer("Hidden Wall")) // I have changed all the layers for the hidden walls (player-shark-checkpoint)
            {
                shootingCop28.forceCallArrowBack = true;
                //Debug.Log("Force Call Arrow Back");
            }
        }
    }
}
