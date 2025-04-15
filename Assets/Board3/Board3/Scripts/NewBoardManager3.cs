using UnityEngine;

public class NewBoardManagerB3 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard3.Instance == null)
        {
            Debug.LogError("GameManagerBoard3 instance is not initialized.");
            return;
        }

        Debug.Log("Tile or Piece clicked: " + gameObject.name);

        if (GameManagerBoard3.Instance.IsPieceSelected())
        {
            GameManagerBoard3.Instance.MovePiece(gameObject);
        }
        else
        {
            GameManagerBoard3.Instance.SelectPiece(gameObject);
        }
    }
}
    
