using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTile
{
    public Vector2Int tier1; //this is also the location/position
    public List<Vector2Int> tier2;
    public List<Vector2Int> tier3;
    public Vector2Int tile_size = new Vector2Int(5,5); //how many tiles it occupies
    public int reserve_amount = 3;

    public ResourceTile(int x, int y, Vector2Int min_coords, Vector2Int max_coords)
    {
        tier1 = new Vector2Int(x, y);

        tier2 = new List<Vector2Int>();
        Vector2Int tier2_min_coords = new Vector2Int(x - 1, y - 1);
        Vector2Int tier2_max_coords = new Vector2Int(x + 1, y + 1);
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2Int coords = new Vector2Int(tier2_min_coords.x + i, tier2_min_coords.y + j);
                if (coords.x >= min_coords.x && coords.x <= max_coords.x &&
                    coords.y >= min_coords.y && coords.y <= max_coords.y)
                {
                    if (coords.x == x && coords.y == y)
                    {
                        continue; //skip if within tier1
                    }
                    tier2.Add(coords);
                }
            }
        }

        tier3 = new List<Vector2Int>();
        Vector2Int tier3_min_coords = new Vector2Int(x - 2, y - 2);
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2Int coords = new Vector2Int(tier3_min_coords.x + i, tier3_min_coords.y + j);
                if (coords.x >= min_coords.x && coords.x <= max_coords.x &&
                    coords.y >= min_coords.y && coords.y <= max_coords.y)
                {
                    if (coords.x >= tier2_min_coords.x && coords.x <= tier2_max_coords.x &&
                        coords.y >= tier2_min_coords.y && coords.y <= tier2_max_coords.y)
                    {
                        continue; //skip if within tier2
                    }
                    tier3.Add(coords);
                }
            }
        }
    }

    public bool HasCoordsInTile(int x, int y)
    {
        int tile_size = reserve_amount - 1;
        if (tile_size < 0) //no reserve
        {
            return false;
        }
        Vector2Int tier1_min_coords = new Vector2Int(tier1.x - tile_size, tier1.y - tile_size);
        Vector2Int tier1_max_coords = new Vector2Int(tier1.x + tile_size, tier1.y + tile_size);
        if (x >= tier1_min_coords.x && x <= tier1_max_coords.x &&
            y >= tier1_min_coords.y && y <= tier1_max_coords.y)
        {
            return true;
        }
        return false;
    }

    public int GetTierFromCoords(int x, int y)
    {
        if (x == tier1.x && y == tier1.y)
        {
            //return 1;
            return (reserve_amount - 4) * -1; //return tier based on reserve_amount
        }
        foreach (Vector2Int v in tier2)
        {
            if (x == v.x && y == v.y)
            {
                return (reserve_amount - 5) * -1; //return tier based on reserve_amount
            }
        }
        foreach (Vector2Int v in tier3)
        {
            if (x == v.x && y == v.y)
            {
                return 3;
            }
        }
        return 0;
    }
}
