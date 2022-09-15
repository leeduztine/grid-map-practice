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

    // states
    private int maxHp;
    private int curHp;
    private int atk;
    private int armor;
    private int range;
    private float reload;

    private int curXp = 10;
    private int[] xpRequirement = new []{0,30,90};

    // buff/debuff
    private bool isStunned = false;
    private bool isSilent = false;
    private bool isBleeding = false;
    
    private int tier = 1;

    [SerializeField] private Transform tierContainer;
    [SerializeField] private Transform mainHpBar;
    [SerializeField] private Transform subHpBar;

    public Action<bool> OnHpChangeCallback;

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
        LoadHeroState();
        curHp = maxHp;
    }

    private void LoadHeroState()
    {
        id = state.id;
        sr.sprite = state.icon;
        
        maxHp = state.hp;
        atk = state.hp;
        armor = state.armor;
        range = state.range;
        reload = state.reload;
    }

    public float GetHpPercentage()
    {
        return (float) curHp / maxHp;
    }
    
    public int GetTier() => tier;

    public void GainXp()
    {
        if (tier == 3) return;
        curXp += 10;

        if (curXp == xpRequirement[tier])
        {
            Upgrade();
        }
    }

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

    public void TakeDamage(int amount/*, bool isTrueDmg = false*/)
    {
        int lastHp = curHp;
        
        int takenDmg = amount;
        /*if (!isTrueDmg)*/ takenDmg -= armor;
        
        curHp -= takenDmg;
        if (curHp < 0) curHp = 0;
        OnHpChangeCallback?.Invoke(true);
    }

    public void Heal(int amount)
    {
        int lastHp = curHp;
        
        int healAmount = amount;
        if (isBleeding) healAmount /= 2;

        curHp += healAmount;
        if (curHp > maxHp) curHp = maxHp;
        OnHpChangeCallback?.Invoke(false);
    }

    private void OnHpChange(bool isDamaged)
    {
        if (isDamaged)
        {
            subHpBar.GetChild(0).GetComponent<SpriteRenderer>().color = Color.yellow;
            DOTween.Kill(mainHpBar);
            mainHpBar.DOScaleX(GetHpPercentage(), 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                subHpBar.DOScaleX(GetHpPercentage(), 0.2f).SetEase(Ease.InQuad);
            });
        }
        else
        {
            subHpBar.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            DOTween.Kill(subHpBar);
            subHpBar.DOScaleX(GetHpPercentage(), 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                mainHpBar.DOScaleX(GetHpPercentage(), 0.2f).SetEase(Ease.InQuad);
            });
        }
    }
}
