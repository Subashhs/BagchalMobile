using UnityEngine;

public class NewTigerSelectionB5 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard5.Instance == null)
        {
            Debug.LogError("GameManagerBoard3 instance is not initialized.");
            return;
        }

        if (GameManagerBoard5.Instance.currentTurn == GameManagerBoard5.Turn.Player)
        {
            if (GameManagerBoard5.PlayerIsTiger)
            {
                Debug.Log("Player Tiger clicked.");
                GameManagerBoard5.Instance.SelectPiece(gameObject);
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