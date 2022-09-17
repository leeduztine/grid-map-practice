using System;
using System.Collections;
using System.Collections.Generic;
using GridMap;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UI;

public class Deck : BaseMap
{
    private static Deck instance;

    public static Deck Instance
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
        grid = new Grid<HeroProfile>(new Vector3(0f, -27f, 0f), 5, 1, 12.5f);
        base.Start();
    }

    public  void DragIntoDeck(HeroDragging hd)
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
            hd.UpdateOrigin(grid.TileToWorldPosition(selectedTile),false);
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

    public override void DragIntoMap(HeroDragging hd)
    {
        base.DragIntoMap(hd);
    }

    public override void DropIntoMap(HeroDragging hd,Tile destination)
    {
        var source = hd.gridType;
        
        switch (source)
        {
            case GridType.Deck:
                // move Deck -> Deck
                grid.SetValue(grid.WorldPositionToTile(hd.origin), null);
                grid.SetValue(destination, hd.gameObject.GetComponent<HeroProfile>());
                hd.UpdateOrigin(grid.TileToWorldPosition(destination),false);
                hd.MoveToOrigin();
                break;
            
            case GridType.Ground:
                // move Ground -> Deck
                var groundGrid = Ground.Instance.GetGrid();
                groundGrid.SetValue(groundGrid.WorldPositionToTile(hd.origin),null);
                grid.SetValue(destination, hd.gameObject.GetComponent<HeroProfile>());
                hd.UpdateOrigin(grid.TileToWorldPosition(destination),false);
                hd.MoveToOrigin();
                break;
            
            default:
                break;
        }
    }

    public override void SwapHero(HeroDragging hd1, HeroDragging hd2)
    {
        Vector3 tmpPos1 = hd1.origin;
        Vector3 tmpPos2 = hd2.origin;
        hd1.UpdateOrigin(tmpPos2,false);
        hd1.MoveToOrigin();
        hd2.UpdateOrigin(tmpPos1,false);
        hd2.MoveToOrigin();
    }
}
