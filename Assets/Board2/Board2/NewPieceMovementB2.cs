using UnityEngine;

public class PieceMovementB2 : MonoBehaviour
{
    private TigerMovementB2 tigerMovement;
    private GoatMovementB2 goatMovement;

    private void Start()
    {
        // Getting references to the Tiger and Goat movement scripts.
        tigerMovement = GetComponent<TigerMovementB2>();
        goatMovement = GetComponent<GoatMovementB2>();
    }

    private void OnMouseDown()
    {
        // Check if the GameManagerBoard2 instance exists
        if (GameManagerBoard2.Instance == null)
        {
            Debug.LogError("GameManagerBoard2 instance is not initialized.");
            return;
        }

        Debug.Log("Tile or Piece clicked.");

        // Check if a piece is already selected
        if (GameManagerBoard2.Instance.IsPieceSelected())
        {
            // Handle movement based on the current turn (Tiger or Goat)
            if (GameManagerBoard2.Instance.currentTurn == GameManagerBoard2.Turn.Tiger &&
                GameManagerBoard2.Instance.selectedTiger != null)
            {
                // Try moving the tiger
                if (tigerMovement.TryMove(GameManagerBoard2.Instance.selectedTiger, gameObject, GameManagerBoard2.Instance.tiles))
                {
                    // Successfully moved the tiger
                    GameManagerBoard2.Instance.selectedTiger = null;  // Deselect the tiger
                    GameManagerBoard2.Instance.currentTurn = GameManagerBoard2.Turn.Goat;  // Switch to Goat's turn
                    GameManagerBoard2.Instance.UpdateTurnText();  // Update the UI
                }
            }
            else if (GameManagerBoard2.Instance.currentTurn == GameManagerBoard2.Turn.Goat &&
                     GameManagerBoard2.Instance.selectedGoat != null)
            {
                // Try moving the goat
                if (goatMovement.TryMove(GameManagerBoard2.Instance.selectedGoat, gameObject, GameManagerBoard2.Instance.tiles))
                {
                    // Successfully moved the goat
                    GameManagerBoard2.Instance.selectedGoat = null;  // Deselect the goat
                    GameManagerBoard2.Instance.currentTurn = GameManagerBoard2.Turn.Tiger;  // Switch to Tiger's turn
                    GameManagerBoard2.Instance.UpdateTurnText();  // Update the UI
                }
            }
        }
        else
        {
            // Select the piece (goat or tiger)
            GameManagerBoard2.Instance.SelectPiece(gameObject);
        }
    }
}
