using System.Collections;
using System.Collections.Generic;
using GridMap;
using UnityEngine;
using CodeMonkey.Utils;

public class Deck : MonoBehaviourSingleton<Deck>
{
    private Grid<HeroProfile> grid;

    void Start()
    {
        grid = new Grid<HeroProfile>(new Vector3(0f, -27f, 0f), 5, 1, 12.5f);
    }

    public Tile SelectedTile()
    {
        return grid.SelectXY(UtilsClass.GetMouseWorldPosition());
    }
    
    public void DragIntoDeck(HeroDragging hd)
    {
        if (SelectedTile() == null) return;
        
        hd.UpdateOriginPosition(grid.GetTilePosition(SelectedTile()),false);
    }
    
    public void SwapHeroPos(HeroDragging hd1, HeroDragging hd2)
    {
        hd1.UpdateOriginPosition(hd2.Origin,false);
        hd2.UpdateOriginPosition(hd1.Origin,false);
    }
}
