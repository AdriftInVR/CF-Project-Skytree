using UnityEngine;

public class SkylerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator; // Referencia al componente Animator
    public Rigidbody2D rb;    // Referencia al componente Rigidbody2D

    private Vector2 movement; // Almacena el input de movimiento

    void Update()
    {
        // Obtener input del jugador (teclas de flecha o WASD)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Actualizar animaciones según la dirección del movimiento
        if (movement != Vector2.zero)
        {
            // Definir qué dirección está moviéndose el jugador para cambiar la animación
            if (movement.x > 0)
            {
                animator.Play("Walk_Right");
            }
            else if (movement.x < 0)
            {
                animator.Play("Walk_Left");
            }
            else if (movement.y > 0)
            {
                animator.Play("Walk_Up");
            }
            else if (movement.y < 0)
            {
                animator.Play("Walk_Down");
            }
        }
        else
        {
            // Si no hay movimiento, cambiar a animación de idle
            animator.Play("Idle");
        }

        // Pasar valores al Animator para hacer blending si es necesario
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude); // Velocidad del personaje (para activar/deactivar animaciones de caminar)
    }

    void FixedUpdate()
    {
        // Aplicar el movimiento
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
