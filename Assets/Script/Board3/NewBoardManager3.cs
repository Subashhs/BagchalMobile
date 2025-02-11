using UnityEngine;

public class NewBoardManagerB3 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard3.Instance == null) return;

        // Tiger Movement
        if (GameManagerBoard3.Instance.currentTurn == GameManagerBoard3.Turn.Tiger && GameManagerBoard3.Instance.IsTigerSelected())
        {
            GameManagerBoard3.Instance.MoveTiger(gameObject);
            return;
        }

        // Goat Movement
        if (GameManagerBoard3.Instance.currentTurn == GameManagerBoard3.Turn.Goat)
        {
            if (GameManagerBoard3.Instance.GetSelectedGoat() != null)
            {
                GameManagerBoard3.Instance.MoveGoat(gameObject);
            }
        }
    }
}
