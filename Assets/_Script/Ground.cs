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
        return grid.SelectXY(UtilsClass.GetMouseWorldPosition());
    }

    public void DragIntoGround(HeroDragging hd)
    {
        if (SelectedTile() == null) return;
        
        if (grid.GetValue(SelectedTile()) == null)
        {
            // destination tile has null data 
            
            grid.SetValue(grid.SelectXY(hd.Origin), null);
            hd.UpdateHeroPosition(grid.GetTilePosition(SelectedTile()), true);
            grid.SetValue(SelectedTile(), hd.gameObject.GetComponent<HeroProfile>());
        }
        else
        {
            // destination tile already has a hero data

            grid.SwapValue(SelectedTile(),grid.SelectXY(hd.Origin));
            var hd2 = grid.GetValue(SelectedTile()).gameObject.GetComponent<HeroDragging>();
            SwapHeroPos(hd,hd2);
        }
        
        grid.PrintGridArray(txt);
    }

    public void SwapHeroPos(HeroDragging hd1, HeroDragging hd2)
    {
        var tmpPos = hd1.Origin;
        hd1.UpdateHeroPosition(hd2.Origin,true);
        hd2.UpdateHeroPosition(tmpPos,true);
    }
}
