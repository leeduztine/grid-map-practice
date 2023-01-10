using System;
using System.Collections;
using System.Collections.Generic;
using GridMap;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public HeroState[] itmList;

    public Button[] btnList;

    private void Start()
    {
        itmList = new HeroState[3];
        
        Refresh();

        for (int i=0; i<btnList.Length; i++)
        {
            btnList[i].onClick.RemoveAllListeners();

            int buyIdx = i;
            btnList[i].onClick.AddListener(()=>BuyHero(buyIdx));
        }
    }

    [Button]
    public void Refresh()
    {
        for (int i = 0; i < itmList.Length; i++)
        {
            itmList[i] = GameManager.Instance.GetRandomHeroState();
            btnList[i].gameObject.GetComponent<Image>().sprite = itmList[i].img;
        }
    }

    [Button]
    public void BuyHero(int index)
    {
        Tile spawnTile = Deck.Instance.GetAvailableTile();
        if (spawnTile == new Tile(-1, -1))
        {
            Debug.Log("Deck is full");
            return;
        }
        
        var newHero = new Hero();
        var heroData = itmList[index];
        newHero.id = heroData.id;
        newHero.curXp = 10;
        newHero.gridType = GridType.Deck;
        newHero.curTileX = spawnTile.x;
        newHero.curTileY = spawnTile.y;
        
        Debugger.Instance.SpawnHero(newHero, new Vector3(-50f,0f,0f));

        itmList[index] = null;
        btnList[index].gameObject.GetComponent<Image>().sprite = null;
    }
}
