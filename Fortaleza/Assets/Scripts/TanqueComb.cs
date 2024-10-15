using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanqueComb : MonoBehaviour
{
    public GameObject QTECanvas; // Referencia al canvas del QTE
    public BidonComb bidon; // Referencia al bidón

    private bool isQTEActive = false;

    void OnTriggerEnter(Collider other)
    {
        
        // Verifica si el jugador se acerca con el bidón en la mano y el QTE no está activo
        if (other.CompareTag("Player") && bidon.IsHeldByPlayer() && !isQTEActive)
        {
            Debug.Log("QTE Activado");
            // Activa el canvas del QTE
            QTECanvas.SetActive(true);
            isQTEActive = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        
        // Si el jugador sale del trigger, desactiva el QTE si estaba activo
        if (other.CompareTag("Player") && isQTEActive)
        {
            Debug.Log("QTE Desactivado");
            QTECanvas.SetActive(false);
            isQTEActive = false;
        }
    }

    // Método para finalizar el QTE y llenar el bidón
    public void CompleteQTE(bool success)
    {
        if (success)
        {
            bidon.FillFull();
        }
        else
        {
            bidon.FillHalf();
        }

        // Desactiva el canvas después de completar el QTE
        QTECanvas.SetActive(false);
        isQTEActive = false;
    }
}

