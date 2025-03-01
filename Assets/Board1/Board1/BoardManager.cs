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
            GameManager.Instance.PlaceGoat(gameObject); // Place goat on selected tile
        }

        // After placing 20 goats, allow goat movement
        else if (GameManager.Instance.currentTurn == GameManager.Turn.Goat && GameManager.Instance.GetPlacedGoats().Count >= 20)
        {
            // If a goat is selected, move it
            if (GameManager.Instance.GetSelectedGoat() != null)
            {
                GameManager.Instance.MoveGoat(gameObject);
            }
            else
            {
                // If no goat is selected, select a goat to move
                GameManager.Instance.SelectGoat(gameObject);
            }
        }
    }
}