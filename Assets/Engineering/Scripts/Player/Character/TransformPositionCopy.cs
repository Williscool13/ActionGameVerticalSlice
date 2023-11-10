using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPositionCopy : MonoBehaviour
{
    [SerializeField] Transform target;
    void Update()
    {
        transform.position = target.position;
    }
}
