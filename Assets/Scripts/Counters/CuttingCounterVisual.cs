using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private const string CUT = "Cut";

    [SerializeField] private CuttingCounter CuttingCounter;

    private Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }
    private void Start()
    {
        CuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        Animator.SetTrigger(CUT);
    }
}
