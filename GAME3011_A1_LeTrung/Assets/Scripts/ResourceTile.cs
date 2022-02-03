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

    ResourceTile(int x, int y, Vector2Int min_coords, Vector2Int max_coords)
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
                if ((coords.x != x && coords.y != y) && 
                    coords.x >= min_coords.x && coords.x <= max_coords.x &&
                    coords.y >= min_coords.y && coords.y <= max_coords.y)
                {
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
}
