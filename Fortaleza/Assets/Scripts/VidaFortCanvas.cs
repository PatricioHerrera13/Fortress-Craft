using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaFortCanvas : MonoBehaviour
{
    public CameraVS cameraScript; // Referencia al script que controla las cámaras
    public Canvas VidasFort1; // Canvas para la cámara 1
    public Canvas VidasFort2; // Canvas para la cámara 2

    private RectTransform fort1Rect; // RectTransform de Fort1
    private RectTransform fort2Rect; // RectTransform de Fort2

    // Start is called before the first frame update
    void Start()
    {
        // Verificar si la referencia al script CameraVS está asignada
        if (cameraScript == null)
        {
            Debug.LogError("CameraVS script no asignado en el inspector.");
            return;
        }

        // Obtener los RectTransform de los paneles Fort1 y Fort2
        fort1Rect = VidasFort1.transform.Find("Fort1")?.GetComponent<RectTransform>();
        fort2Rect = VidasFort2.transform.Find("Fort2")?.GetComponent<RectTransform>();

        // Verificar si los paneles fueron encontrados correctamente
        if (fort1Rect == null)
        {
            Debug.LogError("Fort1 no encontrado en el canvas VidasFort1.");
            return;
        }
        if (fort2Rect == null)
        {
            Debug.LogError("Fort2 no encontrado en el canvas VidasFort2.");
            return;
        }

        // Verificar si las cámaras están correctamente asignadas
        if (cameraScript.Cam1 == null || cameraScript.Cam2 == null)
        {
            Debug.LogError("Las cámaras no están asignadas en el script CameraVS.");
            return;
        }

        // Posicionar los paneles
        PositionPanelInCamera(fort1Rect, cameraScript.Cam1);
        PositionPanelInCamera(fort2Rect, cameraScript.Cam2);
    }

    // Función para posicionar un panel en la parte superior de la cámara dividida
    void PositionPanelInCamera(RectTransform panelRect, Camera cam)
    {
        if (panelRect == null)
        {
            Debug.LogError("panelRect es null, no se puede posicionar.");
            return;
        }

        if (cam == null)
        {
            Debug.LogError("Cámara es null, no se puede posicionar el panel.");
            return;
        }

        // Obtener el Viewport Rect de la cámara
        Rect cameraViewport = cam.rect;

        // Calcular la posición dentro del canvas basada en el viewport de la cámara
        Vector2 viewportPos = new Vector2(
            cameraViewport.x + cameraViewport.width / 2f,  // Centrar en la X del viewport
            cameraViewport.y + cameraViewport.height        // Posicionar en la parte superior
        );

        // Obtener el tamaño del canvas
        RectTransform canvasRect = cam.GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
        if (canvasRect == null)
        {
            Debug.LogError("RectTransform del canvas no encontrado.");
            return;
        }

        // Usar la cámara actual (cam) para la conversión de posiciones
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            cam.ViewportToScreenPoint(viewportPos),  // Usar la cámara correcta para convertir
            cam,                                     // Cámara activa
            out localPos
        );

        // Ajustar la posición del panel dentro del canvas
        panelRect.anchoredPosition = localPos;

        // Ajustar la altura del panel para que esté pegado al borde superior
        panelRect.anchoredPosition += new Vector2(0, -10); // Cambia este valor para ajustarlo más cerca del borde superior

        Debug.Log($"Posición del panel {panelRect.name} ajustada en la cámara {cam.name}: {panelRect.anchoredPosition}");
    }
}
