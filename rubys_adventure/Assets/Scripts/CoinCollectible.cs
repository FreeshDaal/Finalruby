using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectible : MonoBehaviour
{
    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.ChangeCScore(1);
            PlayCollectedSound();
            Destroy(gameObject);
        }
    }

    void PlayCollectedSound()
    {
        if (collectedClip != null)
        {
            AudioSource.PlayClipAtPoint(collectedClip, transform.position);
        }
    }
}