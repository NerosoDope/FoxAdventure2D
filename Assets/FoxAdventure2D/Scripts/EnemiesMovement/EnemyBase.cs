using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected bool isAlive = true;
    protected Rigidbody2D myRigidbody;
    protected Collider2D myCollider;
    protected Animator myAnimator;

    protected virtual void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        myAnimator = GetComponent<Animator>();
    }

    public virtual void Die()
    {
        isAlive = false;
        if (myCollider != null)
        {
            foreach (var collider in GetComponents<Collider2D>())
            {
                collider.enabled = false;
            }
        }

        if (myAnimator != null)
        {
            myAnimator.SetTrigger("isDying");
        }

        Destroy(gameObject, 0.3f);
    }
}