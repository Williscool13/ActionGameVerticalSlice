using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float offset = 5f;
    void Update()
    {
        transform.position = target.position + Vector3.up * offset;
    }
}
