using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjects : ScriptableObject
{
    [SerializeField]
    private KitchenObjectsSO KitchenObjectsSO;

    public KitchenObjectsSO GetKitchenObjectSO()
    {
        return KitchenObjectsSO;
    }
}
