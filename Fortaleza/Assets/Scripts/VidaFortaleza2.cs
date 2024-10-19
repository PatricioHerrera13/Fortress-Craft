using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaFortaleza2 : MonoBehaviour
{
    public VidaFortCanvas canvas1;
    
    

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Proyectil"))
        {
            Debug.Log("Nashe en 2");
        }

    }
}
