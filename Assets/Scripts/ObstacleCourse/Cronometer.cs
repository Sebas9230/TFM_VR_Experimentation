using UnityEngine;
using TMPro;

public class Cronometer : MonoBehaviour
{
    public TextMeshProUGUI textoTiempo;
    private float tiempo;
    private bool contando = true;

    void Update()
    {
        if (contando)
        {
            tiempo += Time.deltaTime;
            int minutos = Mathf.FloorToInt(tiempo / 60F);
            int segundos = Mathf.FloorToInt(tiempo % 60F);
            textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }

    public void DetenerCronometro()
    {
        contando = false;
    }

    public void ReiniciarCronometro()
    {
        tiempo = 0f;
        contando = true;
    }

    public float ObtenerTiempoFinal()
    {
        return tiempo;
    }
}
