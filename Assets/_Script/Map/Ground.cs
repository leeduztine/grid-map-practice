using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using GridMap;
using UnityEngine.UI;

public class Ground : BaseMap
{
    private static Ground instance;

    public static Ground Instance
    {
        get => instance;
    }

    private void Awake()
    {
        if (instance == null) 
            instance = this;
        else if (instance != this) 
            Destroy(gameObject);
    }
    
    protected override void Start()
    {
        grid = new Grid<HeroProfile>(new Vector3(0f, 9f, 0f), 5, 5, 10f);
        base.Start();
    }

    public override void DragIntoMap(HeroDragging hd)
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
            grid.SetValue(grid.WorldPositionToTile(hd.origin), null);
            hd.UpdateOrigin(grid.TileToWorldPosition(selectedTile),true);
            hd.MoveToOrigin();
        }
        else
        {
            // selected tile already has a hero -> swap

            var hd2 = grid.GetValue(selectedTile).gameObject.GetComponent<HeroDragging>();
            grid.SwapValue(selectedTile, grid.WorldPositionToTile(hd.origin));
            SwapHero(hd,hd2);
        }
        
        base.DragIntoMap(hd);
    }

    public override void SwapHero(HeroDragging hd1, HeroDragging hd2)
    {
        Vector3 tmpPos1 = hd1.origin;
        Vector3 tmpPos2 = hd2.origin;
        hd1.UpdateOrigin(tmpPos2,true);
        hd1.MoveToOrigin();
        hd2.UpdateOrigin(tmpPos1,true);
        hd2.MoveToOrigin();
    }
}
