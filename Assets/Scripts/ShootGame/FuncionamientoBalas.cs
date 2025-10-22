using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncionamientoBalas : MonoBehaviour
{
    public float vida;
    public float nacimiento;
 
    // Records bullet birth time when activated
    void OnEnable() {
        nacimiento = Time.time; // save bullet birth time
    }
    
    // Checks if bullet lifetime has expired
    void Update() {
        if(Time.time > nacimiento + vida) { // if current time is greater than birth time plus lifetime
            gameObject.SetActive(false); // deactivate bullet
        }
    }
    
    // Handles collision with shootable targets
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
