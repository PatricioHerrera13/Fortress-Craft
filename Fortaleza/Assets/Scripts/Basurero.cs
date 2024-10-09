using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basurero : MonoBehaviour
{
    // Este método se llama cuando otro objeto entra en el trigger del basurero
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entra en el trigger tiene la etiqueta "Item"
        if (other.gameObject.CompareTag("Item"))
        {
            // Eliminar el objeto
            Destroy(other.gameObject);

            // Opcional: Mostrar un mensaje en consola para confirmar que el objeto fue eliminado
            Debug.Log("Objeto eliminado: " + other.gameObject.name);
        }
    }
}
