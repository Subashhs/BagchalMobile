using UnityEngine;

public class NewTigerSelectionB2 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard2.Instance == null) return;

        if (GameManagerBoard2.Instance.currentTurn == GameManagerBoard2.Turn.Tiger)
        {
            GameManagerBoard2.Instance.SelectTiger(gameObject);
        }
    }
}
