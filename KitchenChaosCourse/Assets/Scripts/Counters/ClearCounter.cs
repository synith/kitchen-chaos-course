using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // no kitchen object
            if (player.HasKitchenObject())
            {
                // player is carrying something
                player.KitchenObject.KitchenObjectParent = this;
            }
            else
            {
                // player is not carrying anything
            }
        }
        else
        {
            // kitchen object
            if (player.HasKitchenObject())
            {
                // player is carrying something
            }
            else
            {
                // player is not carrying anything
                KitchenObject.KitchenObjectParent = player;
            }
        }
    }
}
