using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard.Instance == null)
        {
            Debug.LogError("GameManagerBoard2 instance is not initialized.");
            return;
        }

        Debug.Log("Tile or Piece clicked.");

        // Check if a piece is already selected
        if (GameManagerBoard.Instance.IsPieceSelected())
        {
            // Move the selected piece
            GameManagerBoard.Instance.MovePiece(gameObject);
        }
        else
        {
            // Select the piece
            GameManagerBoard.Instance.SelectPiece(gameObject);
        }
    }
}
