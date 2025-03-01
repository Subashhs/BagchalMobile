using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool occupied = false;

    public void SetOccupied(bool state)
    {
        occupied = state;
    }

    public bool IsOccupied()
    {
        return occupied;
    }
}