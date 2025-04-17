using UnityEngine;

public class NewGoatSelectionB4 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard4.Instance == null)
        {
            Debug.LogError("GameManagerBoard3 instance is not initialized.");
            return;
        }

        if (GameManagerBoard4.Instance.currentTurn == GameManagerBoard4.Turn.Player)
        {
            if (!GameManagerBoard4.PlayerIsTiger && GameManagerBoard4.Instance.GetPlayerGoats().Contains(gameObject))
            
                {
                Debug.Log("Player Goat clicked.");
                GameManagerBoard4.Instance.SelectPiece(gameObject);
            }
            else
            {
                Debug.Log("Clicked object is not the player's goat.");
            }
        }
        else
        {
            Debug.Log("It's the AI's turn.");
        }
    }
}