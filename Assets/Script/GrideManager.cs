using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridTilePrefab; // Prefab for the grid tile
    public int gridSize = 5;         // 5x5 grid

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        float tileSize = 1f; // Size of each grid tile
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(x * tileSize, y * tileSize, 0);
                Instantiate(gridTilePrefab, position, Quaternion.identity, transform);
            }
        }
    }
}