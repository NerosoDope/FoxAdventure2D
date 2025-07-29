using UnityEngine;

public class EagleMovement : EnemyBase
{
    [SerializeField] float moveSpeed;
    [SerializeField] float changeOfDirTime;

    protected override void Start()
    {
        base.Start();
        InvokeRepeating("FlipDirection", changeOfDirTime, changeOfDirTime);
    }

    void Update()
    {
        if (isAlive)
        {
            myRigidbody.linearVelocity = new Vector2(0, moveSpeed);
        }
        else
        {
            myRigidbody.linearVelocity = Vector2.zero;
        }
    }

    void FlipDirection()
    {
        if (isAlive)
        {
            moveSpeed = -moveSpeed;
        }
    }
}
