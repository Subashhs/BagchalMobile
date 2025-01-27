using UnityEngine;

public class PositionButton : MonoBehaviour
{
    public int x;
    public int y;

    public void OnClick()
    {
        GameManager.Instance.OnPositionClicked(x, y);
    }
}
