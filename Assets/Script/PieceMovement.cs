using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    // This is where you're trying to access the tiger's selection status
    private void SomeMethod()
    {
        if (GameManager.Instance.IsTigerSelected())  // Using the public method to check if the tiger is selected
        {
            // Proceed with the tiger move logic
            Debug.Log("Tiger is selected, proceed with movement.");
        }
        else
        {
            // Handle the case when no tiger is selected
            Debug.Log("No tiger selected.");
        }
    }
}
