using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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

            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized * speed;

            // Actualizar la velocidad del Rigidbody
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

            // Dash
            if (Input.GetKeyDown(KeyCode.F) && dashCooldownTimer <= 0)
            {
                StartCoroutine(Dash(movement));
            }
        }
    }


    private System.Collections.IEnumerator Dash(Vector3 direction)
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