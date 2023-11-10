using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAimTarget : MonoBehaviour
{
    [SerializeField] Transform camera;
    [SerializeField] Vector3 offset = new Vector3(0, 1.7f, 0);
    void Update()
    {
        transform.position = camera.position + camera.forward * 10.0f + offset;
        //transform.position = cam.transform.position + (cam.transform.forward + offset) * 200.0f;       
    }
}
