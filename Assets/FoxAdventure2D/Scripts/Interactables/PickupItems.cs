using System.Collections;
using UnityEngine;

public class PickupItems : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;
    int pointsForCherryPickup = 100;
    int pointsForGemPickup = 300;
    int points = 0;
    Collider2D myCollider;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            if (CompareTag("Cherry")) points = pointsForCherryPickup;
            else if (CompareTag("Gem")) points = pointsForGemPickup;
            if (points > 0) FindAnyObjectByType<GameSession>().AddToScore(points);
            if (pickupSFX != null)
            {
                AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position);
            }
            GetComponent<Animator>().SetTrigger("isPicked");
            myCollider.enabled = false;
            Destroy(gameObject, 0.3f);
        }
    }
}
