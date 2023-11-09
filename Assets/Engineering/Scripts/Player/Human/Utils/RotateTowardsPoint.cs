using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsPoint : MonoBehaviour
{
    [SerializeField] Transform target;
    void Update()
    {
        transform.LookAt(target);        
    }
}
