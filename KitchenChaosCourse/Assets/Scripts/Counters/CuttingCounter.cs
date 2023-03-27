using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] CuttingRecipeSO[] cutKitchenObjectSOArray;

    int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // no kitchen object
            if (player.HasKitchenObject())
            {
                // player is carrying something
                if (HasRecipeWithInput(player.KitchenObject.KitchenObjectSO))
                {
                    // player is carrying something that can be cut
                    player.KitchenObject.KitchenObjectParent = this;

                    cuttingProgress = 0;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(KitchenObject.KitchenObjectSO);

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs 
                    {                        
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
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
            }
            else
            {
                // player is not carrying anything
                KitchenObject.KitchenObjectParent = player;
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(KitchenObject.KitchenObjectSO))
        {
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(KitchenObject.KitchenObjectSO);

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(KitchenObject.KitchenObjectSO);
                KitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }

        }
    }

    bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetCuttingRecipeSOWithInput(inputKitchenObjectSO) != null;
    }

    KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        return cuttingRecipeSO != null ? cuttingRecipeSO.output : null;
    }

    CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cutKitchenObjectSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
