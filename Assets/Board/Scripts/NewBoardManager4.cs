using UnityEngine;

public class NewBoardManagerB4 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard4.Instance != null && GameManagerBoard4.Instance.IsPieceSelected() && GameManagerBoard4.Instance.currentTurn == GameManagerBoard4.Turn.Player)
        {
            Debug.Log($"Tile {gameObject.name} clicked for movement.");
            GameManagerBoard4.Instance.MovePiece(gameObject);
        }
        else if (GameManagerBoard4.Instance != null && !GameManagerBoard4.Instance.IsPieceSelected())
        {
            Debug.Log($"Tile {gameObject.name} clicked, but no piece is selected.");
            // Optionally provide feedback to the player that they need to select a piece first.
        }
        else if (GameManagerBoard4.Instance != null && GameManagerBoard4.Instance.currentTurn == GameManagerBoard4.Turn.AI)
        {
            Debug.Log("It's the AI's turn. Cannot select or move.");
        }
    }
}