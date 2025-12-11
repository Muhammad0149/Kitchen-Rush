using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{

    public event EventHandler<IHasProgress.OnProgressChangeArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] CuttingRecipeSOArray;

    private int CuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // 1. Counter is EMPTY
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (HasRecipewithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player carrying something that can be cut -> Put it down
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    CuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeArgs
                    {
                        ProgressNormalized = (float)CuttingProgress / cuttingRecipeSO.CuttingProgressMax
                    });
                }
                // else: Player carrying something that CANNOT be cut (do nothing)
            }
            // else: Counter is empty, Player is empty (do nothing)
        }
        else
        {
            // 2. Counter HAS an object
            if (player.HasKitchenObject())
            {
                // Player is carrying something -> Interaction logic

                // Try adding the counter's item to the player's plate (if they have one)
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf(); // Successfully added, destroy item on counter
                        // Reset progress after clearing the counter
                        CuttingProgress = 0;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeArgs
                        {
                            ProgressNormalized = 0f // Reset progress bar
                        });
                    }
                }
                // else: Player is holding a non-plate item (e.g., another tomato), do nothing
            }
            else
            {
                // FIX: Player is NOT carrying anything -> This is the pickup scenario!
                // Give the item from the counter to the player
                GetKitchenObject().SetKitchenObjectParent(player);

                // Reset progress after pickup
                CuttingProgress = 0;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeArgs
                {
                    ProgressNormalized = 0f // Reset progress bar
                });
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipewithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            //there is an object here and it can be cut
            CuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeArgs
            {
                ProgressNormalized = (float)CuttingProgress / cuttingRecipeSO.CuttingProgressMax
            });


            if (CuttingProgress >= cuttingRecipeSO.CuttingProgressMax)
            {
                //cutting complete
                KitchenObjectSO ouputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(ouputKitchenObjectSO, this);
            }
        }
    }

    private bool HasRecipewithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in CuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }

}