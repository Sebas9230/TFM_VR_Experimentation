using UnityEngine;
using TMPro;

public class CronometerScore : MonoBehaviour
{
    public TextMeshProUGUI textoTiempo;
    public TextMeshProUGUI textoSombreros; // NEW: text for correct hats

    private float tiempo;
    private bool contando = true;

    public static CronometerScore Instance; // Singleton for access from other scripts

    // Initializes singleton instance
    private void Awake()
    {
        Instance = this;
    }

    // Updates timer display while counting
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

    // Updates hat count display
    public void ActualizarSombreros(int cantidad)
    {
        textoSombreros.text = $"{cantidad}/5";
    }

    // Stops timer
    public void DetenerCronometro()
    {
        contando = false;
    }

    // Restarts timer
    public void ReiniciarCronometro()
    {
        tiempo = 0f;
        contando = true;
    }

    // Returns final timer value
    public float ObtenerTiempoFinal()
    {
        return tiempo;
    }
}
