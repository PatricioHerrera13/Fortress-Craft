using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PozoDeAgua : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra al trigger es el cubo de agua
        CuboDeAgua cubo = other.GetComponent<CuboDeAgua>();

        if (cubo != null)
        {
            // Llamamos al método que recarga el cubo
            cubo.RecargarCubo();
        }
    }
}
