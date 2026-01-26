using System;
using UnityEditor;
using UnityEngine;

public class BaseCounter : MonoBehaviour,IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlacedHere;

    [SerializeField] private Transform CounterTopPoint;

    private KitchenObject KitchenObject;

    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }
    public virtual void InteractAlternate(Player player)
    {
        //Debug.LogError("BaseCounter.InteractAlternate();");
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return CounterTopPoint;
    }
    public void SetKitchenObject(KitchenObject KitchenObject)
    {
        this.KitchenObject = KitchenObject;
        if (KitchenObject != null) 
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return KitchenObject;
    }
    public void ClearKitchenObject()
    {
        KitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return KitchenObject != null;
    }
}
