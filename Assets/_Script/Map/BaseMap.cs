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

        if (!grid.IsValid(selectedTile) || selectedTile == hd.curTile)
        {
            // not selected Deck or selected current Tile
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
        
    }

    public void SpawnHero(HeroDragging hd, Tile tile)
    {
        grid.SetValue(tile,hd.gameObject.GetComponent<HeroProfile>());
        hd.UpdateOrigin(grid.TileToWorldPosition(tile),true);
        hd.MoveToOrigin();
        
        PrintGridArray();
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
