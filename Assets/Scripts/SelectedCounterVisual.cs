using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter BaseCounter;
    [SerializeField] private GameObject[] VisualGameObjectArray;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.SelectedCounter == BaseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Show()
    {
        foreach (GameObject VisualGameObject in VisualGameObjectArray)
        {
            VisualGameObject.SetActive(true);
        }
    }
    private void Hide()
    {
        foreach (GameObject VisualGameObject in VisualGameObjectArray)
        {
            VisualGameObject.SetActive(false);
        }
    }   
}
