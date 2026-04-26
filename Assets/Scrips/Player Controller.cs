using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private CapsuleCollider2D col;

    [Header("Ground Check")]
    [SerializeField] private Transform feetPos;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDistance = 0.4f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;

    [Header("Crouch")]
    [SerializeField] private Vector2 standSize = new Vector2(1f, 1.8f);
    [SerializeField] private Vector2 crouchSize = new Vector2(1f, 1f);
    [SerializeField] private Vector2 standOffset = new Vector2(0f, 0f);
    [SerializeField] private Vector2 crouchOffset = new Vector2(0f, -0.4f);

    private bool isGrounded;
    private bool isCrouching;

    void Start()
    {
        // reset state
        isCrouching = false;
        anim.SetBool("isCrouching", false);

        col.size = standSize;
        col.offset = standOffset;

        // กันจมพื้นตอนเริ่ม
        transform.position += Vector3.up * 0.2f;
    }

    void Update()
    {
        CheckGround();
        HandleJump();
        HandleCrouch();
        UpdateAnimation();
    }

    // ================= GROUND =================
    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);
    }

    // ================= JUMP =================
    void HandleJump()
    {
        if (isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // resetแรงตกก่อน
            rb.velocity = new Vector2(rb.velocity.x, 0f);

            // กระโดดแบบเสถียร
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // ================= CROUCH =================
    void HandleCrouch()
    {
        if (isGrounded && Keyboard.current.leftCtrlKey.wasPressedThisFrame)
        {
            isCrouching = true;
            anim.SetBool("isCrouching", true);

            col.size = crouchSize;
            col.offset = crouchOffset;
        }

        if (Keyboard.current.leftCtrlKey.wasReleasedThisFrame)
        {
            isCrouching = false;
            anim.SetBool("isCrouching", false);

            col.size = standSize;
            col.offset = standOffset;
        }
    }

    // ================= ANIMATION =================
    void UpdateAnimation()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    // ================= DEBUG =================
    private void OnDrawGizmosSelected()
    {
        if (feetPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(feetPos.position, groundDistance);
        }
    }
}