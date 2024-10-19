using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaFortaleza : MonoBehaviour
{
    public VidaFortCanvas canvas1; // Referencia al script que maneja los paneles
    public RectTransform fort1Panel; // RectTransform del panel Fort1

    void Start()
    {
        // Asegúrate de obtener la referencia al panel Fort1 desde el canvas
        if (canvas1 != null)
        {
            fort1Panel = canvas1.VidasFort1.GetComponent<RectTransform>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Proyectil"))
        {
            Debug.Log("Nashe en 1");
            // Reducir el tamaño del panel Fort1 en un 20%
            if (fort1Panel != null)
            {
                // Calcular el nuevo tamaño
                Vector3 newScale = fort1Panel.localScale * 0.8f; // Reducir en un 20%
                fort1Panel.localScale = newScale;
            }
        }
    }
}
