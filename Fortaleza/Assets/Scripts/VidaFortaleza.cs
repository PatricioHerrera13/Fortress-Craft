using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaFortaleza : MonoBehaviour
{
    public VidaFortCanvas canvas;
    

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Proyectil"))
        {

        }

    }
}
