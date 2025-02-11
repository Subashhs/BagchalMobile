using UnityEngine;

public class NewPieceMovementB2 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard2.Instance == null) return;

        if (GameManagerBoard2.Instance.currentTurn == GameManagerBoard2.Turn.Goat)
        {
            if (GameManagerBoard2.Instance.GetSelectedGoat() == null)
            {
                GameManagerBoard2.Instance.SelectGoat(gameObject);
            }
        }
    }
}
