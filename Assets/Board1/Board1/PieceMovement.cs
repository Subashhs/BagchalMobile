using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    private TigerMovement tigerMovement;
    private GoatMovement goatMovement;

    private void Start()
    {
        // Getting references to the Tiger and Goat movement scripts.
        tigerMovement = GetComponent<TigerMovement>();
        goatMovement = GetComponent<GoatMovement>();
    }

    private void OnMouseDown()
    {
        // Check if the GameManagerBoard2 instance exists
        if (GameManagerBoard.Instance == null)
        {
            Debug.LogError("GameManagerBoard2 instance is not initialized.");
            return;
        }

        Debug.Log("Tile or Piece clicked.");

        // Check if a piece is already selected
        if (GameManagerBoard.Instance.IsPieceSelected())
        {
            // Handle movement based on the current turn (Tiger or Goat)
            if (GameManagerBoard.Instance.currentTurn == GameManagerBoard.Turn.Tiger &&
                GameManagerBoard.Instance.selectedTiger != null)
            {
                // Try moving the tiger
                if (tigerMovement.TryMove(GameManagerBoard.Instance.selectedTiger, gameObject, GameManagerBoard.Instance.tiles))
                {
                    // Successfully moved the tiger
                    GameManagerBoard.Instance.selectedTiger = null;  // Deselect the tiger
                    GameManagerBoard.Instance.currentTurn = GameManagerBoard.Turn.Goat;  // Switch to Goat's turn
                    GameManagerBoard.Instance.UpdateTurnText();  // Update the UI
                }
            }
            else if (GameManagerBoard.Instance.currentTurn == GameManagerBoard.Turn.Goat &&
                     GameManagerBoard.Instance.selectedGoat != null)
            {
                // Try moving the goat
                if (goatMovement.TryMove(GameManagerBoard.Instance.selectedGoat, gameObject, GameManagerBoard.Instance.tiles))
                {
                    // Successfully moved the goat
                    GameManagerBoard.Instance.selectedGoat = null;  // Deselect the goat
                    GameManagerBoard.Instance.currentTurn = GameManagerBoard.Turn.Tiger;  // Switch to Tiger's turn
                    GameManagerBoard.Instance.UpdateTurnText();  // Update the UI
                }
            }
        }
        else
        {
            // Select the piece (goat or tiger)
            GameManagerBoard.Instance.SelectPiece(gameObject);
        }
    }
}
