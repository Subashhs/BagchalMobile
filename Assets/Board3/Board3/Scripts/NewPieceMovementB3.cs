using UnityEngine;

public class NewPieceMovementB3 : MonoBehaviour
{
    private TigerMovementB3 tigerMovement;
    private GoatMovementB3 goatMovement;

    private void Start()
    {
        // Getting references to the Tiger and Goat movement scripts.
        tigerMovement = GetComponent<TigerMovementB3>();
        goatMovement = GetComponent<GoatMovementB3>();
    }

    private void OnMouseDown()
    {
        // Check if the GameManagerBoard2 instance exists
        if (GameManagerBoard3.Instance == null)
        {
            Debug.LogError("GameManagerBoard2 instance is not initialized.");
            return;
        }

        Debug.Log("Tile or Piece clicked.");

        // Check if a piece is already selected
        if (GameManagerBoard3.Instance.IsPieceSelected())
        {
            // Handle movement based on the current turn (Tiger or Goat)
            if (GameManagerBoard3.Instance.currentTurn == GameManagerBoard3.Turn.Tiger &&
                GameManagerBoard3.Instance.selectedTiger != null)
            {
                // Try moving the tiger
                if (tigerMovement.TryMove(GameManagerBoard3.Instance.selectedTiger, gameObject, GameManagerBoard3.Instance.tiles))
                {
                    // Successfully moved the tiger
                    GameManagerBoard3.Instance.selectedTiger = null;  // Deselect the tiger
                    GameManagerBoard3.Instance.currentTurn = GameManagerBoard3.Turn.Goat;  // Switch to Goat's turn
                    GameManagerBoard3.Instance.UpdateTurnText();  // Update the UI
                }
            }
            else if (GameManagerBoard3.Instance.currentTurn == GameManagerBoard3.Turn.Goat &&
                     GameManagerBoard3.Instance.selectedGoat != null)
            {
                // Try moving the goat
                if (goatMovement.TryMove(GameManagerBoard3.Instance.selectedGoat, gameObject, GameManagerBoard3.Instance.tiles))
                {
                    // Successfully moved the goat
                    GameManagerBoard3.Instance.selectedGoat = null;  // Deselect the goat
                    GameManagerBoard3.Instance.currentTurn = GameManagerBoard3.Turn.Tiger;  // Switch to Tiger's turn
                    GameManagerBoard3.Instance.UpdateTurnText();  // Update the UI
                }
            }
        }
        else
        {
            // Select the piece (goat or tiger)
            GameManagerBoard3.Instance.SelectPiece(gameObject);
        }
    }
}