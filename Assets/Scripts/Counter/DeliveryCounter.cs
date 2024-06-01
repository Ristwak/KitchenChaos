using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{  
    
    public static DeliveryCounter Instance { get; private set;}

    private void Awake() {
        Instance = this;
    }
    public override void Interact(PlayerScript player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TrygetPlate(out PlateKitchenObject plateKitchenObject))
            {
                // Only accepts plates
                
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
