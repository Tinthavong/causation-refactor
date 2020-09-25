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
    float axisY;
    bool isJumping;

    //Movement Axes stash, vertical could go here as well if there was going to be vertical movement like a beat em up
    float horizontal;
    float vertical;

    //Animation and design
    private bool facingRight;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.Sleep();
    }

    // Update is called once per frame
    void Update()
    {
        //Tighter, specific controls might be better here in order to set the speed to 0 immediately when the key is lifted (an abrupt end to the animation)
        horizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontal != 0 ? horizontal : vertical)); //ternary, think of it like a boolean: (is horizontal != 0? if true then horizontal :else vertical)
        
    }

    private void FixedUpdate()
    {
        if(horizontal != 0)
        {
            Vector3 movement = new Vector3(horizontal * runSpeed, 0.0f, 0.0f);
            transform.position = transform.position + movement * Time.deltaTime;
        }
        Flip(horizontal);
    }

    private void Flip(float horizontal)
    {
        if(horizontal < 0 && !facingRight || horizontal > 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
        }
    }
}
