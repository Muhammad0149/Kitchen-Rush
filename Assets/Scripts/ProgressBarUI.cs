using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{

    [SerializeField] CuttingCounter CuttingCounter;
    [SerializeField] private Image BarImage;


    private void Start()
    {
        CuttingCounter.OnProgressChanged += CuttingCounter_OnProgressChanged;
        BarImage.fillAmount = 0f;

        Hide();
    }

    private void CuttingCounter_OnProgressChanged(object sender, CuttingCounter.OnProgressChangeArgs e)
    {
        BarImage.fillAmount = e.ProgressNormalized;

        if (e.ProgressNormalized == 0f || e.ProgressNormalized == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

