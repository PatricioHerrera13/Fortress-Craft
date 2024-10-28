using System.Collections;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public float speed = 20f;
    public float dashSpeed = 50f; // Velocidad del dash
    public float dashDuration = 0.2f; // Duración del dash
    public float dashCooldown = 1f; // Tiempo de espera entre dashes
    private Rigidbody rb;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;
    private Vector3 lastMovementDirection; // Guardar la última dirección de movimiento

    public Transform hand; // Referencia al objeto Hand
    public float handOffsetDistance = 1f; // Distancia de la mano desde el jugador

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Actualiza el temporizador de cooldown
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // Movimiento normal
        if (!isDashing)
        {
            // Movimiento utilizando las flechas del teclado
            float moveHorizontal = 0f;
            float moveVertical = 0f;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveHorizontal = -1f; // Mover hacia la izquierda
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                moveHorizontal = 1f; // Mover hacia la derecha
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveVertical = 1f; // Mover hacia adelante
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                moveVertical = -1f; // Mover hacia atrás
            }

            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;

            // Guarda la última dirección de movimiento válida
            if (movement.magnitude > 0)
            {
                lastMovementDirection = movement;
            }

            // Limita la velocidad en diagonal para evitar que el jugador se mueva más rápido
            rb.velocity = Vector3.ClampMagnitude(movement * speed, speed);

            // Mueve la mano usando la posición del jugador y la dirección de movimiento o la última dirección válida
            Vector3 handTargetPosition = transform.position + (lastMovementDirection.normalized * handOffsetDistance);

            // Si hay movimiento, actualiza la posición y rotación de la mano según la dirección actual
            if (movement != Vector3.zero)
            {
                hand.position = handTargetPosition; // Mover la mano hacia la nueva posición
                hand.rotation = Quaternion.LookRotation(-movement); // Invertir la dirección de movimiento
            }
            else if (lastMovementDirection != Vector3.zero)
            {
                // Si no hay movimiento, mantén la mano en la última dirección válida
                hand.position = handTargetPosition; // Mover la mano hacia la última dirección
                hand.rotation = Quaternion.LookRotation(-lastMovementDirection);
            }

            // Dash
            if (Input.GetKeyDown(KeyCode.P) && dashCooldownTimer <= 0)
            {
                StartCoroutine(Dash(lastMovementDirection));
            }
        }
    }

    private IEnumerator Dash(Vector3 direction)
    {
        isDashing = true;

        // Aumenta la velocidad durante el dash
        rb.velocity = direction.normalized * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        // Vuelve a la velocidad normal
        isDashing = false;

        // Reiniciar el cooldown
        dashCooldownTimer = dashCooldown;
    }
}
