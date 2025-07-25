using UnityEngine;

public class EagleMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float changeOfDirTime;
    Rigidbody2D myRigidbody;
    Collider2D myCollider;
    bool isAlive = true;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();

        InvokeRepeating("FlipDirection", changeOfDirTime, changeOfDirTime); // Enemy sẽ đổi hướng sau 1 khoảng thời gian
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

    void FlipDirection()
    {
        if (isAlive)
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
