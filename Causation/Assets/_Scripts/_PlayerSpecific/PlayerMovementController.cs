using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    //Note to self while refactoring; the controller should handle: Player input, physics/collision, and gizmos alongside typical actions.
    [Header("Controller Stats")]
    public bool isFacingRight = true;
    //Player's running speed
    public float moveSpeed = 10f;
    public float maxSpeed = 7f;
    public Vector2 direction;

    //Jumping
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f;
    public float jumpTimer;

    [Header("Platform Behavior")]
    public LayerMask groundLayer;
    public bool onGround = false;
    public float groundLength = 0.6f;
    public Vector3 colliderOffset;

    [Header("Physics Behavior")]
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;

    PlayerStateManager psm;
    public Animator animator;
    Rigidbody2D rb;

    private void Start()
    {
        psm = GetComponent<PlayerStateManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
        if (psm.isControlling)
        {
            //Jump
            if (Input.GetButtonDown("Jump") && !Input.GetKey(KeyCode.S))
            {
                jumpTimer = Time.time + jumpDelay;
            }

            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

            CrouchDown();
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
    private void FixedUpdate()
    {
        if (!psm.isCrouched)
        {
            MoveCharacter(direction.x);
        }

        if (jumpTimer > Time.time && onGround && !psm.isCrouched)
        {
            Jump();
        }
        ModifyPhysics();
    }

    void CrouchDown()
    {
        if (Input.GetKeyDown(KeyCode.S) && onGround)
        {
            animator.Play("Crouch");
            animator.SetBool("IsCrouched", true);
            psm.isCrouched = true;
            rb.velocity = new Vector2(0f, 0f);
            if (psm.isCrouched)
            {
                GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 1.45f);
                GetComponent<CapsuleCollider2D>().offset = new Vector2(0f, -0.4f);
            }
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("IsCrouched", false);
            psm.isCrouched = false;
            GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 2.3f);
            GetComponent<CapsuleCollider2D>().offset = new Vector2(0f, 0f);
        }
    }

    void MoveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);
        if ((horizontal > 0 && !isFacingRight) || (horizontal < 0 && isFacingRight))
        {
            Flip();
        }

        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
    }

    //Flips character sprite
    public void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.rotation = Quaternion.Euler(0, isFacingRight ? 0 : 180, 0);
    }

    void Jump()
    {
        animator.Play("Jump");
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
    }

    //Utilizes parameters in the inspector to modify player character movement
    void ModifyPhysics()
    {
        bool changingDirection = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround)
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirection)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }
}

