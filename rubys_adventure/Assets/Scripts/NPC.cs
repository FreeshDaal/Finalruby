using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    public AudioClip dialogAppearSound; 
    private AudioSource audioSource; 
    float timerDisplay;
    float dialogCooldown = 10.0f;
    bool canDisplayDialog = true;
    private RubyController rubyController; 

    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        rubyController = FindObjectOfType<RubyController>();

        audioSource = gameObject.AddComponent<AudioSource>(); 
    }

    void Update()
    {
        if (!canDisplayDialog)
        {
            dialogCooldown -= Time.deltaTime;
            if (dialogCooldown <= 0)
            {
                canDisplayDialog = true;
                dialogCooldown = 10.0f; 
            }
        }
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        if (rubyController != null && canDisplayDialog)
        {
            
            if (rubyController.health < rubyController.maxHealth)
            {
                rubyController.ChangeHealth(1);
            }
            
            canDisplayDialog = false;
        }
                if (dialogAppearSound != null)
        {
            audioSource.PlayOneShot(dialogAppearSound);
        }


        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }
}