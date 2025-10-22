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
    
    // Starts the countdown timer
    void Start()
    {
        StartCoroutine(Contador());
    }
    
    // Countdown coroutine that updates the timer display
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
        Debug.Log("Time finished");
        FinishGame();
    }

    // Finishes the game and saves results to SceneTracker
    void FinishGame()
    {
        if (SceneTracker.Instance != null)
        {
            SceneTracker.Instance.PreviousScene = SceneManager.GetActiveScene().name;
            // Save the shooter score
            if (ObjetivosManager.Instance != null)
            {
                SceneTracker.Instance.SetShooterResults(ObjetivosManager.Instance.puntos);
            }
        }
        SceneManager.LoadScene("GameOverScene");
    }
}
