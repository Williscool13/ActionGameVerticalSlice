using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCopy : MonoBehaviour
{
    [SerializeField] private Transform targetTrans;

    public void SetTarget(Transform tar) {
        targetTrans = tar;
    }
    void Update()
    {
        transform.position = targetTrans.position;
        transform.rotation = targetTrans.rotation;
    }
}
