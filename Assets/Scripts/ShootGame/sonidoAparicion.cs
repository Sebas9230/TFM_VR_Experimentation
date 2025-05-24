using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class sonidoAparicion : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(PlayEntrance());
    }
    IEnumerator PlayEntrance() {
        GetComponent<AudioSource>().Play();
        Debug.Log("Sonido de aparicion");
        yield return null;
    }
}
