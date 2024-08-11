using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    [SerializeField] private GameObject CreateItemEffect;

    public void LoadCreateItemEffect()
    {
        CreateItemEffect.SetActive(true);
    }
}

