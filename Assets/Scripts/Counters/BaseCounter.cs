using UnityEngine;

public class BaseCounter : MonoBehaviour,IKitchenObjectParent
{
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
