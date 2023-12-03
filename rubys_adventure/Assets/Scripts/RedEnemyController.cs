using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEnemyController : MonoBehaviour
{
    public float speed;
    public ParticleSystem smokeEffect;
     public AudioClip FixedRobo;

    Rigidbody2D rigidbody2D;
    bool broken = true;

    private RubyController rubyController;

    Animator animator;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    
        rubyController = FindObjectOfType<RubyController>();
    }

    void FixedUpdate()
    {
        // Remember: ! inverses the test, so if broken is true, !broken will be false, and the return won’t be executed.
        if (!broken || rubyController == null)
        {
            return;
        }

        Vector2 directionToRuby = (rubyController.transform.position - transform.position).normalized;

        rigidbody2D.velocity = directionToRuby * speed;

        // Optional: Update animator parameters based on the direction
        animator.SetFloat("Move X", directionToRuby.x);
        animator.SetFloat("Move Y", directionToRuby.y);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    // Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;
        //optional if you added the fixed animation
        animator.SetTrigger("Fixed");
        PlaySound(FixedRobo);
        smokeEffect.Stop();
        
        if (rubyController != null)
        {
            rubyController.ChangeScore(1);
        }
    }
     public void PlaySound(AudioClip clip)
    {
        // Optional: Stop the enemy when fixed
        rigidbody2D.velocity = Vector2.zero;
    }
}