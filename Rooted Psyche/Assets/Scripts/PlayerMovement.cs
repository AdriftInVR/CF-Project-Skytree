using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Animator animator;

    private Vector3 movement;

    public SpriteRenderer spriteRenderer;

    void Update()
    {
        // Get input for 3D movement (X for horizontal, Z for forward/backward)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        // Normalize movement vector to prevent diagonal speed boost
        movement.Normalize();

        // Set Animator parameters for 4-directional movement
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.z);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (!spriteRenderer.flipX && movement.x < 0)
        {
            spriteRenderer.flipX = true;
        } else if (spriteRenderer.flipX && movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    void FixedUpdate()
    {
        // Move the character in the 3D world
        transform.Translate(movement * moveSpeed * Time.fixedDeltaTime, Space.World);
    }
}
