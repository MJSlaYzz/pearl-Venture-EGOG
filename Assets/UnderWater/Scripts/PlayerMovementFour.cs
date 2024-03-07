using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementFour : MonoBehaviour
{
    #region Variables
    // Movement Variables
    [SerializeField] public float moveSpeed = 5f;
    [HideInInspector] private Animator animator;
    public bool isLookingRight = true;
    public Vector2 movement;

    // Dash Variables.
    [HideInInspector] private Image dashBarFill;
    [SerializeField] private Gradient dashGradient;
    [SerializeField] public float dashSpeed = 600f;
    //[HideInInspector] public float dashLength = 0.5f;
    [SerializeField] public float dashCooldown = 5f;
    [HideInInspector] public bool dashPlaying = false;
    [HideInInspector] private bool canDash = true;
    [HideInInspector] public Rigidbody2D rb;
    // Dash Ghost Effect
    [HideInInspector] private DashGhostEffect ghostEffect;
    [HideInInspector] private Slider dashSlider;
    [HideInInspector] private float sliderValue;
    // Dash Audio
    [HideInInspector] public AudioSource playerAudioSource;
    [SerializeField] private AudioClip dashNotReadyAudio;
    [SerializeField] private AudioClip dashReadyAudio;
    [HideInInspector] bool dashReadyAudioPlayed = true;
    [HideInInspector] bool dashErrorAudioCanPlay = true;
    [HideInInspector] float dashErrorTimer;
    // Shooting
    [HideInInspector] private bool isFighting = false;
    [HideInInspector] private Shooting shooting;
    [HideInInspector] private ShootingCop28 shootingCop28;
    [SerializeField] public bool isCop28 = false;
    // Cutting
    [HideInInspector] public bool isCutting = false;
    [HideInInspector] public bool playerCanMove = true; // to replace the disabling of this script


    #endregion
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ghostEffect = GetComponent<DashGhostEffect>();
        dashSlider = GameObject.Find("DashBar").GetComponent<Slider>();
        dashBarFill = dashSlider.GetComponentInChildren<Image>();
        playerAudioSource = GetComponent<AudioSource>();
        sliderValue = dashCooldown;
        dashSlider.maxValue = dashCooldown;
        if (isCop28)
        {
            shootingCop28 = FindObjectOfType<ShootingCop28>();
        }
        else
        {
            shooting = GetComponent<Shooting>();
        }

    }
    private void Update()
    {
        if (playerCanMove)
        {
            if (shooting != null && !isCop28)
            {
                isFighting = shooting.isFighting;
            }
            if (shootingCop28 != null && isCop28)
            {
                isFighting = shootingCop28.isFighting;
            }
            Dash();
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
            animator.SetBool("isLookingRight", isLookingRight);
            animator.SetBool("isFighting", isFighting);
            animator.SetBool("isCutting", isCutting);
        }
        if (isCutting)
        {
            StartCoroutine(CheckCutting());
        }

    }
    private void FixedUpdate()
    {
        if (playerCanMove)
        {
            //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            Movement();
            DirectionCheck();
        }
    }
    void Movement()
    {
        if (!dashPlaying && !isFighting && !isCutting)
        {
            Vector2 movementDirection = new Vector2(movement.x, movement.y);
            float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude); //store the value of the Direction and clamp it before normalize it.
            movementDirection.Normalize();

            transform.Translate(movementDirection * moveSpeed * inputMagnitude * Time.deltaTime, Space.World);
            //specifying that we are moving the character relative to the world by typing Space.World.
        }
    }
    void DirectionCheck()
    {
        if (shootingCop28 != null && isCop28)
        {
            if (!shootingCop28.arrowGotShot)
            {
                if (movement.x > 0.01)
                {
                    isLookingRight = true;
                    //Debug.Log(" player is Looking Right");
                }
                else if (movement.x < -0.01)
                {
                    isLookingRight = false;
                    //Debug.Log(" player is Looking Left");
                }
            }
        }
        else
        {
            if (movement.x > 0.01)
            {
                isLookingRight = true;
                //Debug.Log(" player is Looking Right");
            }
            else if (movement.x < -0.01)
            {
                isLookingRight = false;
                //Debug.Log(" player is Looking Left");
            }
        }

    }
    void Dash()
    {
        dashSlider.value = sliderValue;
        dashBarFill.color = dashGradient.Evaluate(dashSlider.normalizedValue);
        //Debug.Log(sliderValue);
        if (Input.GetKeyDown(KeyCode.Space) && movement != Vector2.zero && !isFighting)
        {
            if (canDash == false)
            {
                if (dashErrorAudioCanPlay) 
                {
                    playerAudioSource.PlayOneShot(dashNotReadyAudio);
                    dashErrorAudioCanPlay = false;
                    dashErrorTimer = 0.3f;
                }
            }
            else
            {
                playerAudioSource.Play();
                //Debug.Log("Player Dashing");
                ghostEffect.instantiateGhostEffect = true;
                movement.Normalize();
                dashSpeed = Mathf.Clamp(dashSpeed,0, dashSpeed); // not sure about it.
                rb.velocity = movement * dashSpeed * Time.deltaTime;
                sliderValue = 0f;
                dashPlaying = true;
                canDash = false;
                dashReadyAudioPlayed = false;
            }
        }
        else
        {
            dashPlaying = false;

        }
        if (sliderValue < dashCooldown)
        {
            sliderValue += Time.deltaTime;
        }
        else
        {
            //Debug.Log("Player Can Dash");
            if (!dashReadyAudioPlayed)
            {
                playerAudioSource.PlayOneShot(dashReadyAudio);
                dashReadyAudioPlayed = true;
            }
            canDash = true;
        }
        if (dashErrorTimer > 0)
        {
            dashErrorTimer -= Time.deltaTime;
        }
        else
        {
            dashErrorAudioCanPlay = true;
        }
    }
    public void ResetDashVariables()
    {
        sliderValue = dashCooldown;
        dashSlider.value = sliderValue;
        canDash = true;
    }
    IEnumerator CheckCutting()
    {
        //print("Is cutting is true start disabling it");
        yield return new WaitForSeconds(0.5f);
        isCutting = false;
    }
}
