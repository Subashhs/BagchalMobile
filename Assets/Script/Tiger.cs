using UnityEngine;

public class Tiger : MonoBehaviour
{
    public LayerMask goatLayer;

    private Vector2 targetPosition;

    void OnMouseDown()
    {
        Debug.Log("Tiger selected");
        // Allow the player to move this tiger
    }

    public void MoveTo(Vector2 position)
    {
        transform.position = position;
    }

    public void CaptureGoat(GameObject goat)
    {
        Destroy(goat);
    }
}
