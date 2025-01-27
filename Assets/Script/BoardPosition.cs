using UnityEngine;
using UnityEngine.UI;

public class BoardPosition : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            string positionKey = button.name; // Use the button's name as the position key
            button.onClick.AddListener(() =>
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.OnPositionClicked(button, positionKey); // Pass both button and positionKey
                }
            });
        }
    }
}
