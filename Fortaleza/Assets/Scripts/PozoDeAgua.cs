using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PozoDeAgua : MonoBehaviour
{
    private CuboDeAgua cubo = null; // Definir la variable cubo fuera de los métodos para que sea accesible

    void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra al trigger es el cubo de agua
        cubo = other.GetComponent<CuboDeAgua>();
    }

    void OnTriggerExit(Collider other)
    {
        // Si el objeto sale del trigger, y es el cubo de agua, lo eliminamos
        if (other.GetComponent<CuboDeAgua>() != null)
        {
            cubo = null; // Para evitar seguir intentando recargar un cubo que ya no está en el trigger
        }
    }

    void Update()
    {
        if (cubo != null && Input.GetKeyDown(KeyCode.T))
        {
            // Llamamos al método que recarga el cubo cuando se presiona la tecla T
            cubo.RecargarCubo();
        }
    }
}
