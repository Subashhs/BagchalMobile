using UnityEngine;

public class NewBoardManagerB4 : MonoBehaviour
{
    private void OnMouseDown()
    {
       Debug.Log($"Clicked on: {gameObject.name}, Tag: {gameObject.tag}"); // Add this line
        if (GameManagerBoard4.Instance == null)
        {
            Debug.LogError("GameManagerBoard3 instance is not initialized.");
            return;
        }

        Debug.Log("Tile or Piece clicked: " + gameObject.name);

        if (GameManagerBoard4.Instance.IsPieceSelected())
        {
            GameManagerBoard4.Instance.MovePiece(gameObject);
        }
        else
        {
            GameManagerBoard4.Instance.SelectPiece(gameObject);
        }
    }
}
    
