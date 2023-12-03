using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;

    public GameObject projectilePrefab;

    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip coinSound;
    public AudioClip victory;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreCoin;
    public TextMeshProUGUI end;
    public GameObject bg;

    public int score = 0;
    public int scoreThreshold = 3;
    public int coins = 0;
    public int coinsThreshold = 5;

    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    public ParticleSystem damageEffect;
    public ParticleSystem healEffect;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);


    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        damageEffect.GetComponent<ParticleSystem>().enableEmission = false;
        healEffect.GetComponent<ParticleSystem>().enableEmission = false;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    { 
        
             if (health <= 0)
                {
                     if (end != null)
                    {
                        bg.gameObject.SetActive(true);
                        end.gameObject.SetActive(true);
                        end.text = "The robots got away! Press 'R' key to restart.";

                    }
                        if (Input.GetKeyDown(KeyCode.R))
                         {
                            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                          }
                 }
        if (score >= 3)
            {scoreText.text = "Complete!";
            }
        if (coins >= 5)
            {scoreCoin.text = "Complete!";
            }

             if (score >= 3 && coins >= 5)
                {
                 if (end != null)
                    {
                        PlaySound(victory);
                        bg.gameObject.SetActive(true);
                        end.gameObject.SetActive(true);
                        end.text = "Good job! Created by Azalee N. & Zach P.";
                    }
                } 


        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
                else
                {
                    NPC npcCharacter = hit.collider.GetComponent<NPC>();

                        if (npcCharacter != null)
                        {
                            npcCharacter.DisplayDialog();
                        }
                }
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
             ParticleSystem projectileObject = Instantiate(damageEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
              
            animator.SetTrigger("Hit");
            PlaySound(hitSound);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        if (amount > 0)
        {
        ParticleSystem projectileObject2 = Instantiate(healEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity); }
    }
    public void ChangeScore(int scoreAmount)
    {
        score += scoreAmount;

        scoreText.text = "Fixed Robots: " + score.ToString();

    }
    public void ChangeCScore(int scoreAmount)
    {
        coins += scoreAmount;

        scoreCoin.text = "Coins: " + coins.ToString();

    }
 
    //particle effect trigger (Notwroking???? possibly need to connect it to the proper animation but that still didint work, had other people double check my code aswell and still diding work)
    void OnTriggerEnter2D()
    {
        damageEffect.GetComponent<ParticleSystem>().enableEmission = true;
        StartCoroutine(stopDamageEffect());
        healEffect.GetComponent<ParticleSystem>().enableEmission = true;
        StartCoroutine(stopHealEffect());
    }

    IEnumerator stopDamageEffect()
    {
        yield return new WaitForSeconds(0.4f);
        damageEffect.GetComponent<ParticleSystem>().enableEmission = false;
    }
    IEnumerator stopHealEffect()
    {
        yield return new WaitForSeconds(0.4f);
        healEffect.GetComponent<ParticleSystem>().enableEmission = false;
    }
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        PlaySound(throwSound);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}