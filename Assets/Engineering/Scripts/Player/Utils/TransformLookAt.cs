using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLookAt : MonoBehaviour
{
    bool rawLookAt = false;
    [SerializeField] Transform target;
    [SerializeField] float targetZRotation = 0;
    [SerializeField] float rotationSpeed = 20.0f;
    void Update()
    {
        if (rawLookAt) {
            transform.LookAt(target);
            return;
        }
        Vector3 lookDirection = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection, transform.up);
        targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, targetZRotation);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
