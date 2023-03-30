using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;


    public static DeliveryManager Instance { get; private set; }

    [SerializeField] RecipeListSO recipeListSO;
    List<RecipeSO> waitingRecipeSOList;

    float spawnRecipeTimer;
    float spawnRecipeTimerMax = 4f;
    int waitingRecipesMax = 4;

    void Awake()
    {
        Instance = this;

        waitingRecipeSOList = new List<RecipeSO>();
    }

    void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer += spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {

                List<RecipeSO> recipeSOList = recipeListSO.recipeSOList;
                int randomIndex = UnityEngine.Random.Range(0, recipeSOList.Count);

                RecipeSO waitingRecipeSO = recipeSOList[randomIndex];

                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // has the same number of ingredients
                bool plateContentsMatchRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    // cycling through all ingredients in the recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        //cycling through all the ingredients on the plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            //Ingredient matches
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        // This recipe ingredient was not found on the plate
                        plateContentsMatchRecipe = false;
                    }
                }
                if (plateContentsMatchRecipe)
                {
                    // Player delivered the correct recipe!
                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
    }


    public List<RecipeSO> GetWaitingRecipeSOList() => waitingRecipeSOList;
}
