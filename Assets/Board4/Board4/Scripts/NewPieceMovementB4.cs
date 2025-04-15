using UnityEngine;
using System.Collections.Generic;

public class NewPieceMovementB4 : MonoBehaviour
{
    //  No TigerMovementB3 here.
    //  Base class for all pieces.
    public Dictionary<string, List<string>> validMoves = new Dictionary<string, List<string>>();

    public virtual List<string> GetValidMoves(string currentTileName, Dictionary<string, GameObject> tiles)
    {
        return new List<string>();
    }

    public virtual bool TryMove(GameObject selectedPiece, GameObject targetTile, Dictionary<string, GameObject> tiles)
    {
        return false;
    }

    public string GetTileName(Vector3 position, Dictionary<string, GameObject> tiles)
    {
        foreach (KeyValuePair<string, GameObject> pair in tiles)
        {
            if (Vector3.Distance(position, pair.Value.transform.position) < 0.1f)
            {
                return pair.Key;
            }
        }
        return null;
    }

    public bool IsTileOccupied(GameObject tile)
    {
        return tile.transform.childCount > 0;
    }

    protected List<string> GetAdjacentTiles(string tileName) // Changed to protected
    {
        if (validMoves.ContainsKey(tileName))
        {
            return validMoves[tileName];
        }
        return new List<string>();
    }
}