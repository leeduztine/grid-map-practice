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

    public Tile GetMapTile(Vector3 worldPos)
    {
        return grid.WorldPositionToTile(worldPos);
    }

    public virtual void DragIntoMap(HeroDragging hd)
    {
        grid.PrintGridArray(txt);
    }

    public virtual void SwapHero(HeroDragging hd1, HeroDragging hd2)
    {
        grid.PrintGridArray(txt);
    }

    public void SpawnHero(HeroDragging hd, Tile tile)
    {
        grid.SetValue(tile,hd.gameObject.GetComponent<HeroProfile>());
        hd.UpdateOrigin(grid.TileToWorldPosition(tile),true);
        hd.MoveToOrigin();
        
        grid.PrintGridArray(txt);
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
        
        grid.PrintGridArray(txt);
    }
}
