using GridMap;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UI;

public class BaseMap : MonoBehaviour
{
    protected Grid<HeroProfile> grid;

    [SerializeField] protected Text txt;
    [SerializeField] protected Transform gridVisualContainer;

    protected virtual void Start()
    {
        grid.DrawGrid(gridVisualContainer);
    }

    public Grid<HeroProfile> GetGrid()
    {
        return grid;
    }

    public Tile GetMapTile(Vector3 worldPos)
    {
        return grid.WorldPositionToTile(worldPos);
    }

    public virtual void DragIntoMap(HeroDragging hd)
    {
        if (hd == null) return;
        
        Tile selectedTile = grid.WorldPositionToTile(hd.transform.position);

        if (!grid.IsValid(selectedTile))
        {
            // selected outside Map
            hd.MoveToOrigin();
            return;
        }

        DropIntoMap(hd,selectedTile);
    }

    public virtual void DropIntoMap(HeroDragging hd, Tile destination)
    {
        
    }

    public virtual void SwapHero(HeroDragging hd1, HeroDragging hd2)
    {
        Vector3 hd1Pos = hd1.origin;
        Vector3 hd2Pos = hd2.origin;
        var hd1Type = hd1.gridType;
        var hd2Type = hd2.gridType;

        hd1.UpdateOrigin(hd2Pos, hd2Type == GridType.Ground);
        hd1.MoveToOrigin();
        hd2.UpdateOrigin(hd1Pos, hd1Type == GridType.Ground);
        hd2.MoveToOrigin();
    }

    public virtual void SpawnHero(HeroDragging hd, Tile tile)
    {
        
    }

    public int GetNumberOfValue()
    {
        return grid.GetAllValue().Count;
    }

    public void DestroyAll()
    {
        var heroes = grid.GetAllValue();
        
        grid.Clear();
        heroes.ForEach(hero =>
        {
            Destroy(hero.gameObject);
        });
        
        PrintGridArray();
    }

    #region Upimportant methods, just for debugging

    public void PrintGridArray()
    {
        grid.PrintGridArray(txt);
    }

    #endregion
}
