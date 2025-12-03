using System;
using UnityEngine;

public class ContainerCounter : BaseCounter { 
    
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSO KitchenObjectSO;


    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //Player is not carrying anything
            KitchenObject.SpawnKitchenObject(KitchenObjectSO, player);


            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
