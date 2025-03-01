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
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is not initialized.");
            return;
        }

        Debug.Log("Tile or Piece clicked.");

        // Check if a piece is already selected
        if (GameManager.Instance.IsPieceSelected())
        {
            // Handle movement based on the current turn (Tiger or Goat)
            if (GameManager.Instance.currentTurn == GameManager.Turn.Tiger &&
                GameManager.Instance.selectedTiger != null)
            {
                // Try moving the tiger
                if (tigerMovement.TryMove(GameManager.Instance.selectedTiger, gameObject, GameManager.Instance.tiles))
                {
                    // Successfully moved the tiger
                    GameManager.Instance.selectedTiger = null;  // Deselect the tiger
                    GameManager.Instance.currentTurn = GameManager.Turn.Goat;  // Switch to Goat's turn
                    GameManager.Instance.UpdateTurnText();  // Update the UI
                }
            }
            else if (GameManager.Instance.currentTurn == GameManager.Turn.Goat &&
                     GameManager.Instance.selectedGoat != null)
            {
                // Try moving the goat
                if (goatMovement.TryMove(GameManager.Instance.selectedGoat, gameObject, GameManager.Instance.tiles))
                {
                    // Successfully moved the goat
                    GameManager.Instance.selectedGoat = null;  // Deselect the goat
                    GameManager.Instance.currentTurn = GameManager.Turn.Tiger;  // Switch to Tiger's turn
                    GameManager.Instance.UpdateTurnText();  // Update the UI
                }
            }
        }
        else
        {
            // Select the piece (goat or tiger)
            GameManager.Instance.SelectPiece(gameObject);
        }
    }
}
