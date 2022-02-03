using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractiveTilemapController : MonoBehaviour
{
    [SerializeField] private TileBase highlight_tile_;
    private Grid grid_;
    private Tilemap tilemap_;
    private Vector3Int prev_tile_coord_ = Vector3Int.zero;

    void Awake()
    {
        grid_ = FindObjectOfType<Grid>();
        tilemap_ = GetComponent<Tilemap>();
    }

    void Update()
    {
        Vector3 mouse_world_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tile_coord = grid_.WorldToCell(mouse_world_pos);

        if (tile_coord != prev_tile_coord_)
        {
            tilemap_.SetTile(prev_tile_coord_, null);
            tilemap_.SetTile(tile_coord, highlight_tile_);
            prev_tile_coord_ = tile_coord;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(tile_coord);
        }
    }
}
