using System;
using UnityEngine;

public interface IHasProgress 
{
    public event EventHandler<OnProgressChangeArgs> OnProgressChanged;
    public class OnProgressChangeArgs : EventArgs
    {
        public float ProgressNormalized;
    }

}
