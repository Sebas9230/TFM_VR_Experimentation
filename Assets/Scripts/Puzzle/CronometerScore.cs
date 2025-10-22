using UnityEngine;
using TMPro;

public class CronometerScore : MonoBehaviour
{
    public TextMeshProUGUI textoTiempo;
    public TextMeshProUGUI textoSombreros; // NEW: text for correct hats

    private float tiempo;
    private bool contando = true;

    public static CronometerScore Instance; // Singleton for access from other scripts

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

    // Update hat count display
    public void ActualizarSombreros(int cantidad)
    {
        if (textoSombreros != null)
        {
            textoSombreros.text = $"{cantidad}/5";
        }
    }

    // Stop the timer
    public void DetenerCronometro()
    {
        contando = false;
    }

    // Restart the timer
    public void ReiniciarCronometro()
    {
        tiempo = 0f;
        contando = true;
    }

    // Get the final time
    public float ObtenerTiempoFinal()
    {
        return tiempo;
    }
}
