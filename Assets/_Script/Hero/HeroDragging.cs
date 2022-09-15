using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using DG.Tweening;
using GridMap;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroDragging : MonoBehaviour
{
    private bool isDragging = false;

    public Vector3 origin { get; private set; }
    public GridType gridType { get; private set; }
    public Tile curTile;

    [SerializeField] private Transform graphic;
    private float originalScale = 1f;
    private Vector3 originalRotation = Vector3.zero;
    private Vector3 delta = Vector3.zero;

    private float scaleOnGround = 1f;
    private float scaleOnDeck = 1.25f;

    [SerializeField] private SpriteRenderer[] srElements;

    private void Awake()
    {
        origin = transform.position;
        originalScale = graphic.localScale.x;
        originalRotation = graphic.rotation.eulerAngles;
    }

    private void OnMouseDown()
    {
        delta = UtilsClass.GetMouseWorldPosition() - transform.position;
        isDragging = true;
        
        graphic.transform.DOScale(1.5f * originalScale, 0.2f);
        graphic.transform.DORotate(originalRotation + new Vector3(0f,0f,-30f), 0.2f);
        
        foreach (var sr in srElements)
        {
            sr.sortingOrder += 100;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        graphic.transform.DOScale(0.75f * originalScale, 0.2f).OnComplete(() =>
        {
            graphic.transform.DOScale(originalScale, 0.2f);
        });
        graphic.transform.DORotate(originalRotation, 0.2f);
        
        foreach (var sr in srElements)
        {
            sr.sortingOrder -= 100;
        }
        
        Ground.Instance.DragIntoMap(this);
        Deck.Instance.DragIntoMap(this);
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

    public void UpdateOrigin(Vector3 pos, bool isOnGround)
    {
        origin = pos;

        if (isOnGround)
        {
            transform.localScale = scaleOnGround * Vector3.one;
            gridType = GridType.Ground;
            curTile = Ground.Instance.GetMapTile(origin);
        }
        else
        {
            transform.localScale = scaleOnDeck * Vector3.one;
            gridType = GridType.Deck;
            curTile = Deck.Instance.GetMapTile(origin);
        }
        
        Debug.Log($"{gridType} {curTile.x},{curTile.y}");
    }

    public void MoveToOrigin()
    {
        transform.DOMove(origin, 0.2f);
    }
}
