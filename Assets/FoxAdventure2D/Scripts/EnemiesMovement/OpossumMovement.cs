using UnityEngine;

public class OpossumMovement : EnemyBase
{
    [SerializeField] float moveSpeed;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (isAlive)
        {
            myRigidbody.linearVelocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            myRigidbody.linearVelocity = Vector2.zero;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-Mathf.Sign(moveSpeed), 1);
    }
}
