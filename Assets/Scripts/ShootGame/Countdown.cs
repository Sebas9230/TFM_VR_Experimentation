using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class Countdown : MonoBehaviour
{
    public float tiempo = 60f;
    public TMP_Text countDownText;
    void Start()
    {
        StartCoroutine(Contador());
    }
    IEnumerator Contador()
    {
        while (tiempo > 0)
        {
            yield return new WaitForSeconds(1);
            tiempo--;
            // Debug.Log(tiempo);
            // GameObject.FindWithTag("Tiempo").GetComponent<Text>().text = "Tiempo Restante   " + tiempo;
            countDownText.text = "Time Left " + tiempo;
        }
        Debug.Log("Fin del tiempo");
        FinishGame();
    }

    void FinishGame()
    {
        if (SceneTracker.Instance != null)
        {
            SceneTracker.Instance.PreviousScene = SceneManager.GetActiveScene().name;
            // Guardar el score del shooter
            if (ObjetivosManager.Instance != null)
            {
                SceneTracker.Instance.SetShooterResults(ObjetivosManager.Instance.puntos);
            }
        }
        SceneManager.LoadScene("GameOverScene");
    }
}
