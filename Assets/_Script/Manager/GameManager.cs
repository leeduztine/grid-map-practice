using System;
using System.Collections.Generic;
using GridMap;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [TableList] 
    public List<HeroState> heroes;

    public HeroState GetHeroState(int id)
    {
        return heroes.Find(hero => hero.id == id);
    }

    public HeroState GetRandomHeroState()
    {
        int randId = Random.Range(0, heroes.Count);
        return heroes.Find(hero => hero.id == randId);
    }
}