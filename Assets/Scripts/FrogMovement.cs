using UnityEngine;

public class FrogMovement : MonoBehaviour
{
    bool isGrounded;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;

    Rigidbody2D myRigidbody;
    Animator myAmimation;
    Collider2D myCollider;
    float jumpTimer;
    float direction = 1f;
    bool isAlive = true;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAmimation = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        isGrounded = myRigidbody.IsTouchingLayers(LayerMask.GetMask("Platform"));
        jumpTimer -= Time.deltaTime;
        if (isAlive)
        {
            if (!isGrounded)
            {
                myAmimation.SetBool("isJumping", true);
            }
            else
            {
                myAmimation.SetBool("isJumping", false);
            }

            if (isGrounded && jumpTimer <= 0f)
            {
                myRigidbody.linearVelocity = new Vector2(direction * moveSpeed, jumpForce);
                direction = -direction; // Đổi hướng cho lần nhảy tiếp theo
                jumpTimer = jumpCooldown; 
            }
            FlipEnemyFacing();
        }
        else
        {
            myRigidbody.linearVelocity = new Vector2(0, 0);
        }
    }

    // Quay mặt khi đổi hướng di chuyển
    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(Mathf.Sign(direction), 1f); // Mỗi lần nhảy thì đổi hướng
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