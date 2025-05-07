using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandController : MonoBehaviour
{
    private LogicPuzzle puzzleLogic;
    private GameObject placedHat;
    
    void Start()
    {
        puzzleLogic = FindObjectOfType<LogicPuzzle>();
    }
    
    public void PlaceHat(GameObject hat)
    {
        if (placedHat != null)
        {
            // Ya hay un sombrero colocado
            return;
        }
        
        placedHat = hat;
        
        // Notificar al LogicPuzzle sobre la colocación
        puzzleLogic.CheckHatPlacement(this.transform, hat);
    }
    
    public void RemoveHat()
    {
        placedHat = null;
    }
    
    // Este método puede ser llamado desde el sistema de interacción XR
    public void OnHatPlaced(GameObject hat)
    {
        PlaceHat(hat);
    }
} 