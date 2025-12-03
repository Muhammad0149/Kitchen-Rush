using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO KitchenObjectSO;


    private IKitchenObjectParent KitchenObjectParent;
    
    
    public KitchenObjectSO GetKitchenObjectSO() 
    { 
        return KitchenObjectSO;
    }


    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if(this.KitchenObjectParent != null)
        {
            this.KitchenObjectParent.ClearKitchenObject();
        }

        this.KitchenObjectParent = kitchenObjectParent;
        
        if(kitchenObjectParent.HasKitchenObject())
        {
            Debug.Log("IKitchenObjectParent already has a kitchen object!");
        }
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return KitchenObjectParent;
    }

    public void DestroySelf()
    {
        KitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform KitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = KitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
}
