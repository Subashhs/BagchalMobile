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
        GameManagerBoard2.Instance.SelectPiece(gameObject);
    }
}
