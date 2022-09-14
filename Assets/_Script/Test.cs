using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using DG.DemiLib;
using UnityEngine;
using GridMap;

public class Test : MonoBehaviour
{
    public Grid<int> arena;
    public Grid<int> deck;
    
    void Start()
    {
        arena = new Grid<int>(new Vector3(0f,9f,0f),5,5,10f);
        deck = new Grid<int>(new Vector3(0f, -29f, 0f), 5, 1, 10f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            arena.SelectXY(UtilsClass.GetMouseWorldPosition());
        }
    }
}
