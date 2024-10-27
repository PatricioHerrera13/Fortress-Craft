using UnityEngine;
using UnityEngine.UI;

public class Wallet
{
    private Text textoMoney; // Hacerlo privado y establecerlo a través de un método
    private float money;

    public void AddMoney(float amount)
    {
        money += amount;
        if (textoMoney != null)
        {
            textoMoney.text = money.ToString("F2"); // Mostrar con 2 decimales, si lo prefieres
        }
    }

    public float GetMoney()
    {
        return money;
    }

    // Método para asignar el Text desde otro script
    public void SetTextComponent(Text newText)
    {
        textoMoney = newText;
        UpdateUI(); // Actualizar el texto en la UI cuando se asigne el componente
    }

    // Método para actualizar el texto cuando se inicializa o cambia la referencia
    private void UpdateUI()
    {
        if (textoMoney != null)
        {
            textoMoney.text = money.ToString("F2");
        }
    }
}
