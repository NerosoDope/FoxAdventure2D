using UnityEngine;

public class FrogMovement : EnemyBase
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;

    bool isGrounded;
    float jumpTimer;
    float direction = 1f;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        isGrounded = myRigidbody.IsTouchingLayers(LayerMask.GetMask("Platform"));
        jumpTimer -= Time.deltaTime;

        if (!isAlive)
        {
            myRigidbody.linearVelocity = Vector2.zero;
            return;
        }

        myAnimator.SetBool("isJumping", !isGrounded);

        if (isGrounded && jumpTimer <= 0f)
        {
            myRigidbody.linearVelocity = new Vector2(direction * moveSpeed, jumpForce);
            direction = -direction;
            jumpTimer = jumpCooldown;
            FlipEnemyFacing();
        }
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(Mathf.Sign(direction), 1f);
    }
}
