using System;
using UnityEngine;

public class CuttingCounter : BaseCounter 
{

    public event EventHandler<OnProgressChangeArgs> OnProgressChanged;
    public class OnProgressChangeArgs : EventArgs 
    {
        public float ProgressNormalized;
    }

    public event EventHandler OnCut;


    [SerializeField] private CuttingRecipeSO[] CuttingRecipeSOArray;

    private int CuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //has no object
            if (player.HasKitchenObject())
            {
                //player is carrying something
                if (HasRecipewithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //player carrying something with recipe
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    CuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());


                    OnProgressChanged?.Invoke(this, new OnProgressChangeArgs
                    {
                        ProgressNormalized = (float) CuttingProgress / cuttingRecipeSO.CuttingProgressMax
                    });
                }
            }
            else
            {
                //player not carrying something
            }
        }
        else
        {
            //has object
            if (player.HasKitchenObject())
            {
                //player is carrying something
            }
            else
            {
                //player is not carrying something
                GetKitchenObject().SetKitchenObjectParent(player);
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

            OnProgressChanged?.Invoke(this, new OnProgressChangeArgs
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
