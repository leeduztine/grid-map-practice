using System;
using System.Collections.Generic;
using GridMap;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [TableList] 
    public List<HeroData> heroes;

    public HeroState GetHeroState(int id)
    {
        return heroes.Find(hero => hero.id == id).state;
    }
}

[Serializable]
public class HeroData
{
    public int id;
    public HeroState state;
}