using UnityEngine;

public class InteractEnemies : MonoBehaviour
{
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    Rigidbody2D myRigidbody;
    [SerializeField] float bounceForce;
    [SerializeField] AudioClip killSFX;

    void Start()
    {
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        EagleMovement eagle = collision.GetComponent<EagleMovement>();
        OpossumMovement opossum = collision.GetComponent<OpossumMovement>();
        FrogMovement frog = collision.GetComponent<FrogMovement>();

        if (collision.CompareTag("Enemy"))
        {
            if (myFeetCollider.IsTouching(collision)) // Nếu chân chạm kẻ thù > kẻ thù bị tiêu diệt
            {
                if (eagle != null)
                {
                    eagle.Die();
                    AudioSource.PlayClipAtPoint(killSFX, Camera.main.transform.position);
                    myRigidbody.linearVelocity = new Vector2(myRigidbody.linearVelocity.x, bounceForce);
                }
                if (opossum != null)
                {
                    opossum.Die();
                    AudioSource.PlayClipAtPoint(killSFX, Camera.main.transform.position);
                    myRigidbody.linearVelocity = new Vector2(myRigidbody.linearVelocity.x, bounceForce);
                }
                if (frog != null)
                {
                    frog.Die();
                    AudioSource.PlayClipAtPoint(killSFX, Camera.main.transform.position);
                    myRigidbody.linearVelocity = new Vector2(myRigidbody.linearVelocity.x, bounceForce);
                }
            }
            else if (myBodyCollider.IsTouching(collision)) // Nếu người chạm kẻ thù > người chơi bị tiêu diệt
            {
                PlayerMovement player = GetComponent<PlayerMovement>();
                if (player != null)
                {
                    player.Die();
                }
            }
        }
    }
}
