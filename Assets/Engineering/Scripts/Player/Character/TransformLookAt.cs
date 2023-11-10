using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLookAt : MonoBehaviour
{
    [SerializeField] Transform target;
    void Update()
    {
        transform.LookAt(target);
    }
}
