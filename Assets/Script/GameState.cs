using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public GameObject character;
    public GameObject tileTabs;
    public GameObject coinCounter, timeCounter, healthCounter, keyIcon, potionIcon;
    public GameObject stopButton, pauseButton, panel;

    private CharacterController2D controller;
    private PlayerMovement movement;
    private Animator playerAnimator;
    private SaveHandler saveHandler;
    
    public GameObject[] enemies;
    public GameObject[] items;

    private bool isPaused;
    public bool isPlay = false;

    public GameObject particle1, particle2;

    private void Start() {
        controller = character.GetComponent<CharacterController2D>();
        movement = character.GetComponent<PlayerMovement>();
        playerAnimator = character.GetComponent<Animator>();
        saveHandler = GetComponent<SaveHandler>();
        
        if(isPlay == false){
            if(particle1 != null && particle2 != null)
            particle1.GetComponent<ParticleSystem>().Stop();
            particle2.GetComponent<ParticleSystem>().Stop();
        }
    }
    

    void FixedUpdate() {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        items = GameObject.FindGameObjectsWithTag("Items");
    }
        
    void LateUpdate() {
        if(movement.currentHealth <= 0){
            StopPlaying();
            movement.currentHealth = 3;
        }
    }

    public void BtnPlay()
    {
        isPlay = true;

        GetComponentEnemy(true);

        controller.enabled = true;
        movement.enabled = true;
        playerAnimator.enabled = true;
        
        tileTabs.SetActive(false);
        coinCounter.SetActive(true);
        timeCounter.SetActive(true);
        healthCounter.SetActive(true);
        pauseButton.SetActive(true);
        stopButton.SetActive(true);
        panel.SetActive(false);

        saveHandler.OnSave();
    }

    public void PauseGame(){
        Time.timeScale = 0f;
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void StopPlaying()
    {   
        isPlay = false;
        
        GetComponentEnemy(false);
        
        controller.enabled = false;
        movement.enabled = false;
        playerAnimator.enabled = false;

        tileTabs.SetActive(true);
        coinCounter.SetActive(false);
        timeCounter.SetActive(false);
        healthCounter.SetActive(false);
        pauseButton.SetActive(false);
        panel.SetActive(true);
        stopButton.SetActive(false);

        particle1.GetComponent<ParticleSystem>().Stop();
        particle2.GetComponent<ParticleSystem>().Stop();
    }

    public void GetComponentEnemy(bool isEnabled){
        foreach(GameObject e in enemies){
            if(e != null){
                if(e.TryGetComponent(out MushroomBehaviour mushroom)){
                    mushroom.enabled = isEnabled;
                    e.GetComponent<Animator>().enabled = isEnabled;
                    if(isEnabled == true){
                        e.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    } else {
                        e.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    }
                } else if(e.TryGetComponent(out FrogBehaviour frog)){
                    frog.enabled = isEnabled;
                    e.GetComponent<Animator>().enabled = isEnabled;
                    if(isEnabled == true){
                        e.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    } else {
                        e.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    }
                } else if(e.TryGetComponent(out Animator animator)){ 
                     animator.enabled = isEnabled;                
                }
            }
        }
    }

    public void GetComponentExtras(bool isEnabled){
        

        
    }
    
     
}
