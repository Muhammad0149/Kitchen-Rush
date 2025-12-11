using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO KitchenObjectSO;


    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // 1. Counter is EMPTY
            if (player.HasKitchenObject())
            {
                // Player is carrying something -> Put it down
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            // 2. Counter HAS an object
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf(); // Successfully added, destroy item on counter
                    }
                }
                else
                {
                    // Player is holding a non-plate item (e.g., another tomato, etc.)
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // Counter is holding a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            // Successfully added, destroy item in player's hand
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                // Give the item from the counter to the player
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            }
        }
    } 
