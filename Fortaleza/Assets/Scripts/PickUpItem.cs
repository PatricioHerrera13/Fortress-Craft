using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public GameObject HandPoint;
    private GameObject pickedItem = null;

    // Nueva variable para guardar el tipo del ítem que el jugador tiene
    public string pickedItemType = "";

    // Variables para el lanzamiento
    public float throwForce = 15f; // Fuerza del lanzamiento
    public float throwAngle = 45f; // Ángulo del lanzamiento

    // Variables para la dirección de movimiento del jugador
    private Vector3 lastMoveDirection = Vector3.zero;
    private Vector3 lastPosition = Vector3.zero;

    void Update()
    {
        // Detectar el movimiento del jugador
        DetectMovement();

        // Soltar el objeto con la tecla 'R'
        if (pickedItem != null)
        {
            if (Input.GetKey("r"))
            {
                ReleaseItem();
            }

            // Lanzar el objeto con la tecla 'H'
            if (Input.GetKeyDown(KeyCode.H))
            {
                ThrowItem();
            }
        }
    }

    // Método para capturar la dirección del movimiento del jugador
    private void DetectMovement()
    {
        // Si el jugador se ha movido desde la última posición, actualizamos la dirección de movimiento
        if (transform.position != lastPosition)
        {
            lastMoveDirection = (transform.position - lastPosition).normalized;
            lastPosition = transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            if (Input.GetKey("e") && pickedItem == null)
            {
                other.GetComponent<Rigidbody>().useGravity = false;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = HandPoint.transform.position;
                other.gameObject.transform.SetParent(HandPoint.gameObject.transform);

                pickedItem = other.gameObject;
                pickedItemType = other.gameObject.GetComponent<Item>().itemType;  // Guarda el tipo del ítem
            }
        }
    }

    // Método para soltar el ítem
    private void ReleaseItem()
    {
        pickedItem.GetComponent<Rigidbody>().useGravity = true;
        pickedItem.GetComponent<Rigidbody>().isKinematic = false;
        pickedItem.transform.SetParent(null);
        pickedItemType = "";  // Resetea el tipo de ítem
        pickedItem = null;
    }

    // Método para lanzar el ítem en la dirección en la que el jugador se movía
    private void ThrowItem()
    {
        if (pickedItem != null)
        {
            // Desanclar el objeto
            pickedItem.transform.SetParent(null);

            // Hacer que el objeto sea afectado por la física
            Rigidbody itemRb = pickedItem.GetComponent<Rigidbody>();
            itemRb.useGravity = true;
            itemRb.isKinematic = false;

            // Verificar si el jugador se ha movido
            Vector3 throwDirection = lastMoveDirection;

            if (throwDirection == Vector3.zero)
            {
                // Si el jugador no se movía, lanzar hacia adelante
                throwDirection = transform.forward;
            }

            // Agregar un componente vertical para el lanzamiento parabólico
            throwDirection = (throwDirection + Vector3.up * Mathf.Tan(throwAngle * Mathf.Deg2Rad)).normalized;

            // Aplicar fuerza al objeto para lanzarlo
            itemRb.AddForce(throwDirection * throwForce, ForceMode.VelocityChange);

            // Limpiar la referencia al ítem
            pickedItem = null;
            pickedItemType = "";
        }
    }

    // Método para obtener el tipo de ítem que el jugador tiene en manos
    public string GetPickedItemType()
    {
        return pickedItemType;
    }
}
