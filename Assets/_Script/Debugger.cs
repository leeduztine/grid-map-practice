using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using GridMap;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Debugger : MonoBehaviourSingleton<Debugger>
{
    public GameObject heroPrefab;

    [TableList] public List<Hero> team;

    public InputField terminal;
    public Button checkNullBtn;

    private void Start()
    {
        StartCoroutine(SpawnTeamAfter(1f));
        
        checkNullBtn.onClick.RemoveAllListeners();
        checkNullBtn.onClick.AddListener(CheckNull);
    }

    IEnumerator SpawnTeamAfter(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnTeam();
    }

    public bool autoSpawnTeam = false;

    [Button]
    public void SpawnTeam()
    {
        team.ForEach(hero =>
        {
            GameObject obj = Instantiate(heroPrefab, Vector3.zero, quaternion.identity);
            obj.name = GameManager.Instance.GetHeroState(hero.id).heroName;
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

    public void SpawnHero(Hero hr, Vector3 origin)
    {
        GameObject obj = Instantiate(heroPrefab, origin, quaternion.identity);
        obj.name = GameManager.Instance.GetHeroState(hr.id).heroName;
        obj.GetComponent<HeroProfile>().LoadHeroState(GameManager.Instance.GetHeroState(hr.id), hr.curXp);
        if (hr.gridType == GridType.Ground)
        {
            Ground.Instance.SpawnHero(obj.GetComponent<HeroDragging>(),new Tile(hr.curTileX, hr.curTileY));
        }
        else
        {
            Deck.Instance.SpawnHero(obj.GetComponent<HeroDragging>(),new Tile(hr.curTileX, hr.curTileY));   
        }
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

    [Button]
    public void CheckNull()
    {
        string msg = terminal.text;
        string[] msgs = msg.Split(" ");

        int x = int.Parse(msgs[1]);
        int y = int.Parse(msgs[2]);
        Tile t = new Tile(x, y);
        if (msgs[0] == "d")
        {
            Debug.Log(Deck.Instance.GetGrid().GetValue(t));
        }
        
        if (msgs[0] == "g")
        {
            Debug.Log(Ground.Instance.GetGrid().GetValue(t));
        }
    }

    public void PrintGridArray()
    {
        Ground.Instance.PrintGridArray();
        Deck.Instance.PrintGridArray();
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