using UnityEngine;
using System.Collections.Generic;

public abstract class BaseAI2
{
    protected Dictionary<string, GameObject> tiles;

    public BaseAI2(Dictionary<string, GameObject> boardTiles)
    {
        tiles = boardTiles;
    }

    public abstract bool MakeMove();
}