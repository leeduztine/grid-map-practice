using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GridMap;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class HeroProfile : MonoBehaviour
{
    private int id;

    public int ID
    {
        get => id;
    }
    
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
    private bool isStunned;
    private bool isSilent;
    private bool isBleeding;
    
    private int tier = 1;

    [SerializeField] private Transform tierContainer;
    [SerializeField] private Image mainHpBar;
    [SerializeField] private Image fxHpBar;

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
        
    }

    public void LoadHeroState(HeroState state, int xp)
    {
        id = state.id;
        sr.sprite = state.icon;
        
        maxHp = state.hp;
        atk = state.hp;
        armor = state.armor;
        range = state.range;
        reload = state.reload;

        curHp = maxHp;

        isStunned = false;
        isSilent = false;
        isBleeding = false;

        curXp = xp;
        tier = 1;
        if (xp >= 30) Upgrade();
        if (xp >= 90) Upgrade();
    }

    public float GetHpPercentage()
    {
        return (float) curHp / maxHp;
    }
    
    public int GetTier() => tier;

    [Button]
    public void GainXp()
    {
        if (tier == 3) return;
        curXp += 10;

        if (curXp == xpRequirement[tier])
        {
            Upgrade();
        }
    }

    [Button]
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

    [Button]
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

    [Button]
    public void TakeDamage(int amount, bool isTrueDmg = false)
    {
        int takenDmg = amount;
        if (!isTrueDmg) takenDmg -= armor;
        if (takenDmg < 1) takenDmg = 1;
        
        curHp -= takenDmg;
        if (curHp < 0) curHp = 0;
        OnHpChangeCallback?.Invoke(true);
    }

    [Button]
    public void Heal(int amount)
    {
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
            fxHpBar.color = Color.yellow;
            DOTween.Kill(mainHpBar);
            mainHpBar.DOFillAmount(GetHpPercentage(), 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                fxHpBar.DOFillAmount(GetHpPercentage(), 0.2f).SetEase(Ease.InQuad);
            });
        }
        else
        {
            fxHpBar.color = Color.green;
            DOTween.Kill(fxHpBar);
            fxHpBar.DOFillAmount(GetHpPercentage(), 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                mainHpBar.DOFillAmount(GetHpPercentage(), 0.2f).SetEase(Ease.InQuad);
            });
        }
    }
}
