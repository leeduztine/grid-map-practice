using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroDragging : MonoBehaviour
{
    public Test test;

    private bool isDragging = false;
    private Color color;
    private Vector3 origin;
    private float originScale = 7f;
    private Vector3 delta = Vector3.zero;

    private void Start()
    {
        origin = transform.position;
        color = GetComponent<SpriteRenderer>().color;
    }

    private void OnMouseDown()
    {
        delta = UtilsClass.GetMouseWorldPosition() - transform.position;
        
        isDragging = true;
        color = Color.red;
        transform.DOScale(1.25f * originScale, 0.2f);
    }

    private void OnMouseUp()
    {
        isDragging = false;
        color = Color.white;
        transform.DOScale(originScale, 0.2f);

        Grid grid = test.grid;
        Grid.Tile tile = grid.SelectXY(transform.position);

        if (tile != new Grid.Tile(-1,-1))
        {
            origin = grid.GetTilePosition(tile);
        }

        transform.DOMove(origin, 0.2f);
    }

    void Update()
    {
        if (isDragging)
        {
            transform.Translate(UtilsClass.GetMouseWorldPosition() - 
                                (transform.position + delta));
        }
    }
    
}
