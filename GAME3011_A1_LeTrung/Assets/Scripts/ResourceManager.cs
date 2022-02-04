using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private Tilemap ground_map_;
    [SerializeField] private Tilemap resource_map_;
    [SerializeField] private TileBase tier1_tile_;
    [SerializeField] private TileBase tier2_tile_;
    [SerializeField] private TileBase tier3_tile_;
    [SerializeField] private int max_resource_count_ = 10;
    private int resource_count_ = 0;
    private Vector2Int min_coords_; 
    private Vector2Int max_coords_;
    private List<ResourceTile> tile_list_;

    private void Awake()
    {
        ground_map_.CompressBounds();

        Debug.Log(ground_map_.size);
        Debug.Log(ground_map_.cellBounds);
        Debug.Log(ground_map_.cellBounds.xMax);
        Debug.Log(ground_map_.cellBounds.xMin);
        Debug.Log(ground_map_.cellBounds.yMax);
        Debug.Log(ground_map_.cellBounds.yMin);

        min_coords_ = new Vector2Int(ground_map_.cellBounds.xMin, ground_map_.cellBounds.yMin);
        max_coords_ = new Vector2Int(ground_map_.cellBounds.xMax -1, ground_map_.cellBounds.yMax -1);
        tile_list_ = new List<ResourceTile>();

        PopulateResourceMap();
    }

    private void PopulateResourceMap()
    {
        List<Vector2Int> used_tiles = new List<Vector2Int>();
        for (int i = 0; i < max_resource_count_; i++)
        {
            for (int j = 0; j < 10; j++) //try 10 times to find a coord
            {
                int x = Random.Range(min_coords_.x, max_coords_.x + 1); //[minInclusive..maxExclusive)
                int y = Random.Range(min_coords_.y, max_coords_.y + 1); //[minInclusive..maxExclusive)
                if (!AreCoordsInRange(x, y, used_tiles))
                {
                    tile_list_.Add(new ResourceTile(x, y, min_coords_, max_coords_));
                    resource_count_++;
                    used_tiles.Add(new Vector2Int(x, y));
                    break;
                }
            }
        }
        RevealResourceMap();
    }

    public void RevealResourceMap()
    {
        foreach (ResourceTile rt in tile_list_)
        {
            // TIER1
            resource_map_.SetTile(new Vector3Int(rt.tier1.x, rt.tier1.y, 0), tier1_tile_);
            // TIER2
            foreach (Vector2Int v in rt.tier2)
            {
                resource_map_.SetTile(new Vector3Int(v.x, v.y, 0), tier2_tile_);
            }
            // TIER3
            foreach (Vector2Int v in rt.tier3)
            {
                resource_map_.SetTile(new Vector3Int(v.x, v.y, 0), tier3_tile_);
            }
        }
    }

    public bool HasCoordsInList(int x, int y, List<Vector2Int> list)
    {
        Vector2Int vector = new Vector2Int(x, y);
        foreach (Vector2Int v in list)
        {
            if (vector == v)
            {
                return true;
            }
        }
        return false;
    }

    public bool AreCoordsInRange(int x, int y, List<Vector2Int> list)
    {
        Vector2Int new_coords = new Vector2Int(x, y);
        foreach (Vector2Int v in list)
        {
            Vector2Int min_coords = new Vector2Int(v.x - 4, v.y - 4);
            Vector2Int max_coords = new Vector2Int(v.x + 4, v.y + 4);
            if (x >= min_coords.x && x <= max_coords.x &&
                y >= min_coords.y && y <= max_coords.y)
            {
                return true;
            }
        }
        return false;
    }

    public ResourceTile GetTileFromCoords(int x, int y)
    {
        foreach (ResourceTile rt in tile_list_)
        {
            if (rt.HasCoordsInTile(x, y))
            {
                return rt;
            }
        }
        return null;
    }

    public void TryDepleteResource(int x, int y)
    {
        ResourceTile tile = GetTileFromCoords(x, y);
        if (tile != null)
        {
            tile.reserve_amount--;
            if (tile.reserve_amount == 2)
            {
                foreach (Vector2Int v in tile.tier3)
                {
                    resource_map_.SetTile(new Vector3Int(v.x, v.y, 0), null);
                }
                foreach (Vector2Int v in tile.tier2)
                {
                    resource_map_.SetTile(new Vector3Int(v.x, v.y, 0), tier3_tile_);
                }
                resource_map_.SetTile(new Vector3Int(tile.tier1.x, tile.tier1.y, 0), tier2_tile_);

                tile.tier3.Clear();
            }
            else if (tile.reserve_amount == 1)
            {
                foreach (Vector2Int v in tile.tier2)
                {
                    resource_map_.SetTile(new Vector3Int(v.x, v.y, 0), null);
                }
                resource_map_.SetTile(new Vector3Int(tile.tier1.x, tile.tier1.y, 0), tier3_tile_);

                tile.tier2.Clear();
            }
            else if (tile.reserve_amount == 0)
            {
                resource_map_.SetTile(new Vector3Int(tile.tier1.x, tile.tier1.y, 0), null);
            }
        }
    }
}

