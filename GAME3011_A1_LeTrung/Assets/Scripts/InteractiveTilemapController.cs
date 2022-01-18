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
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tileCoordinate = grid_.WorldToCell(mouseWorldPos);

        if (tileCoordinate != prev_tile_coord_)
        {
            tilemap_.SetTile(prev_tile_coord_, null);
            tilemap_.SetTile(tileCoordinate, highlight_tile_);
            prev_tile_coord_ = tileCoordinate;
        }
    }
}
