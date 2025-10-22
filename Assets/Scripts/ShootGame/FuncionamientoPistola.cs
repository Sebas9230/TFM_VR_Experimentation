using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class FuncionamientoPistola : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public InputActionProperty shootAction;
    public float bulletSpeed = 10;
    public bool disparo = false;
    public bool disparado = true;
    public TMP_Text scoreText;
    void Update()
    {
        Puntaje();
        float shoot = shootAction.action.ReadValue<float>();
        if(shoot == 1){
            disparo = true;
        } else {
            disparo = false;
            disparado = true;
        }
        if(disparo && disparado){
            Shoot();
            disparado = false;
        }
    }
    // Fire a bullet from the gun
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
        // GameObject.FindObjectOfType<UnityEngine.UI.Text>().text = "Score  " + ObjetivosManager.Instance.puntos;
        // GameObject.FindWithTag("Score").GetComponent<Text>().text = "Score " + ObjetivosManager.Instance.puntos;
        scoreText.text = "Score " + ObjetivosManager.Instance.puntos;
    }
}
