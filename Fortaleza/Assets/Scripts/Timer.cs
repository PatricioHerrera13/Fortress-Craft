using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timer = 100;
    public Text textoTimer;
    public PortalDeEntregas portal;
    public GameObject PanelFase;
    public GameObject PanelDerrota;

    // Update is called once per frame
    void Update()
    {
        
        if(timer > 0)
        {
            timer -= Time.deltaTime;

            textoTimer.text = "00:" + timer.ToString("f0");
        }
        else
        {
            Time.timeScale = 0;
            if(portal.cantEntrega >= 5)
            {
                PanelFase.SetActive(true);
            }
            else
            {
                PanelDerrota.SetActive(true);
            }
        }
    }
}
