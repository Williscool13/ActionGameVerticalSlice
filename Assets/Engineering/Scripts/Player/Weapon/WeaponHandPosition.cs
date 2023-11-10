using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandPosition : MonoBehaviour
{
    [SerializeField] private Transform targetTrans;
    void Update()
    {
        transform.position = targetTrans.position;
        transform.rotation = targetTrans.rotation;
    }
}
