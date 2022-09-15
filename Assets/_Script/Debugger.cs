using System;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using GridMap;
using Sirenix.OdinInspector;
using Unity.Mathematics;

public class Debugger : MonoBehaviour
{
    public GameObject heroPrefab;

    [TableList] public List<Hero> team;

    private void Start()
    {
        
    }

    [Button]
    public void SpawnTeam()
    {
        team.ForEach(hero =>
        {
            GameObject obj = Instantiate(heroPrefab, Vector3.zero, quaternion.identity);
            obj.GetComponent<HeroProfile>().LoadHeroState(GameManager.Instance.GetHeroState(hero.id), hero.curXp);
            if (hero.gridType == GridType.Ground)
            {
                Ground.Instance.SpawnHero(obj.GetComponent<HeroDragging>(),new Tile(hero.curTileX, hero.curTileY));
            }
            else
            {
                Deck.Instance.SpawnHero(obj.GetComponent<HeroDragging>(),new Tile(hero.curTileX, hero.curTileY));   
            }
        });
    }

    [Button]
    public void PrintNumberOfValue()
    {
        Debug.Log($"Ground: {Ground.Instance.GetNumberOfValue()}");
        Debug.Log($"Deck: {Deck.Instance.GetNumberOfValue()}");
    }

    [Button]
    public void DestroyAll()
    {
        Ground.Instance.DestroyAll();
        Deck.Instance.DestroyAll();
    }
}

[Serializable]
public class Hero
{
    public int id;
    public int curXp;
    public GridType gridType;
    public int curTileX;
    public int curTileY;
}

public enum GridType{ Ground, Deck,}