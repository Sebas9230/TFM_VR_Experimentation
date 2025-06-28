using UnityEngine;
using TMPro;

public class CronometerScore : MonoBehaviour
{
    public TextMeshProUGUI textoTiempo;
    public TextMeshProUGUI textoSombreros; // NUEVO: texto para sombreros correctos

    private float tiempo;
    private bool contando = true;

    public static CronometerScore Instance; // Singleton para acceso desde otros scripts

    private void Awake()
    {
        Instance = this;
    }

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

    public void ActualizarSombreros(int cantidad)
    {
        if (textoSombreros != null)
        {
            textoSombreros.text = $"{cantidad}/5";
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
