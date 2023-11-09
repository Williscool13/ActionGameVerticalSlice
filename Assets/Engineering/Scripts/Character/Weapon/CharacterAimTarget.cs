using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAimTarget : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Vector3 offset;
    void Update()
    {
        transform.position = cam.transform.position + (cam.transform.forward + offset) * 200.0f;       
    }
}
