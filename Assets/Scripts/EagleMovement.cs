using UnityEngine;

public class EagleMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Rigidbody2D myRigidbody;
    Collider2D myCollider;
    bool isAlive = true;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isAlive)
        {
            myRigidbody.linearVelocity = new Vector2(0, moveSpeed);
        }
        else
        {
            myRigidbody.linearVelocity = new Vector2(0, 0);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Die();
            }
        }
        else
        {
            moveSpeed = -moveSpeed;
        }
    }

    public void Die()
    {
        isAlive = false;
        if (myCollider != null)
        {
            myCollider.enabled = false;
        }
        GetComponent<Animator>().SetTrigger("isDying");
        Destroy(gameObject, 0.3f);
    }
}
