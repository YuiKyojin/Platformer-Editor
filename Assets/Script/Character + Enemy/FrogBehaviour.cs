using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBehaviour : MonoBehaviour
{
    public bool facingRight;
    public LayerMask whatIsGround;

    public bool isGrounded;
    public bool isFalling;
    public bool isJumping;

    public float jumpForceX = 2f;
    public float jumpForceY = 4f;

    public float lastYPosition = 0;

    public enum Animations{
        Idle = 0,
        Jumping = 1,
        Falling = 2
    };
    
    public Animations currentAnim;

    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Animator anim;

    public float idleTime = 2f;
    public float currentIdleTime = 0;
    public bool isIdle = true;

    // Start is called before the first frame update
    void Start()
    {
        lastYPosition = transform.position.y;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        if(isIdle == true){
            currentIdleTime += Time.deltaTime;
            if(currentIdleTime >= idleTime){
                currentIdleTime = 0;
                Jump();                
                facingRight = !facingRight;
                spriteRenderer.flipX = facingRight;
            }
        }

        isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f), 
        new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f), whatIsGround);

        if(isGrounded && !isJumping){
            isFalling = false;
            isJumping = false;
            isIdle = true;
            ChangeAnimation(Animations.Idle);
        } else if(transform.position.y > lastYPosition && !isGrounded && !isIdle){
            isJumping = true;
            isFalling = false;
            ChangeAnimation(Animations.Jumping);
        } else if(transform.position.y < lastYPosition && !isGrounded && !isIdle){
            isJumping = false;
            isFalling = true;
            ChangeAnimation(Animations.Falling);
        }
        lastYPosition = transform.position.y;
    }

    void Jump(){
        isJumping = true;
        isIdle = false;
        int direction = 0;

        if(facingRight == true){
            direction = 1;
        } else{
            direction = -1;
        }

        rb.velocity = new Vector2(jumpForceX * direction, jumpForceY);
    }

    void ChangeAnimation(Animations newAnim){
        if (currentAnim != newAnim){
            currentAnim = newAnim;
            anim.SetInteger("state", (int)newAnim);
        }
    }
}
