using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] Transform counterTopPoint;

    public KitchenObject KitchenObject { get; set; }
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
