using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Animator myAnimator;
    Rigidbody2D myRigidbody;
    BoxCollider2D myFeetCollider;
    CapsuleCollider2D myBodyCollider;
    Collider2D myCollider;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float climbSpeed;
    [SerializeField] float deathBounce;
    [SerializeField] AudioClip deathSFX;
    bool isCrouchPressed;
    bool isAlive = true;
    float gravityScaleAtStart;

    void Start()
    {
        Application.targetFrameRate = 180;
        myRigidbody = GetComponent<Rigidbody2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        UpdateAnimationState();
        ClimbLadder();
    }

    // Di chuyển
    // Bấm nút để di chuyển
    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    // Xử lý di chuyển
    void Run()
    {
        if (isCrouchPressed)
        {
            myRigidbody.linearVelocity = new Vector2(0, myRigidbody.linearVelocity.y);
            myAnimator.SetBool("isRunning", !isCrouchPressed);
            return;
        }

        myRigidbody.linearVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.linearVelocity.y);
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    // Quay mặt khi đổi hướng di chuyển
    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), 1f);
        }
    }

    // Nhảy
    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            return;
        }
        if (value.isPressed)
        {
            myRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
        if (isCrouchPressed)
        {
            return;
        }
    }

    // Chuyển animation từ nhảy > rơi
    void UpdateAnimationState()
    {
        bool isPlatformed = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));

        myAnimator.SetBool("isJumping", myRigidbody.linearVelocity.y > 0.1f && !isPlatformed);
        myAnimator.SetBool("isFalling", myRigidbody.linearVelocity.y < -0.1f && !isPlatformed);
    }

    // Leo thang
    void ClimbLadder()
    {
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        myRigidbody.linearVelocity = new Vector2(myRigidbody.linearVelocity.x, moveInput.y * climbSpeed);
        myRigidbody.gravityScale = 0f;

        myAnimator.SetBool("isClimbing", Mathf.Abs(moveInput.y * climbSpeed) > Mathf.Epsilon);

        // Chỉ tắt các animation khác khi thực sự trèo
        if (Mathf.Abs(moveInput.y * climbSpeed) > Mathf.Epsilon)
        {
            myAnimator.SetBool("isJumping", false);
            myAnimator.SetBool("isFalling", false);
            myAnimator.SetBool("isRunning", false);
        }
    }

    // Ngồi
    // Bấm nút để ngồi
    // void OnCrouch(InputValue value)
    // {
    //     if (!isAlive) { return; }
    //     isCrouchPressed = value.isPressed;
    // }

    // // Xử lý ngồi
    // void Crouch()
    // {
    //     if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform")))
    //     {
    //         myAnimator.SetBool("isCrouching", isCrouchPressed);
    //     }
    // }

    public void Die(Transform enemyTransform)
    {
        if (!isAlive) return;

        isAlive = false;
        myAnimator.SetTrigger("isDying");

        // Xác định hướng văng dựa trên vị trí enemy
        float knockbackDirection = transform.position.x < enemyTransform.position.x ? -1f : 1f;

        Vector2 knockback = new Vector2(knockbackDirection * moveSpeed, deathBounce);
        myRigidbody.linearVelocity = knockback;

        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);

        var gameSession = FindAnyObjectByType<GameSession>();
        gameSession?.ProcessPlayerDeath();

        // Nếu còn mạng thì hồi phục, nếu không thì giữ nguyên animation "isDying"
        if (gameSession != null && gameSession.PlayerHasLivesLeft())
        {
            StartCoroutine(RecoverAfterHit());
        }
        else
        {
            // Ngắt hết collider để nhân vật không tương tác gì nữa
            myFeetCollider.enabled = false;
            myBodyCollider.enabled = false;
        }
    }

    IEnumerator RecoverAfterHit()
    {
        yield return new WaitForSeconds(0.3f); // Chờ animation chết

        // Reset input và velocity
        moveInput = Vector2.zero;
        myRigidbody.linearVelocity = Vector2.zero;

        myAnimator.ResetTrigger("isDying");
        myAnimator.Play("Idle"); // Đảm bảo đây là tên đúng trong Animator

        isAlive = true;
    }
}