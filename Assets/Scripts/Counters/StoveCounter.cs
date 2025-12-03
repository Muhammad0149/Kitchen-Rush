using System;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter
{
    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }



    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;

    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private float FryingTimer;
    private float BurningTimer;

    private void Start()
    {
        state = State.Idle;    
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {

                case State.Idle:
                    break;
                case State.Frying:
                    FryingTimer += Time.deltaTime;


                    if (FryingTimer > fryingRecipeSO.FryingTimerMax)
                    {
                        //Fried

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);


                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        state = State.Fried;
                        BurningTimer = 0f;

                    }
                    break;
                case State.Fried:
                    BurningTimer += Time.deltaTime;


                    if (BurningTimer > burningRecipeSO.BurningTimerMax)
                    {
                        //Fried

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned;

                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

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
                    //player carrying something that can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);


                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    FryingTimer = 0f;

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
                state = State.Idle;
            }
        }
    }

    private bool HasRecipewithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {

            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO FryingRecipeSO in fryingRecipeSOArray)
        {
            if (FryingRecipeSO.input == inputKitchenObjectSO)
            {
                return FryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}