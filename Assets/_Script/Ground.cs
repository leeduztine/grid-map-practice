using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using GridMap;
using UnityEngine.UI;

public class Ground : MonoBehaviourSingleton<Ground>
{
    private Grid<HeroProfile> grid;

    [SerializeField] private Text txt;

    void Start()
    {
        grid = new Grid<HeroProfile>(new Vector3(0f, 9f, 0f), 5, 5, 10f);
    }

    public Tile SelectedTile()
    {
        return grid.WorldPositionToTile(UtilsClass.GetMouseWorldPosition());
    }

    public void DragIntoGround(HeroDragging hd)
    {
        Tile nullTile = new Tile(-1,-1);
        Tile selectedTile = grid.WorldPositionToTile(UtilsClass.GetMouseWorldPosition());

        if (selectedTile == nullTile)
        {
            hd.MoveToOrigin();
            return;
        }

        if (grid.GetValue(selectedTile) == null)
        {
            // selected tile is empty -> move
            
            grid.SetValue(selectedTile, hd.gameObject.GetComponent<HeroProfile>());
            grid.SetValue(grid.WorldPositionToTile(hd.Origin), null);
            hd.UpdateOrigin(grid.TileToWorldPosition(selectedTile),true);
            hd.MoveToOrigin();
        }
        else
        {
            // selected tile already has a hero -> swap

            var hd2 = grid.GetValue(selectedTile).gameObject.GetComponent<HeroDragging>();
            grid.SwapValue(selectedTile, grid.WorldPositionToTile(hd.Origin));
            SwapHeroPos(hd,hd2);
        }
        
        grid.PrintGridArray(txt);
    }

    public void SwapHeroPos(HeroDragging hd1, HeroDragging hd2)
    {
        Vector3 tmpPos1 = hd1.Origin;
        Vector3 tmpPos2 = hd2.Origin;
        hd1.UpdateOrigin(tmpPos2,true);
        hd1.MoveToOrigin();
        hd2.UpdateOrigin(tmpPos1,true);
        hd2.MoveToOrigin();
    }
}
