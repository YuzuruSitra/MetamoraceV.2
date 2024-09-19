using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    void Update()
    {
    
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 direction = cameraPosition - transform.position;
            direction.y = 0; // Y軸方向の回転を防ぐ
            transform.rotation = Quaternion.LookRotation(-direction); // 反転を防ぐために方向を反転
    
    }
}