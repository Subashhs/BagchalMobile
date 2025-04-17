using UnityEngine;

public class NewTigerSelectionB4 : MonoBehaviour
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
            if (GameManagerBoard4.PlayerIsTiger)
            {
                Debug.Log("Player Tiger clicked.");
                GameManagerBoard4.Instance.SelectPiece(gameObject);
            }
            else
            {
                Debug.Log("Clicked object is not the player's tiger.");
            }
        }
        else
        {
            Debug.Log("It's the AI's turn.");
        }
    }
}