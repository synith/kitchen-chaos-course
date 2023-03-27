using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
    KitchenObject KitchenObject { get; set; }
    public void ClearKitchenObject();
    public bool HasKitchenObject();
    public Transform GetKitchenObjectFollowTransform();
}
