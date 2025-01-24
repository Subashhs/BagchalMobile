using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    public GameObject tigerPrefab;
    public GameObject goatPrefab;
    private GameObject selectedPiece;

    void Update()
    {
        // If the user clicks/taps anywhere, move the selected piece
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Move the selected piece to the new position
            if (selectedPiece != null)
            {
                selectedPiece.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            }
        }
    }

    // Call this method to select the tiger or goat
    public void SelectPiece(GameObject piece)
    {
        selectedPiece = piece;
    }
}
