using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private AudioClip _coinPickupSFX;
    [SerializeField] private int pointsForCoinPickup = 100;

    private bool _wasCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_wasCollected)
        {
            _wasCollected = true;

            FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);

            AudioSource.PlayClipAtPoint(_coinPickupSFX, Camera.main.transform.position);

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
