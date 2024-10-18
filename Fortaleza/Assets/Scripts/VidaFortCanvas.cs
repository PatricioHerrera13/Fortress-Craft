using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaFortCanvas : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public Canvas VidasFort;

    private RectTransform fort1Rect;
    private RectTransform fort2Rect;

    // Start is called before the first frame update
    void Start()
    {
        // Obtener los RectTransform de los paneles Fort1 y Fort2
        fort1Rect = VidasFort.transform.Find("Fort1").GetComponent<RectTransform>();
        fort2Rect = VidasFort.transform.Find("Fort2").GetComponent<RectTransform>();

        // Posicionar los paneles en las coordenadas indicadas
        PositionPanelInCamera(fort1Rect, fort2Rect);
    }

    // Función para posicionar los paneles en las posiciones dadas
    void PositionPanelInCamera(RectTransform fort1Rect, RectTransform fort2Rect)
    {
        // Posicionar Fort1 en las coordenadas (-200, -0.4, -12)
        fort1Rect.position = new Vector3(-200, -0.4f, -12);

        // Posicionar Fort2 en las coordenadas (200, -0.4, -19)
        fort2Rect.position = new Vector3(200, -0.4f, -19);
    }
}
