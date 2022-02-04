using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public enum InteractMode
{
    kScan = 0,
    kExtract = 1
}

public class InteractiveTilemapController : MonoBehaviour
{
    [SerializeField] private InteractMode mode_ = InteractMode.kScan;
    [SerializeField] private Tilemap interact_map_;
    [SerializeField] private TileBase highlight_tile_;
    [SerializeField] private Slider clicks_slider_;
    [SerializeField] private TMP_Text clicks_txt_;
    [SerializeField] private TMP_Text resources_txt_;
    [SerializeField] private TMP_Text interact_mode_btn_txt_;
    private Grid grid_;
    private Vector3Int prev_tile_coord_ = Vector3Int.zero;
    private ResourceManager resource_manager_;
    private int clicks_ = 6;
    private int max_clicks_ = 6;
    private int resources_ = 0;

    void Awake()
    {
        grid_ = FindObjectOfType<Grid>();
        resource_manager_ = FindObjectOfType<ResourceManager>();
        clicks_txt_.text = "Clicks: " + clicks_;
        resources_txt_.text = "Resources: " + resources_;
    }

    void Update()
    {
        Vector3 mouse_world_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tile_coords = grid_.WorldToCell(mouse_world_pos);

        if (tile_coords != prev_tile_coord_)
        {
            interact_map_.SetTile(prev_tile_coord_, null);
            interact_map_.SetTile(tile_coords, highlight_tile_);
            prev_tile_coord_ = tile_coords;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(tile_coords);
            switch (mode_)
            {
                case InteractMode.kScan:
                    if (clicks_ > 0)
                    {
                        bool result = resource_manager_.RevealResourceAtCoords(tile_coords.x, tile_coords.y);
                        if (result)
                        {
                            clicks_--;
                            clicks_txt_.text = "Clicks: " + clicks_;
                            clicks_slider_.value = (float)clicks_ / (float)max_clicks_;
                        }
                    }
                    break;
                case InteractMode.kExtract:
                    int tier = resource_manager_.GetTierAndDepleteResource(tile_coords.x, tile_coords.y);
                    Debug.Log(">>> Extracting tier " + tier.ToString());
                    if (tier == 1)
                    {
                        resources_ += 500;
                    }
                    else if (tier == 2)
                    {
                        resources_ += 250;
                    }
                    else if (tier == 3)
                    {
                        resources_ += 125;
                    }
                    resources_txt_.text = "Resources: " + resources_;
                    break;
                default:
                    break;
            }
        }
    }

    public void ToggleInteractMode()
    {
        mode_ = 1 - mode_;
        switch (mode_)
        {
            case InteractMode.kScan:
                interact_mode_btn_txt_.text = "Scan Mode";
                break;
            case InteractMode.kExtract:
                interact_mode_btn_txt_.text = "Extract Mode";
                break;
            default:
                break;
        }
    }
}
