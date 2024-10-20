using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaFortaleza2 : MonoBehaviour
{
    public Canvas VidasFort2; // Referencia al canvas de Fort1
    public RectTransform fort2Image; // RectTransform de la imagen dentro del panel Fort1

    void Start()
    {
        // Asegúrate de obtener la referencia a la imagen dentro del panel Fort1
        if (VidasFort2 != null)
        {
            fort2Image = VidasFort2.transform.Find("Fort2/Image")?.GetComponent<RectTransform>();
            if (fort2Image == null)
            {
                Debug.LogError("No se encontró la imagen dentro del panel Fort2.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Proyectil"))
        {
            Debug.Log("Nashe en 1");
            // Reducir el tamaño de la imagen dentro del panel Fort1 en un 20%
            if (fort2Image != null)
            {
                // Calcular el nuevo tamaño
                Vector3 newScale = fort2Image.localScale * 0.8f; // Reducir en un 20%
                fort2Image.localScale = newScale;
            }
        }
    }
}
