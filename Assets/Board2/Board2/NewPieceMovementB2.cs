using UnityEngine;

public class PieceMovementB2 : MonoBehaviour
{
    private TigerMovementB2 tigerMovement;
    private GoatMovementB2 goatMovement;

    private void Start()
    {
        tigerMovement = GetComponent<TigerMovementB2>();
        goatMovement = GetComponent<GoatMovementB2>();
    }

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
            if (GameManagerBoard2.Instance.currentTurn == GameManagerBoard2.Turn.Tiger &&
                GameManagerBoard2.Instance.selectedTiger != null)
            {
                if (tigerMovement.TryMove(GameManagerBoard2.Instance.selectedTiger, gameObject, GameManagerBoard2.Instance.tiles))
                {
                    // Successfully moved the tiger
                    GameManagerBoard2.Instance.selectedTiger = null;
                    GameManagerBoard2.Instance.currentTurn = GameManagerBoard2.Turn.Goat;
                    GameManagerBoard2.Instance.UpdateTurnText();
                }
            }
            else if (GameManagerBoard2.Instance.currentTurn == GameManagerBoard2.Turn.Goat &&
                     GameManagerBoard2.Instance.selectedGoat != null)
            {
                if (goatMovement.TryMove(GameManagerBoard2.Instance.selectedGoat, gameObject, GameManagerBoard2.Instance.tiles))
                {
                    // Successfully moved the goat
                    GameManagerBoard2.Instance.selectedGoat = null;
                    GameManagerBoard2.Instance.currentTurn = GameManagerBoard2.Turn.Tiger;
                    GameManagerBoard2.Instance.UpdateTurnText();
                }
            }
        }
        else
        {
            // Select the piece
            GameManagerBoard2.Instance.SelectPiece(gameObject);
        }
    }
}
