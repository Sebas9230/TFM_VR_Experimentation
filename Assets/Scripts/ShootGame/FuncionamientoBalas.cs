using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncionamientoBalas : MonoBehaviour
{
    public float vida;
    public float nacimiento;
 
    void OnEnable() {
        nacimiento = Time.time; // Save the bullet birth time
    }
    void Update() {
        if(Time.time > nacimiento + vida) { // If current time is greater than birth time plus lifetime
            gameObject.SetActive(false); // Deactivate the bullet
        }
    }
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Disparable")) {
            ObjetivosManager.Instance.fuera = true;
            ObjetivosManager.Instance.Despawn();
            Debug.Log("Target destroyed");
            ObjetivosManager.Instance.fuera = false;
            gameObject.SetActive(false); 
            Debug.Log("Bullet destroyed");
            ObjetivosManager.Instance.puntos ++;
            Debug.Log("Points: " + ObjetivosManager.Instance.puntos);
        }
    }
}
