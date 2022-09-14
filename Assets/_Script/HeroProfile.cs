using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GridMap;
using UnityEngine;

public class HeroProfile : MonoBehaviour
{
    private int id;

    public int ID
    {
        get => id;
    }
    
    [SerializeField] private HeroState state;

    [SerializeField] private SpriteRenderer sr;

    private float maxHp;
    private float curHp;
    private float atk;
    private float armor;
    private float range;
    private float reload;

    private int tier = 1;

    [SerializeField] private Transform tierContainer;
    [SerializeField] private Transform mainHpBar;
    [SerializeField] private Transform subHpBar;

    public Action<bool,float> OnHpChangeCallback;

    private void OnEnable()
    {
        OnHpChangeCallback += OnHpChange;
    }

    private void OnDisable()
    {
        OnHpChangeCallback -= OnHpChange;
    }

    private void Start()
    {
        id = state.id;
        
        maxHp = state.hp;
        curHp = maxHp;
        atk = state.hp;
        armor = state.armor;
        range = state.range;
        reload = state.reload;

        sr.sprite = state.sprite;
    }
    
    public int GetTier() => tier;

    public void Upgrade()
    {
        if (tier == 3) return;
        tier++;

        int index = 0;
        foreach (Transform child in tierContainer)
        {
            child.gameObject.SetActive(false);
            index++;
            if (index <= tier) child.gameObject.SetActive(true);
        }
    }

    public void ResetTier()
    {
        tier = 1;
        
        int index = 0;
        foreach (Transform child in tierContainer)
        {
            child.gameObject.SetActive(false);
            index++;
            if (index <= tier) child.gameObject.SetActive(true);
        }
    }

    public void TakeDamage(float amount)
    {
        curHp = Mathf.Clamp(curHp - amount, 0f, maxHp);
        OnHpChangeCallback?.Invoke(true,amount);
    }

    public void Heal(float amount)
    {
        curHp = Mathf.Clamp(curHp + amount, 0f, maxHp);
        OnHpChangeCallback?.Invoke(false,amount);
    }

    private void OnHpChange(bool isDamaged, float amount)
    {
        if (isDamaged)
        {
            subHpBar.GetChild(0).GetComponent<SpriteRenderer>().color = Color.yellow;
            DOTween.Kill(mainHpBar);
            mainHpBar.DOScaleX(curHp / maxHp, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                subHpBar.DOScaleX(curHp / maxHp, 0.3f).SetEase(Ease.OutQuad);
            });
        }
        else
        {
            subHpBar.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            DOTween.Kill(subHpBar);
            subHpBar.DOScaleX(curHp / maxHp, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                mainHpBar.DOScaleX(curHp / maxHp, 0.3f).SetEase(Ease.OutQuad);
            });
        }
    }
}
