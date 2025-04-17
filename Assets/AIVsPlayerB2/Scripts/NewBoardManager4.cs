using UnityEngine;

public class NewBoardManagerB5 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard5.Instance != null && GameManagerBoard5.Instance.IsPieceSelected() && GameManagerBoard5.Instance.currentTurn == GameManagerBoard5.Turn.Player)
        {
            Debug.Log($"Tile {gameObject.name} clicked for movement.");
            GameManagerBoard5.Instance.MovePiece(gameObject);
        }
        else if (GameManagerBoard5.Instance != null && !GameManagerBoard5.Instance.IsPieceSelected())
        {
            Debug.Log($"Tile {gameObject.name} clicked, but no piece is selected.");
            // Optionally provide feedback to the player that they need to select a piece first.
        }
        else if (GameManagerBoard5.Instance != null && GameManagerBoard5.Instance.currentTurn == GameManagerBoard5.Turn.AI)
        {
            Debug.Log("It's the AI's turn. Cannot select or move.");
        }
    }
}