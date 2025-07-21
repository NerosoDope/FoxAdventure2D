using UnityEngine;

public class InteractEnemies : MonoBehaviour
{
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    Rigidbody2D myRigidbody;
    [SerializeField] float bounceForce;

    void Start()
    {
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (myFeetCollider.IsTouching(collision))
            {
                EagleMovement eagle = collision.GetComponent<EagleMovement>();
                OpossumMovement opossum = collision.GetComponent<OpossumMovement>();
                FrogMovement frog = collision.GetComponent<FrogMovement>();

                if (eagle != null)
                {
                    eagle.Die();
                    myRigidbody.linearVelocity = new Vector2(myRigidbody.linearVelocity.x, bounceForce);
                }
                if (opossum != null)
                {
                    opossum.Die();
                    myRigidbody.linearVelocity = new Vector2(myRigidbody.linearVelocity.x, bounceForce);
                }
                if (frog != null)
                {
                    frog.Die();
                    myRigidbody.linearVelocity = new Vector2(myRigidbody.linearVelocity.x, bounceForce);
                }
            }
            else if (myBodyCollider.IsTouching(collision))
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
