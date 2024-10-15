using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BidonComb : MonoBehaviour
{
    private bool isHeldByPlayer = false; // Para verificar si el bidón está sostenido
    public float fillAmount = 0f; // Nivel de llenado del bidón

    // Referencia al 'HandPoint' donde se colocan los objetos que el jugador sostiene
    public Transform handPoint;

    void Update()
    {
        // Verifica si el bidón está siendo sostenido por el player (colocado en HandPoint)
        if (transform.parent == handPoint)
        {
            isHeldByPlayer = true;
            
        }
        else
        {
            isHeldByPlayer = false;
            
        }
    }

    public bool IsHeldByPlayer()
    {
        return isHeldByPlayer;
    }

    // Método para llenar completamente el bidón
    public void FillFull()
    {
        fillAmount = 1f; // Llena completamente el bidón
        Debug.Log("Bidón lleno completamente.");
    }

    // Método para llenar el bidón a la mitad
    public void FillHalf()
    {
        fillAmount = 0.5f; // Llena el bidón a la mitad
        Debug.Log("Bidón lleno a la mitad.");
    }
}
