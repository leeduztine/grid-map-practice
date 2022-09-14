using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroDragging : MonoBehaviour
{
    private bool isDragging = false;
    
    private Vector3 origin;

    public Vector3 Origin
    {
        get => origin;
    }
    
    [SerializeField] private Transform graphic;
    private float originalScale = 1f;
    private Vector3 originalRotation = Vector3.zero;
    private Vector3 delta = Vector3.zero;

    private float scaleOnGround = 1f;
    private float scaleOnDeck = 1.25f;

    private void Start()
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
    }

    private void OnMouseUp()
    {
        isDragging = false;
        graphic.transform.DOScale(0.75f * originalScale, 0.2f).OnComplete(() =>
        {
            graphic.transform.DOScale(originalScale, 0.2f);
        });
        graphic.transform.DORotate(originalRotation, 0.2f);
        
        Ground.Instance.DragIntoGround(this);
        Deck.Instance.DragIntoDeck(this);

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

    public void UpdateOriginPosition(Vector3 pos, bool isOnGround)
    {
        origin = pos;

        if (isOnGround) transform.localScale = scaleOnGround * Vector3.one;
        else transform.localScale = scaleOnDeck * Vector3.one;
        
        transform.DOMove(origin, 0.2f);
    }
}
