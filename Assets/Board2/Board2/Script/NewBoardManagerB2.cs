using UnityEngine;

public class NewBoardManagerB2 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard2.Instance == null)
        {
            Debug.LogError("GameManagerBoard2 instance is not initialized.");
            return;
        }

        Debug.Log("Tile or Piece clicked.");

        // Check if a piece is already selected
        if (GameManagerBoard2.Instance.IsPieceSelected())
        {
            // Move the selected piece
            GameManagerBoard2.Instance.MovePiece(gameObject);
        }
        else
        {
            // Select the piece
            GameManagerBoard2.Instance.SelectPiece(gameObject);
        }
    }
}
