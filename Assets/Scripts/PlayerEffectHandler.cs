using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectHandler : MonoBehaviour
{
    [SerializeField] private GameObject Stain;
    private Vector3 _stainPos;
    public void StainWall()
    {
        _stainPos = new Vector3((int)transform.position.x, (int)transform.position.y + 0.25f, -1.0f);
        Instantiate(Stain, _stainPos,Quaternion.identity);
        // Stain
    }

}

