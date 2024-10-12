using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuboDeAgua : MonoBehaviour
{
    // Este script debe estar en el cubo de agua

    void OnTriggerEnter(Collider other)
    {
        // Comprobar si el objeto que entra en el trigger es un fuego
        if (other.CompareTag("Fuego"))
        {
            // Destruir el objeto de fuego
            Destroy(other.gameObject);
            Debug.Log("Fuego destruido por el cubo de agua.");
        }
    }
}
