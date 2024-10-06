using System.Collections;
using System.Collections.Generic;
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
            float moveHorizontal = 0f;
            float moveVertical = 0f;

            // Control del movimiento WASD
            if (Input.GetKey(KeyCode.A))
            {
                moveHorizontal = -1f; // Mover a la izquierda
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveHorizontal = 1f; // Mover a la derecha
            }

            if (Input.GetKey(KeyCode.W))
            {
                moveVertical = 1f; // Mover hacia adelante
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveVertical = -1f; // Mover hacia atrás
            }

            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized * speed;

            // Actualizar la velocidad del Rigidbody
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

            // Dash (verifica si la tecla de dash está configurada correctamente)
            if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0)
            {
                StartCoroutine(Dash(movement));
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
