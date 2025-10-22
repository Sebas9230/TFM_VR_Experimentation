using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Windows.Speech; //para usar KeywordRecognizer
using System; //para usar Action
using System.Linq; //para usar ToArray

public class ReconocimientoVoz : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10;
    public TMP_Text scoreText;
    //for voice recognition
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, Action> wordsToActions;

    // Initializes voice recognition system
    void Start()
    {
        wordsToActions = new Dictionary<string, Action>(); //create dictionary of words to actions
        wordsToActions.Add("shoot", Shoot); //add "shoot" word to dictionary and Shoot action
        keywordRecognizer = new KeywordRecognizer(wordsToActions.Keys.ToArray()); //create KeywordRecognizer with dictionary words converted to array
        keywordRecognizer.OnPhraseRecognized += WordRecognizer; //assign WordRecognizer method to KeywordRecognizer OnPhraseRecognized event
        keywordRecognizer.Start(); //start KeywordRecognizer
    }

    // Updates score display
    void Update()
    {
        Puntaje();
        
    }
     
     // Handles recognized voice commands
     private void WordRecognizer(PhraseRecognizedEventArgs word)
    {
        Debug.Log(word.text);
        wordsToActions[word.text].Invoke();
    }
    
    // Creates and fires a bullet from the spawn point
    public void Shoot()
    {
        Debug.Log("Shoot");
        var bullet = PoolManager.Instance.GetBullet();
        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
        
    }

    // Function to display score on screen
    void Puntaje() {
        // GameObject.FindObjectOfType<UnityEngine.UI.Text>().text = "Puntuación  " + ObjetivosManager.Instance.puntos;
        // GameObject.FindWithTag("Score").GetComponent<Text>().text = "Puntuación " + ObjetivosManager.Instance.puntos;
        scoreText.text = "Score " + ObjetivosManager.Instance.puntos;
    }
}
