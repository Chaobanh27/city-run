using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;
    private Animator playerAnimation;
    private SpriteRenderer playerSpriteRenderer;
    [HideInInspector] public bool playerUnlocked;
    [HideInInspector] public bool extraLife;

    [Header("Speed")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedToSurvive = 6;
    private float defaultSpeed;

    [Space]
    [SerializeField] private float milestoneIncrease;
    [SerializeField] private float speedMilestone;
    private float defaultMilestoneIncrease;


    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    private bool canDoubleJump;

    [Header("Slide")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTime;
    [SerializeField] private float slideCooldown;
    private float slideCooldownCounter;
    private bool isSliding;
    private float slideTimeCounter;

    [Header("Collision")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float cellingCheckDistance;
    [SerializeField] private LayerMask groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;

    private bool isWall;
    private bool isCelling;
    private bool isGround;
    public bool isDead;

    [HideInInspector] public bool isLedge;

    [Header("Ledge")]
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;

    private Vector2 climbStartPosition;
    private Vector2 climbOverPosition;

    private bool canGrabLedge = true;
    private bool isClimbing;

    [Header("Knockback")]
    [SerializeField] private Vector2 knockbackDirection;
    private bool isKnockback;
    private bool canBeKnockBack = true;

    [Header("VFX")]
    [SerializeField] private ParticleSystem dustFX;
    private bool isLanding;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        speedMilestone = milestoneIncrease;
        defaultSpeed = moveSpeed;
        defaultMilestoneIncrease = milestoneIncrease;

    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
        AnimationController();
        slideTimeCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;

        if (moveSpeed >= speedToSurvive)
        {
            extraLife = true;

        }

        if (isKnockback) return;
        if (isDead) return;
        if (playerUnlocked)
        {
            Movement();
        }

        CheckLanding();
        SpeedController();
        CheckForLedge();
        InputController();
        CheckForSlide();

    }

    private void CheckLanding()
    {
        if (playerRigidbody.velocity.y < -5 && !isGround)
        {
            isLanding = true;
        }
        else if (isLanding && isGround)
        {
            dustFX.Play();
            isLanding = false;
        }
    }

    public void Damage()
    {
        if(extraLife)
        {
            KnockBack();
        }
        else
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Invincibility()
    {
        Color originalColor = playerSpriteRenderer.color;
        Color darkerColor = new Color(playerSpriteRenderer.color.r, playerSpriteRenderer.color.g, playerSpriteRenderer.color.b, .5f);


        canBeKnockBack = false;
        playerSpriteRenderer.color = darkerColor;
        yield return new WaitForSeconds(.1f);

        playerSpriteRenderer.color = originalColor;
        yield return new WaitForSeconds(.1f);

        playerSpriteRenderer.color = darkerColor;
        yield return new WaitForSeconds(.15f);

        playerSpriteRenderer.color = originalColor;
        yield return new WaitForSeconds(.15f);

        playerSpriteRenderer.color = darkerColor;
        yield return new WaitForSeconds(.25f);

        playerSpriteRenderer.color = originalColor;
        yield return new WaitForSeconds(.25f);

        playerSpriteRenderer.color = darkerColor;
        yield return new WaitForSeconds(.3f);

        playerSpriteRenderer.color = originalColor;
        yield return new WaitForSeconds(.35f);

        playerSpriteRenderer.color = darkerColor;
        yield return new WaitForSeconds(.4f);

        playerSpriteRenderer.color = originalColor;
        canBeKnockBack = true;
    }

    private void KnockBack()
    {
        if(!canBeKnockBack) return;

        StartCoroutine(Invincibility());
        isKnockback = true;
        playerRigidbody.velocity = knockbackDirection;
    }

    private IEnumerator Die()
    {
        AudioManagerController.instance.PlaySFX(0);
        isDead = true;
        canBeKnockBack = false;
        playerRigidbody.velocity = knockbackDirection;
        playerAnimation.SetBool("isDead", true);

        Time.timeScale = .6f;

        yield return new WaitForSeconds(1f);

        Time.timeScale = 1f;
        playerRigidbody.velocity = new Vector2(0, 0);
        GameManager.instance.GameEnded();
    }

    private void CancelKnockBack()
    {
        isKnockback = false;
    }

    private void SpeedReset()
    {
        if(isSliding)
        {
            return;
        }
        moveSpeed = defaultSpeed;
        milestoneIncrease = defaultMilestoneIncrease;
    }

    private void SpeedController()
    {
        if(moveSpeed == maxSpeed)
        {
            return;
        }

        if(transform.position.x > speedMilestone)
        {
            speedMilestone += milestoneIncrease; 
            moveSpeed *= speedMultiplier;
            milestoneIncrease *= speedMultiplier;
            if(moveSpeed > maxSpeed)
            {
                moveSpeed = maxSpeed;
            }
        }
    }
    private void CheckForLedge()
    {
        if(isLedge && canGrabLedge)
        {
            canGrabLedge = false;

            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;
            climbStartPosition = ledgePosition - offset1;
            climbOverPosition = ledgePosition + offset2;
            isClimbing = true;
        }

        if (isClimbing)
        {
            transform.position = climbStartPosition;
        }
    }

    private void LedgeClimbOver()
    {
        isClimbing = false;
        transform.position = climbOverPosition;
        Invoke("AllowLedgeGrab", .1f);
    }

    private void AllowLedgeGrab()
    {
        canGrabLedge = true;
    }

    private void InputController()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Slide();
        }

    }

    public void Slide()
    {
        if (isDead) return;

        if (playerRigidbody.velocity.x != 0 && slideCooldownCounter < 0)
        {
            dustFX.Play();
            isSliding = true;
            slideTimeCounter = slideTime;
            slideCooldownCounter = slideCooldown;
        }
    }

    private void AnimationController()
    {
        playerAnimation.SetBool("canDoubleJump", canDoubleJump);
        playerAnimation.SetBool("isGround", isGround);
        playerAnimation.SetBool("isSliding", isSliding);
        playerAnimation.SetBool("isClimbing", isClimbing);
        playerAnimation.SetFloat("xVelocity", playerRigidbody.velocity.x);
        playerAnimation.SetFloat("yVelocity", playerRigidbody.velocity.y);
        playerAnimation.SetBool("isKnockBack", isKnockback);

        if (playerRigidbody.velocity.y < -8)
        {
            playerAnimation.SetBool("isRolling", true);
        }

    }

    private void RollAnimFinished()
    {
        playerAnimation.SetBool("isRolling", false);
    }

    private void CheckForSlide()
    {
        if (slideTimeCounter < 0 && !isCelling)
        {
            isSliding = false;
        }

    }

    private void Movement()
    {
        if (isWall)
        {
            //SpeedReset();
            return;
        }


        if(isSliding)
        {
            playerRigidbody.velocity = new Vector2(slideSpeed, playerRigidbody.velocity.y);
        }
        else
        {
            playerRigidbody.velocity = new Vector2(moveSpeed, playerRigidbody.velocity.y);
        }
    }

    public void Jump()
    {
        if (isDead) return;
        if (isSliding) return;

        RollAnimFinished();

        if (isGround)
        {
            dustFX.Play();
            AudioManagerController.instance.PlaySFX(UnityEngine.Random.Range(2, 3));
            canDoubleJump = true;
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, doubleJumpForce);
        }
    }
    private void CheckCollision()
    {
        isGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundCheck);
        isCelling = Physics2D.Raycast(transform.position, Vector2.up, cellingCheckDistance, groundCheck);
        isWall = Physics2D.BoxCast(wallCheck.position, wallCheckSize,0, Vector2.zero, 0, groundCheck );

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + cellingCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }


}
