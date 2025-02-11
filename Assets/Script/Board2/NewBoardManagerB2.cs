using UnityEngine;

public class NewBoardManagerB2 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard2.Instance == null) return;

        // Tiger Movement
        if (GameManagerBoard2.Instance.currentTurn == GameManagerBoard2.Turn.Tiger && GameManagerBoard2.Instance.IsTigerSelected())
        {
            GameManagerBoard2.Instance.MoveTiger(gameObject);
            return;
        }

        // Goat Movement
        if (GameManagerBoard2.Instance.currentTurn == GameManagerBoard2.Turn.Goat)
        {
            if (GameManagerBoard2.Instance.GetSelectedGoat() != null)
            {
                GameManagerBoard2.Instance.MoveGoat(gameObject);
            }
        }
    }
}
