using UnityEngine;

public class OpossumMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Rigidbody2D myRigidbody;
    Collider2D myCollider;
    bool isAlive = true;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (isAlive)
        {
            myRigidbody.linearVelocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            myRigidbody.linearVelocity = new Vector2(0, 0);
        }
    }

    // Chạm đất đổi hướng (trái/phải)
    void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    // Quay mặt khi đổi hướng di chuyển
    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-Mathf.Sign(moveSpeed), 1);
    }

    public void Die()
    {
        isAlive = false;
        if (myCollider != null)
        {
            myCollider.enabled = false;
        }
        GetComponent<Animator>().SetTrigger("isDying");
        Destroy(gameObject, 0.3f); // Thời gian chờ để chạy hoạt ảnh > xóa object
    }
}
