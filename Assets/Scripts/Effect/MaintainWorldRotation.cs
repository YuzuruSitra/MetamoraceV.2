using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainWorldRotation : MonoBehaviour
{
    private Quaternion initialWorldRotation;

    void Start()
    {
        // ワールド座標系での初期回転を記録
        initialWorldRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // ワールド座標系の初期回転を維持
        transform.rotation = initialWorldRotation;
    }
}
