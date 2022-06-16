using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    float horizontalMove = 0f;
    public float runSpeed = 40f;

    bool jump = false;
    bool isGrounded = false;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        
        if (Input.GetButtonDown("Jump") && isGrounded == true){
                jump = true;
                isGrounded = false;
                animator.SetBool("isJumping", true);                
        }
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
            jump = false;
    }
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Platform")){
            isGrounded = true;
        }
    }
}