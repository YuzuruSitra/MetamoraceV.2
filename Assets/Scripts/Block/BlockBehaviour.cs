using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockBehaviour : MonoBehaviour
{
    [SerializeField]
    public string _objName;
    [SerializeField]
    private float _objHealth;
    private float _maxobjHealth;
    private ItemCBehavior itemCBehavior;

    [SerializeField] private Image _healthGage;

    void Start()
    {
        _maxobjHealth = _objHealth;
        //ItemCBehaviorがついていればゲットコンポーネント}
        itemCBehavior = GetComponent<ItemCBehavior>();
    }

    public string DestroyBlock(float power)
    {
        _objHealth -= power * Time.deltaTime;
        //currentTime = setTime;
        float _nowhealth =  _objHealth/_maxobjHealth;
        _healthGage.fillAmount = _nowhealth;
        if(_objHealth <= 0)
        {   
            if (itemCBehavior != null)
            {
                itemCBehavior.Break4();
            }
            gameObject.SetActive(false);
            return _objName;
        }
       return "NULL";
    }
}
