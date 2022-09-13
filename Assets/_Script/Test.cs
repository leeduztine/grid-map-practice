using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using DG.DemiLib;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Grid grid;
    
    void Start()
    {
        grid = new Grid(5, 5, 10f, new Vector3(0f,3f,0f));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SelectXY(UtilsClass.GetMouseWorldPosition());
        }
    }
}
