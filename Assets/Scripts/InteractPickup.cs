using UnityEngine;

public class InteractPickup : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;
    Collider2D myCollider;
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position);
            GetComponent<Animator>().SetTrigger("isPicked");
            myCollider.enabled = false;
            Destroy(gameObject, 0.3f);
        }
    }
}
