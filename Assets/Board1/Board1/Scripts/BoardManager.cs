using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private void OnMouseDown()
    {
        // If it's the Tiger's turn and the player has selected a tiger, let them move it
        if (GameManager.Instance.currentTurn == GameManager.Turn.Tiger && GameManager.Instance.IsTigerSelected())
        {
            GameManager.Instance.MoveTiger(gameObject);
        }

        // If it's the Goat's turn, and no goat is selected, allow placing a goat
        else if (GameManager.Instance.currentTurn == GameManager.Turn.Goat && GameManager.Instance.GetPlacedGoats().Count < 20)
        {
            GameManager.Instance.PlaceGoat(gameObject);
        }

        // Goat movement phase (after 20 goats)
        else if (GameManager.Instance.currentTurn == GameManager.Turn.Goat && GameManager.Instance.GetPlacedGoats().Count >= 20)
        {
            if (GameManager.Instance.GetSelectedGoat() != null)
            {
                GameManager.Instance.MoveGoat(gameObject);
            }
            else
            {
                // Select a goat to move
                GameManager.Instance.SelectGoat(gameObject);
            }
        }
    }
}
