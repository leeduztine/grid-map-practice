using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Test : MonoBehaviour
{
    private Image img;
    
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    [Button]
    public void Up()
    {
        img.DOFillAmount(1f,1f).SetEase(Ease.InQuart);
    }

    [Button]
    public void Down()
    {
        img.DOFillAmount(0f, 1f).SetEase(Ease.InQuart);
    }
}
