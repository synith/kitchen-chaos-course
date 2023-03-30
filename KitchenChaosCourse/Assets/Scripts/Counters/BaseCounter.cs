using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlacedHere;

    [SerializeField] Transform counterTopPoint;

    KitchenObject kitchenObject;
    public KitchenObject KitchenObject
    {
        get { return kitchenObject; }
        set 
        { 
            kitchenObject = value;
            if (kitchenObject != null)
            {
                OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public void ClearKitchenObject() => KitchenObject = null;
    public bool HasKitchenObject() => KitchenObject != null;
    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;

    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact()");
    }
    public virtual void InteractAlternate(Player player)
    {
        //Debug.LogError("BaseCounter.InteractAlternate()");
    }
}
