using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum InteractMode
{
    kScan = 0,
    kExtract = 1
}

public class InteractiveTilemapController : MonoBehaviour
{
    [SerializeField] private InteractMode mode_ = InteractMode.kExtract;
    [SerializeField] private TileBase highlight_tile_;
    private Grid grid_;
    private Tilemap tilemap_;
    private Vector3Int prev_tile_coord_ = Vector3Int.zero;
    private ResourceManager resource_manager_;

    void Awake()
    {
        grid_ = FindObjectOfType<Grid>();
        tilemap_ = GetComponent<Tilemap>();
        resource_manager_ = FindObjectOfType<ResourceManager>();
    }

    void Update()
    {
        Vector3 mouse_world_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tile_coords = grid_.WorldToCell(mouse_world_pos);

        if (tile_coords != prev_tile_coord_)
        {
            tilemap_.SetTile(prev_tile_coord_, null);
            tilemap_.SetTile(tile_coords, highlight_tile_);
            prev_tile_coord_ = tile_coords;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(tile_coords);
            switch (mode_)
            {
                case InteractMode.kScan:
                    break;
                case InteractMode.kExtract:
                    resource_manager_.TryDepleteResource(tile_coords.x, tile_coords.y);
                    break;
                default:
                    break;
            }
        }
    }
}
