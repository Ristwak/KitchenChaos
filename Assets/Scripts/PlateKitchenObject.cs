using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectsSO kitchenObjectsSO;
    }
    [SerializeField] private List<KitchenObjectsSO> validKitchenObjectSOList;
    private List<KitchenObjectsSO> kitchenObjectsSOList;

    private void Awake()
    {
        kitchenObjectsSOList = new List<KitchenObjectsSO>();
    }
    public bool TryAddIngredient(KitchenObjectsSO kitchenObjectsSO)
    {
        if(!validKitchenObjectSOList.Contains(kitchenObjectsSO))
        {
            // Not a valid type od KitchenObject
            return false;
        }
        if (kitchenObjectsSOList.Contains(kitchenObjectsSO))
        {
            // Already has that kitchen object on plate
            return false;
        }
        else
        {
            kitchenObjectsSOList.Add(kitchenObjectsSO);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs{
                kitchenObjectsSO = kitchenObjectsSO
            });
            return true;
        }
    }

    public List<KitchenObjectsSO> GetkitchenObjectsSOList()
    {
        return kitchenObjectsSOList;
    }
}
