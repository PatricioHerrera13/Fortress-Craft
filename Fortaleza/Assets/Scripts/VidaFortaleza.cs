using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaFortaleza : MonoBehaviour
{
    public Canvas VidasFort1; // Referencia al canvas de Fort1
    public RectTransform fort1Image; // RectTransform de la imagen dentro del panel Fort1

    void Start()
    {
        // Asegúrate de obtener la referencia a la imagen dentro del panel Fort1
        if (VidasFort1 != null)
        {
            fort1Image = VidasFort1.transform.Find("Fort1/Image")?.GetComponent<RectTransform>();
            if (fort1Image == null)
            {
                Debug.LogError("No se encontró la imagen dentro del panel Fort1.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Proyectil"))
        {
            Debug.Log("Nashe en 1");
            // Reducir el tamaño de la imagen dentro del panel Fort1 en un 20%
            if (fort1Image != null)
            {
                // Calcular el nuevo tamaño
                Vector3 newScale = fort1Image.localScale * 0.8f; // Reducir en un 20%
                fort1Image.localScale = newScale;
            }
        }
    }
}
