using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectSO> KitchenObjectSOList;

    public string RecipeName;

}
