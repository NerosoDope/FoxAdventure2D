using UnityEngine;

public class FrogMovement : MonoBehaviour
{
    bool isGrounded;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;

    Rigidbody2D myRigidbody;
    Animator myAmimation;
    float jumpTimer;
    float direction = 1f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAmimation = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = myRigidbody.IsTouchingLayers(LayerMask.GetMask("Platform"));
        jumpTimer -= Time.deltaTime;

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
            direction = -direction;
            jumpTimer = jumpCooldown;
        }

        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(Mathf.Sign(direction), 1f);
    }
}