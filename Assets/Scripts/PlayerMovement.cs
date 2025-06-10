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
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    bool isCrouchPressed;

    void Start()
    {
        Application.targetFrameRate = 144;
        myRigidbody = GetComponent<Rigidbody2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Run();
        Crouch();
        FlipSprite();
        UpdateAnimationState();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

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

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), 1f);
        }
    }

    void OnJump(InputValue value)
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            return;
        }
        if (isCrouchPressed)
        {
            return;
        }
        if (value.isPressed)
        {
            myRigidbody.linearVelocity = new Vector2(0, jumpSpeed);
        }
    }

    void UpdateAnimationState()
    {
        bool isPlatformed = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));

        myAnimator.SetBool("isJumping", myRigidbody.linearVelocity.y > 0.1f && !isPlatformed);
        myAnimator.SetBool("isFalling", myRigidbody.linearVelocity.y < -0.1f && !isPlatformed);
    }

    void OnCrouch(InputValue value)
    {
        isCrouchPressed = value.isPressed;
    }

    void Crouch()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            myAnimator.SetBool("isCrouching", isCrouchPressed);
        }
    }
}