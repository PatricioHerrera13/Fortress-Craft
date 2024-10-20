using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public Canvas shopMenuCanvas; // Referencia pública al canvas de la tienda.
    private bool playerInRange; // Variable para saber si el jugador está dentro del trigger.

    void Start()
    {
        shopMenuCanvas.enabled = false; // Asegúrate de que el canvas esté deshabilitado al inicio.
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que entra tiene la etiqueta "Player".
        {
            playerInRange = true; // Jugador está dentro del rango.
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que sale tiene la etiqueta "Player".
        {
            playerInRange = false; // Jugador salió del rango.
            shopMenuCanvas.enabled = false; // Cierra el menú de la tienda si el jugador sale.
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E)) // Si el jugador está en rango y presiona "E".
        {
            shopMenuCanvas.enabled = !shopMenuCanvas.enabled; // Abre o cierra el menú de la tienda.
        }
    }
}
