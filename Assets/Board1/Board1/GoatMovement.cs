using UnityEngine;
using System.Collections.Generic;

public class GoatMovement : MonoBehaviour
{
    // Define valid moves for the goat. These are the exact tiles the goat can move to from each tile.
    private Dictionary<string, List<string>> validMoves = new Dictionary<string, List<string>>()
    {
        { "Tile_0_0", new List<string> { "Tile_0_1", "Tile_1_0", "Tile_1_1" } },
        { "Tile_0_1", new List<string> { "Tile_0_0", "Tile_0_2", "Tile_1_1" } },
        { "Tile_0_2", new List<string> { "Tile_0_1", "Tile_0_3", "Tile_1_1", "Tile_1_2", "Tile_1_3" } },
        { "Tile_0_3", new List<string> { "Tile_0_2", "Tile_0_4", "Tile_1_3" } },
        { "Tile_0_4", new List<string> { "Tile_0_3", "Tile_1_3", "Tile_1_4" } },
        { "Tile_1_0", new List<string> { "Tile_0_0", "Tile_1_1", "Tile_2_0" } },
        { "Tile_1_1", new List<string> { "Tile_0_0", "Tile_0_1", "Tile_0_2", "Tile_1_0", "Tile_1_2", "Tile_2_0", "Tile_2_1", "Tile_2_2" } },
        { "Tile_1_2", new List<string> { "Tile_0_2", "Tile_1_1", "Tile_2_2", "Tile_1_3" } },
        { "Tile_1_3", new List<string> { "Tile_0_2", "Tile_0_3", "Tile_0_4", "Tile_1_2", "Tile_1_4", "Tile_2_2", "Tile_2_3", "Tile_2_4" } },
        { "Tile_1_4", new List<string> { "Tile_0_4", "Tile_1_3", "Tile_2_4" } },
        { "Tile_2_0", new List<string> { "Tile_1_0", "Tile_1_1", "Tile_2_1", "Tile_3_0", "Tile_3_1" } },
        { "Tile_2_1", new List<string> { "Tile_1_1", "Tile_2_0", "Tile_2_2", "Tile_3_1" } },
        { "Tile_2_2", new List<string> { "Tile_1_1", "Tile_1_2", "Tile_1_3", "Tile_2_1", "Tile_2_3", "Tile_3_1", "Tile_3_2", "Tile_3_3" } },
        { "Tile_2_3", new List<string> { "Tile_1_3", "Tile_2_2", "Tile_2_4", "Tile_3_3" } },
        { "Tile_2_4", new List<string> { "Tile_1_3", "Tile_1_4", "Tile_2_3", "Tile_3_3", "Tile_3_4" } },
        { "Tile_3_0", new List<string> { "Tile_2_0", "Tile_3_1", "Tile_4_0" } },
        { "Tile_3_1", new List<string> { "Tile_2_0", "Tile_2_1", "Tile_2_2", "Tile_3_0", "Tile_3_2", "Tile_4_0", "Tile_4_1", "Tile_4_2" } },
        { "Tile_3_2", new List<string> { "Tile_2_2", "Tile_3_1", "Tile_3_3", "Tile_4_2" } },
        { "Tile_3_3", new List<string> { "Tile_2_2", "Tile_2_3", "Tile_2_4", "Tile_3_2", "Tile_3_4", "Tile_4_2", "Tile_4_3", "Tile_4_4" } },
        { "Tile_3_4", new List<string> { "Tile_2_4", "Tile_3_3", "Tile_4_4" } },
        { "Tile_4_0", new List<string> { "Tile_3_0", "Tile_3_1", "Tile_4_1" } },
        { "Tile_4_1", new List<string> { "Tile_3_1", "Tile_4_0", "Tile_4_2" } },
        { "Tile_4_2", new List<string> { "Tile_3_1", "Tile_3_2", "Tile_3_3", "Tile_4_1", "Tile_4_3" } },
        { "Tile_4_3", new List<string> { "Tile_3_3", "Tile_4_2", "Tile_4_4" } },
        { "Tile_4_4", new List<string> { "Tile_3_3", "Tile_3_4", "Tile_4_3" } }
    };


    // Try to move the goat to the target tile
    public bool TryMove(GameObject goat, GameObject targetTile, Dictionary<string, GameObject> boardTiles)
    {
        string currentTile = GetTileName(goat.transform.position, boardTiles); // Get the current tile name
        string targetTileName = targetTile.name; // Get the name of the target tile

        // If the target tile is the same as the current tile, return false
        if (currentTile == targetTileName)
        {
            Debug.LogWarning($"Cannot move goat to the same tile: {currentTile}.");
            return false;
        }

        // Check if the target tile is a valid move from the current tile
        if (validMoves.ContainsKey(currentTile) && validMoves[currentTile].Contains(targetTileName))
        {
            // Check if the target tile is occupied
            if (!IsTileOccupied(targetTile))
            {
                // Move the goat to the target tile
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
            Debug.LogWarning($"Invalid move for goat from {currentTile} to {targetTileName}. This move is not allowed.");
        }

        return false;
    }

    // Get the name of the tile based on the position
    private string GetTileName(Vector3 position, Dictionary<string, GameObject> boardTiles)
    {
        foreach (var tile in boardTiles)
        {
            if (Vector3.Distance(tile.Value.transform.position, position) < 0.1f)
                return tile.Key;  // Return the name of the tile if the position is close enough
        }
        Debug.LogError("Goat position does not match any tile.");
        return string.Empty;
    }

    // Check if the target tile is occupied by any piece
    private bool IsTileOccupied(GameObject tile)
    {
        foreach (var goat in GameManager.Instance.goats)
        {
            if (Vector3.Distance(goat.transform.position, tile.transform.position) < 0.1f)
            {
                return true;  // The tile is occupied by a goat
            }
        }

        // Check if the tiger is occupying the tile
        if (GameManager.Instance.tiger != null &&
            Vector3.Distance(GameManager.Instance.tiger.transform.position, tile.transform.position) < 0.1f)
        {
            return true;  // The tile is occupied by a tiger
        }

        return false;  // The tile is not occupied
    }
}
