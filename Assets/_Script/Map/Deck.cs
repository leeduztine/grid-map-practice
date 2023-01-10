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

    public override void DropIntoMap(HeroDragging hd,Tile destination)
    {
        var source = hd.gridType;
        
        switch (source)
        {
            case GridType.Deck:
                if (!grid.GetValue(destination))
                {
                    // move Deck(-) -> Deck(x)
                    grid.SetValue(grid.WorldPositionToTile(hd.origin), null);
                    grid.SetValue(destination, hd.gameObject.GetComponent<HeroProfile>());
                    hd.UpdateOrigin(grid.TileToWorldPosition(destination), false);
                    hd.MoveToOrigin();
                }
                else
                {
                    // swap Deck(x) -> Deck(x)
                    var hd2 = grid.GetValue(destination).gameObject.GetComponent<HeroDragging>();
                    grid.SwapValue(destination, hd.curTile);
                    SwapHero(hd,hd2);
                }
                break;
            
            case GridType.Ground:
                if (!grid.GetValue(destination))
                {
                    // move Ground(-) -> Deck(x)
                    var groundGrid = Ground.Instance.GetGrid();
                    groundGrid.SetValue(groundGrid.WorldPositionToTile(hd.origin), null);
                    grid.SetValue(destination, hd.gameObject.GetComponent<HeroProfile>());
                    hd.UpdateOrigin(grid.TileToWorldPosition(destination), false);
                    hd.MoveToOrigin();
                }
                else
                {
                    // swap Ground(x) -> Deck(x)
                    var hd2 = grid.GetValue(destination).gameObject.GetComponent<HeroDragging>();
                    var groundGrid = Ground.Instance.GetGrid();
                    var tmpData1 = groundGrid.GetValue(hd.curTile);
                    var tmpData2 = grid.GetValue(destination);
                    groundGrid.SetValue(hd.curTile, tmpData2);
                    grid.SetValue(destination, tmpData1);
                    SwapHero(hd,hd2);
                }
                break;
            
            default:
                break;
        }
    }

    public override void SpawnHero(HeroDragging hd, Tile tile)
    {
        grid.SetValue(tile,hd.gameObject.GetComponent<HeroProfile>());
        hd.UpdateOrigin(grid.TileToWorldPosition(tile),false);
        hd.MoveToOrigin();
        
        PrintGridArray();
    }

    public Tile GetAvailableTile()
    {
        Tile tile = new Tile(-1, -1);

        for (int x=0; x<5; x++)
        {
            if (grid.GetValue(new Tile(x, 0)) == null)
            {
                tile = new Tile(x, 0);
                break;
            }
        }
        
        return tile;
    }
}
