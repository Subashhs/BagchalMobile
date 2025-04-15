using UnityEngine;

public class PieceMovementB3 : MonoBehaviour
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
        if (GameManagerBoard3.Instance == null)
        {
            Debug.LogError("GameManagerBoard3 instance is not initialized.");
            return;
        }

        Debug.Log("Tile or Piece clicked.");

        if (GameManagerBoard3.Instance.IsPieceSelected())
        {
            if (GameManagerBoard3.Instance.currentTurn == GameManagerBoard3.Turn.Tiger &&
                GameManagerBoard3.Instance.selectedTiger != null && tigerMovement != null) // added null check
            {
                if (tigerMovement.TryMove(GameManagerBoard3.Instance.selectedTiger, gameObject, GameManagerBoard3.Instance.tiles))
                {
                    GameManagerBoard3.Instance.selectedTiger = null;
                    GameManagerBoard3.Instance.currentTurn = GameManagerBoard3.Turn.Goat;
                    GameManagerBoard3.Instance.UpdateTurnText();
                }
            }
            else if (GameManagerBoard3.Instance.currentTurn == GameManagerBoard3.Turn.Goat &&
                     GameManagerBoard3.Instance.selectedGoat != null && goatMovement != null) // added null check
            {
                if (goatMovement.TryMove(GameManagerBoard3.Instance.selectedGoat, gameObject, GameManagerBoard3.Instance.tiles))
                {
                    GameManagerBoard3.Instance.selectedGoat = null;
                    GameManagerBoard3.Instance.currentTurn = GameManagerBoard3.Turn.Tiger;
                    GameManagerBoard3.Instance.UpdateTurnText();
                }
            }
        }
        else
        {
            GameManagerBoard3.Instance.SelectPiece(gameObject);
        }
    }
}