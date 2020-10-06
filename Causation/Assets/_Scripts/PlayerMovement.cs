using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb; //rigidbody used for jumping

    [SerializeField]
    //Player's running speed
    //An acceleration function might be cool, hold a horizontal direction down longer and you move faster?
    private int runSpeed = 5;  

    [SerializeField]
    //Jumping
    private float jumpForce = 300;
    private float axisY;
    public bool isJumping;

    //Movement Axes stash, vertical could go here as well if there was going to be vertical movement like a beat em up
    private float horizontal;
    private float vertical;

    //Animation and design
    private bool facingRight;

    public GameObject nearObject;

    private bool isCrouched;

    void Awake()
    {
        axisY = gameObject.transform.position.y; //axisY is set immediately to the player game object's y pos
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //rb.Sleep();
    }

    // Update is called once per frame
    void Update()
    {
        //Tighter, specific controls might be better here in order to set the speed to 0 immediately when the key is lifted (an abrupt end to the animation)
        horizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontal != 0 ? horizontal : 0)); //ternary, think of it like a boolean: (is horizontal != 0? if true then horizontal value :else 0)
    }

    public void OnLanding() 
    {
        isJumping = false;
        animator.SetBool("IsJumping", false);

        //rb.gravityScale = 0f;
        //rb.Sleep();
        //transform.position = new Vector3(gameObject.transform.position.x, axisY);
        //Debug.Log("Landed at " + axisY);
    }

    private void FixedUpdate()
    {
        if (horizontal != 0 && !isCrouched)
        {
            Vector3 movement = new Vector3(horizontal * runSpeed, 0.0f, 0.0f);
            transform.position = transform.position + movement * Time.deltaTime;
        }
        Flip(horizontal);

        //reminder, axisY is the y coordinate that the player jumped from so once the player falls back down and their y position is less than or equal to it will stop falling 
        if (rb.velocity.y == 0 && isJumping) //this doesn't make any sense, if it does 
        {
            OnLanding();
        }
        //jumping isn't perfect, the player model will continuously go down instead of staying at the value that it jumped from

        if (Input.GetButton("Jump") && !isJumping && !isCrouched)
        {
            animator.SetBool("IsJumping", true);
            axisY = transform.position.y;
            Debug.Log("Jumped at " + axisY);
            isJumping = true;
            rb.gravityScale = 1.5f;
            rb.WakeUp();
            rb.AddForce(new Vector2(0, jumpForce));
        }

        vertical = Input.GetAxis("Vertical");
        if (vertical < 0)
        {
            //make it so movement is stopped or forced into crouch walk
            animator.SetBool("IsCrouched", true);
            isCrouched = true;
        }
        else if (Input.GetAxis("Vertical") > -1)
        {
            //the "exit" time when not pressing the up key is too slow right now
            animator.SetBool("IsCrouched", false);
            isCrouched = false;
        }
    }

    //Handles flipping the sprite across the x axis to show that movement direction has changed
    private void Flip(float horizontal)
    {
        if (horizontal < 0 && !facingRight || horizontal > 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
        }
    }
}
