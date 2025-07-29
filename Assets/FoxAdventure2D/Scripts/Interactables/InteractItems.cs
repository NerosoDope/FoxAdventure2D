using UnityEngine;

public class InteractItems : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;
    int pointsForCherryPickup = 100;
    int pointsForGemPickup = 300;
    Collider2D myCollider;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (CompareTag("Cherry"))
            {
                FindObjectOfType<GameSession>().AddToScore(pointsForCherryPickup);
            }
            else if (CompareTag("Gem"))
            {
                FindObjectOfType<GameSession>().AddToScore(pointsForGemPickup);
            }

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
