using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatInteraction : MonoBehaviour
{
    private bool isPlaced = false;
    private StandController currentStand;
    
    // Este método sería llamado por el sistema de interacción XR existente
    public void OnPlace(Collider other)
    {
        if (isPlaced) return;
        
        StandController stand = other.GetComponent<StandController>();
        if (stand != null)
        {
            isPlaced = true;
            currentStand = stand;
            stand.PlaceHat(this.gameObject);
        }
    }
    
    // Este método sería llamado cuando se toma el sombrero de nuevo
    public void OnPickup()
    {
        if (isPlaced && currentStand != null)
        {
            currentStand.RemoveHat();
            isPlaced = false;
            currentStand = null;
        }
    }
} 