// GoatMovementB3.cs
using UnityEngine;
using System.Collections.Generic;

public class GoatMovementB4 : MonoBehaviour
{
    public Dictionary<string, List<string>> validMoves = new Dictionary<string, List<string>>()
    {
        { "Tile_0_0", new List<string> { "Tile_1_2", "Tile_1_0", "Tile_1_1" } },
        { "Tile_1_0", new List<string> { "Tile_0_0", "Tile_1_1", "Tile_2_0" } },
        { "Tile_1_1", new List<string> { "Tile_0_0", "Tile_1_0", "Tile_1_2" } },
        { "Tile_1_2", new List<string> { "Tile_0_0", "Tile_1_1", "Tile_2_2" } },
        { "Tile_2_0", new List<string> { "Tile_1_0", "Tile_2_1" } },
        { "Tile_2_1", new List<string> { "Tile_2_0", "Tile_2_2", "Tile_3_1" } },
        { "Tile_2_2", new List<string> { "Tile_1_2", "Tile_2_1" } },
        { "Tile_3_0", new List<string> { "Tile_3_1" } },
        { "Tile_3_1", new List<string> { "Tile_2_1", "Tile_3_0", "Tile_3_2", "Tile_4_1" } },
        { "Tile_3_2", new List<string> { "Tile_3_1" } },
        { "Tile_4_0", new List<string> { "Tile_4_1" } },
        { "Tile_4_1", new List<string> { "Tile_3_1", "Tile_4_0", "Tile_4_2" } },
        { "Tile_4_2", new List<string> { "Tile_4_1" } }
    };

    public bool TryMove(GameObject goat, GameObject targetTile, Dictionary<string, GameObject> boardTiles)
    {
        string currentTile = GetTileName(goat.transform.position, boardTiles);
        string targetTileName = targetTile.name;

        if (currentTile == targetTileName)
        {
            Debug.LogWarning($"Cannot move goat to the same tile: {currentTile}.");
            return false;
        }

        if (validMoves.ContainsKey(currentTile) && validMoves[currentTile].Contains(targetTileName))
        {
            if (!IsTileOccupied(targetTile))
            {
                goat.transform.position = targetTile.transform.position;
                Debug.Log($"Goat moved from {currentTile} to {targetTileName}.");
                return true;
            }
            else
            {
                Debug.LogWarning($"Cannot move goat to {targetTileName} because it is occupied.");
            }
        }
        else
        {
            Debug.LogWarning($"Invalid move for goat from {currentTile} to {targetTileName}.");
        }

        return false;
    }

    private string GetTileName(Vector3 position, Dictionary<string, GameObject> boardTiles)
    {
        foreach (var tile in boardTiles)
        {
            if (Vector3.Distance(tile.Value.transform.position, position) < 0.1f)
                return tile.Key;
        }
        Debug.LogError("Goat position does not match any tile.");
        return string.Empty;
    }

    private bool IsTileOccupied(GameObject tile)
    {
        Vector3 tilePos = tile.transform.position;
        if (GameManagerBoard4.Instance.GetTiger() != null &&
            Vector3.Distance(GameManagerBoard4.Instance.GetTiger().transform.position, tilePos) < 0.1f)
        {
            return true;
        }
        foreach (var goat in GameManagerBoard4.Instance.GetAllGoats())
        {
            if (goat != null && Vector3.Distance(goat.transform.position, tilePos) < 0.1f)
            {
                return true;
            }
        }
        return false;
    }
}