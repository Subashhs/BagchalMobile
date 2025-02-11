using UnityEngine;

public class NewTigerSelectionB3 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard3.Instance == null) return;

        if (GameManagerBoard3.Instance.currentTurn == GameManagerBoard3.Turn.Tiger)
        {
            GameManagerBoard3.Instance.SelectTiger(gameObject);
        }
    }
}
