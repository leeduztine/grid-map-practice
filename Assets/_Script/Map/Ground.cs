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
    
    public override void DropIntoMap(HeroDragging hd, Tile destination)
    {
        var source = hd.gridType;

        switch (source)
        {
            case GridType.Ground:
                if (!grid.GetValue(destination))
                {
                    // move Ground(-) -> Ground(x)
                    grid.SetValue(grid.WorldPositionToTile(hd.origin), null);
                    grid.SetValue(destination, hd.gameObject.GetComponent<HeroProfile>());
                    hd.UpdateOrigin(grid.TileToWorldPosition(destination), true);
                    hd.MoveToOrigin();
                }
                else
                {
                    // swap Ground(x) -> Ground(x)
                    var hd2 = grid.GetValue(destination).gameObject.GetComponent<HeroDragging>();
                    grid.SwapValue(destination, hd.curTile);
                    SwapHero(hd,hd2);
                }
                break;
            
            case GridType.Deck:
                if (!grid.GetValue(destination))
                {
                    //move Deck(-) -> Ground(x)
                    var deckGrid = Deck.Instance.GetGrid();
                    deckGrid.SetValue(deckGrid.WorldPositionToTile(hd.origin), null);
                    grid.SetValue(destination, hd.gameObject.GetComponent<HeroProfile>());
                    hd.UpdateOrigin(grid.TileToWorldPosition(destination), true);
                    hd.MoveToOrigin();
                }
                else
                {
                    // swap Deck(x) -> Ground(x)
                    var hd2 = grid.GetValue(destination).gameObject.GetComponent<HeroDragging>();
                    var deckGrid = Deck.Instance.GetGrid();
                    var tmpData1 = deckGrid.GetValue(hd.curTile);
                    var tmpData2 = grid.GetValue(destination);
                    deckGrid.SetValue(hd.curTile, tmpData2);
                    grid.SetValue(destination,tmpData1);
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
        hd.UpdateOrigin(grid.TileToWorldPosition(tile),true);
        hd.MoveToOrigin();
        
        PrintGridArray();
    }
}
