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

    public void DragIntoDeck(HeroDragging hd)
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
            hd.UpdateOrigin(grid.TileToWorldPosition(selectedTile),false);
            hd.MoveToOrigin();
        }
        else
        {
            // selected tile already has a hero -> swap

            var hd2 = grid.GetValue(selectedTile).gameObject.GetComponent<HeroDragging>();
            grid.SwapValue(selectedTile, grid.WorldPositionToTile(hd.Origin));
            SwapHeroPos(hd,hd2);
        }
    }

    public void SwapHeroPos(HeroDragging hd1, HeroDragging hd2)
    {
        Vector3 tmpPos1 = hd1.Origin;
        Vector3 tmpPos2 = hd2.Origin;
        hd1.UpdateOrigin(tmpPos2,false);
        hd1.MoveToOrigin();
        hd2.UpdateOrigin(tmpPos1,false);
        hd2.MoveToOrigin();
    }
}
