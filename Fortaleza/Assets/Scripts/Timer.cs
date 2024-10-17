using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timer = 90;
    public Text textoTimer;
    public PortalDeEntregas portal;
    public GameObject PanelFase;
    public GameObject PanelVic;
    public GameObject PanelDerrota;
    public float segundos = 2;
    public float fase1 = 0;

    // Update is called once per frame
    void Update()
    {
        
        if(timer > 0)
        {
            timer -= Time.deltaTime;

            textoTimer.text = "00:" + timer.ToString("f0");

            if(portal.cantEntrega >= 2)
            {
                //Debug.Log("Calculando...");
                if(fase1 == 0)
                {
                    StartCoroutine(ActivarObjetoPorTiempo());
                }   
            }

            if(portal.cantEntrega >= 4)
            {
                
                StartCoroutine(ActivarObjetoPorTiempo());
            }
        }
        else
        {
            Time.timeScale = 0;
            if(portal.cantEntrega < 5)
            {
                PanelDerrota.SetActive(true);
            }
        }

    }

    
    private IEnumerator ActivarObjetoPorTiempo()
    {
        if(fase1 == 0)
        {
            fase1 = 1;
            PanelFase.SetActive(true);
            yield return new WaitForSeconds(segundos); // Espera por el tiempo especificado
            PanelFase.SetActive(false); // Desactiva el objeto
        }
        else
        {
            PanelVic.SetActive(true);
            yield return new WaitForSeconds(segundos); // Espera por el tiempo especificado
            Time.timeScale = 0;
        }
        
    }
}
