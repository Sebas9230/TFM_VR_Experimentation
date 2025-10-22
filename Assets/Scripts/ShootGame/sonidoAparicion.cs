using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class sonidoAparicion : MonoBehaviour
{
    // Plays entrance sound when object is activated
    void OnEnable()
    {
        StartCoroutine(PlayEntrance());
    }
    
    // Coroutine to play entrance sound
    IEnumerator PlayEntrance() {
        GetComponent<AudioSource>().Play();
        Debug.Log("Entrance sound");
        yield return null;
    }
}
