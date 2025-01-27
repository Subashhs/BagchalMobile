using UnityEngine;

public class TigerMovement : MonoBehaviour
{
    private Vector3 startPosition;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnMouseDown()
    {
        startPosition = transform.position; // Record the start position
        Debug.Log("Tiger selected at: " + startPosition);
    }

    void OnMouseUp()
    {
        // Add logic to validate move and snap tiger to the new position
    }
}
