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
    private int resource_count_ = 10;
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
    }

    private void PopulateResourceMap()
    {
        for (int i = 0; i < max_resource_count_; i++)
        {
            int x = Random.Range(min_coords_.x, max_coords_.x + 1); //[minInclusive..maxExclusive)
            int y = Random.Range(min_coords_.y, max_coords_.y + 1); //[minInclusive..maxExclusive)
            //tile_list_.Add()
        }
    }
}

