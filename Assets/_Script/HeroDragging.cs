using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using GridMap;

public class HeroDragging : MonoBehaviour
{
    public Test test;

    private bool isDragging = false;
    private SpriteRenderer sr;
    private Vector3 origin;
    private float originScale = 1f;
    private Vector3 delta = Vector3.zero;

    private void Start()
    {
        origin = transform.position;
        sr = GetComponent<SpriteRenderer>();
        originScale = transform.localScale.x;
    }

    private void OnMouseDown()
    {
        delta = UtilsClass.GetMouseWorldPosition() - transform.position;
        
        isDragging = true;
        sr.color = Color.red;
        transform.DOScale(1.5f * originScale, 0.2f);
        transform.DORotate(new Vector3(0f, 0f, -30), 0.2f);
    }

    private void OnMouseUp()
    {
        isDragging = false;
        sr.color = Color.white;
        transform.DOScale(originScale, 0.2f);
        transform.DORotate(new Vector3(0f, 0f, 0), 0.2f);

        var grid = test.arena;
        Tile tile = grid.SelectXY(UtilsClass.GetMouseWorldPosition());

        if (tile != null)
        {
            origin = grid.GetTilePosition(tile);
        }

        transform.DOMove(origin, 0.2f);
    }

    void Update()
    {
        if (isDragging)
        {
            // transform.Translate(UtilsClass.GetMouseWorldPosition() - 
            //                     (transform.position + delta));

            transform.position = UtilsClass.GetMouseWorldPosition() - delta;
        }
    }
    
}
