using UnityEngine;

public class NewTigerSelectionB2 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard2.Instance == null)
        {
            Debug.LogError("GameManagerBoard2 instance is not initialized.");
            return;
        }

        Debug.Log("Tiger clicked.");

        // Select the tiger
        if (GameManagerBoard2.Instance.currentTurn == GameManagerBoard2.Turn.Tiger)
        {
            GameManagerBoard2.Instance.SelectPiece(gameObject);
        }
        else
        {
            Debug.LogWarning("Invalid selection for the current turn.");
        }
    }
}