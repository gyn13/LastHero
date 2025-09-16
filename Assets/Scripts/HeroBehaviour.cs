using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HeroBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    Animator animator;

    [Header("Movement")]
    [SerializeField] private float speed;
    private float horizontal;
    
    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    
    [Header("Roll")]
    [SerializeField] private float rollSpeed = 20f;
    [SerializeField] private float rollDuration = 0.1f;
    [SerializeField] private float rollCooldown = 0.1f;
    private bool isRolling;
    private bool canRoll = true;
    
    bool isFacingRight = false;
    
    private bool isGrounded = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Jump();
        Roll();
    }
    
    void FixedUpdate()
    { 
        if (isRolling)
        {
            return;
        }
        
        Move();
        FlipSprite();
    }

    void FlipSprite()
    {
        if (isFacingRight && horizontal > 0f || !isFacingRight && horizontal < 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }
    
    void Move()
    {
        horizontal = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
        }
    }

    void Roll()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canRoll && isGrounded)
        {
            StartCoroutine(RollCoroutine());
        }
    }

    private IEnumerator RollCoroutine()
    {
        canRoll = false;
        isRolling = true;
        
        float rollDirection = isFacingRight ? -1f : 1f;
        
        //Roll move
        rb.linearVelocity = new Vector2(rollDirection *  rollSpeed, rb.linearVelocity.y);
        animator.SetBool("isRolling", true);

        yield return new WaitForSeconds(rollDuration);

        //Reset horizontal velocity
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        
        isRolling = false;

        yield return new WaitForSeconds(rollCooldown);
        canRoll = true;
        
        animator.SetBool("isRolling", false);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        isGrounded = true;
        animator.SetBool("isJumping", !isGrounded);
    }
}