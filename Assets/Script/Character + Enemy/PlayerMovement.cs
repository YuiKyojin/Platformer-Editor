using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public GameObject manager;
    private GameState gameState;

    float horizontalMove = 0f;
    public float runSpeed = 100f;

    public bool jump = false;
    public bool isGrounded = true;

    public int playerHealth = 3;
    public int currentHealth;

    bool isInvincible = false;

    public GameObject blink;
    public TMP_Text healthText;

    private Rigidbody2D rb;

    private float jumpTime;
    private float jumpTimeCounter;
    private bool isJumping = false;
    private float jumpForce = 10f;

    void Awake() {
        currentHealth = playerHealth;
        rb = gameObject.GetComponent<Rigidbody2D>();
        manager = GameObject.Find("Manager");
        gameState = manager.GetComponent<GameState>();
    }

    void Update()
    {
        if(healthText.text != currentHealth.ToString()){
            healthText.text = currentHealth.ToString();
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        
        if (Input.GetButtonDown("Jump") && isGrounded == true){
            isGrounded = false;
            isJumping = true;
            jumpTimeCounter = jumpTime;
            jump = true;
            animator.SetBool("isJumping", true);                
        }

        if(isJumping == true){
            if(jumpTimeCounter > 0){
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter  -= Time.deltaTime;
            }
        }

        if(Input.GetButtonDown("Jump")){
            isJumping = false;
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

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy" && isInvincible == false && isGrounded == true){
            currentHealth -= 1;
            SetInvincible(3);
            Invoke("EnableBlink", 0f);
            Invoke("DisableBlink", 0.1f);
        }
    }

    public void SetInvincible(float invincibleTime){
        isInvincible = true;

        StartCoroutine(COSetInvincible(invincibleTime));
    }

    void SetDamageable(){
        isInvincible = false;
    }

    public IEnumerator COSetInvincible (float invincibleTime){
        yield return new WaitForSeconds(invincibleTime);
        SetDamageable();
    }

    private void EnableBlink(){
        blink.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void DisableBlink(){
        blink.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnBecameInvisible() {
        if(this.gameObject.tag == "Player"){
            if(gameState.isPlay == true){
                gameState.StopPlaying();
            }
        }
    }
}
