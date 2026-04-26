using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform GFX;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private float jumpTime = 0.3f;

    [SerializeField] private float crouchHeight = 0.5f;

    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimer;

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);
        #region JUMPING
        if (isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isJumping = true;
            rb.velocity = Vector2.up * jumpForce;
        }
        if (isJumping && Keyboard.current.spaceKey.isPressed)
        {
            if (jumpTimer < jumpTime)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimer += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            isJumping = false;
            jumpTimer = 0f;
        }
        #endregion

        #region CROUCHING
        if (isGrounded && Keyboard.current.ctrlKey.wasPressedThisFrame)
        {
            GFX.localScale = new Vector3(GFX.localScale.x, crouchHeight, GFX.localScale.z);
        }

        if (Keyboard.current.ctrlKey.wasReleasedThisFrame)
        {
            GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
        }
        #endregion
    }

}
