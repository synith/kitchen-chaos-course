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
                if (player.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {                    
                    if (plateKitchenObject.TryAddIngredient(KitchenObject.KitchenObjectSO))
                    {
                        KitchenObject.DestroySelf();
                    }
                }
                else
                {
                    // Player is not carrying plate, but something else
                    if (KitchenObject.TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(player.KitchenObject.KitchenObjectSO))
                        {
                            player.KitchenObject.DestroySelf();
                        }
                    }
                }
            }
            else
            {
                // player is not carrying anything
                KitchenObject.KitchenObjectParent = player;
            }
        }
    }
}
