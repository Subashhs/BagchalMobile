using UnityEngine;

public class NewBoardManagerB3 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard3.Instance == null)
        {
            Debug.LogError("GameManagerBoard2 instance is not initialized.");
            return;
        }

        Debug.Log("Tile or Piece clicked.");

        // Check if a piece is already selected
        if (GameManagerBoard3.Instance.IsPieceSelected())
        {
            // Move the selected piece
            GameManagerBoard3.Instance.MovePiece(gameObject);
        }
        else
        {
            // Select the piece
            GameManagerBoard3.Instance.SelectPiece(gameObject);
        }
    }
}
