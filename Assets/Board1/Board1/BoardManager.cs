using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManage instance is not initialized.");
            return;
        }

        Debug.Log("Tile or Piece clicked.");

        // Check if a piece is already selected
        if (GameManager.Instance.IsPieceSelected())
        {
            // Move the selected piece
            GameManager.Instance.MovePiece(gameObject);
        }
        else
        {
            // Select the piece
            GameManager.Instance.SelectPiece(gameObject);
        }
    }
}
