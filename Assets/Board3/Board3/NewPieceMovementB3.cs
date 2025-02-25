using UnityEngine;

public class NewPieceMovementB3 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard3.Instance == null) return;

        if (GameManagerBoard3.Instance.currentTurn == GameManagerBoard3.Turn.Goat)
        {
            if (GameManagerBoard3.Instance.GetSelectedGoat() == null)
            {
                GameManagerBoard3.Instance.SelectGoat(gameObject);
            }
        }
    }
}
