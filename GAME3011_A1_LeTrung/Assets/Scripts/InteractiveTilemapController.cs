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
    [SerializeField] private Slider scans_slider_;
    [SerializeField] private TMP_Text scans_txt_;
    [SerializeField] private Slider extracts_slider_;
    [SerializeField] private TMP_Text extracts_txt_;
    [SerializeField] private TMP_Text resources_txt_;
    [SerializeField] private TMP_Text interact_mode_btn_txt_;
    [SerializeField] private TMP_InputField info_txtfield_;
    private Grid grid_;
    private Vector3Int prev_tile_coord_ = Vector3Int.zero;
    private ResourceManager resource_manager_;
    private int scans_ = 6;
    private int max_scans_ = 6;
    private int extracts_ = 3;
    private int max_extracts_ = 3;
    private int resources_ = 0;

    void Awake()
    {
        grid_ = FindObjectOfType<Grid>();
        resource_manager_ = FindObjectOfType<ResourceManager>();
        mode_ = InteractMode.kScan;
        scans_txt_.text = "Scans remaining: " + scans_;
        extracts_txt_.text = "Extracts remaining: " + extracts_;
        resources_txt_.text = "Resources: " + resources_;
        info_txtfield_.interactable = false;
        info_txtfield_.text = "> Press Scan Mode to toggle\nbetween that and Extract Mode.";
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
                    if (scans_ > 0)
                    {
                        bool result = resource_manager_.RevealResourceAtCoords(tile_coords.x, tile_coords.y);
                        if (result)
                        {
                            scans_--;
                            scans_txt_.text = "Scans remaining: " + scans_;
                            scans_slider_.value = (float)scans_ / (float)max_scans_;
                        }
                    }
                    else
                    {
                        info_txtfield_.text = "> No scans remaining!\n" + info_txtfield_.text;
                    }
                    break;
                case InteractMode.kExtract:
                    if (extracts_ > 0)
                    {
                        int tier = resource_manager_.GetTierAndDepleteResource(tile_coords.x, tile_coords.y);
                        Debug.Log(">>> Extracting tier " + tier.ToString());
                        if (tier == 1)
                        {
                            resources_ += 500;
                            info_txtfield_.text = "> Resource gained: 500.\n" + info_txtfield_.text;
                        }
                        else if (tier == 2)
                        {
                            resources_ += 250;
                            info_txtfield_.text = "> Resource gained: 250.\n" + info_txtfield_.text;
                        }
                        else if (tier == 3)
                        {
                            resources_ += 125;
                            info_txtfield_.text = "> Resource gained: 125.\n" + info_txtfield_.text;
                        }

                        if (tier != 0)
                        {
                            extracts_--;
                            extracts_txt_.text = "Scans remaining: " + extracts_;
                            extracts_slider_.value = (float)extracts_ / (float)max_extracts_;
                            resources_txt_.text = "Resources: " + resources_;
                        }
                    }
                    else
                    {
                        info_txtfield_.text = "> No extracts remaining!\n" + info_txtfield_.text;
                    }
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
