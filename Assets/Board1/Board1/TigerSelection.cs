using UnityEngine;

public class NewTigerSelection : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard.Instance == null)
        {
            Debug.LogError("GameManagerBoard2 instance is not initialized.");
            return;
        }

        Debug.Log("Tiger clicked.");

        // Select the tiger
        GameManagerBoard.Instance.SelectPiece(gameObject);
    }
}
