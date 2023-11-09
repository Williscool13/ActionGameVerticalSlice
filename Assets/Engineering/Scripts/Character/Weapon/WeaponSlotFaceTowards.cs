using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotFaceTowards : MonoBehaviour
{
    [SerializeField] Transform target;
    void Update()
    {
        transform.LookAt(target);
        /*Ray rayout = cam.ViewportPointToRay(new Vector3(0.5f, 0.45f, 0.0f));
        RaycastHit rhit;
        if (Physics.Raycast(rayout, out rhit, 200.0f, mask)) {
            transform.LookAt(rhit.point, Vector3.up);
        }*/

        Vector3 goal = target.position - transform.position;
        Vector3 current = transform.forward;
        float angle = Vector3.Angle(goal, current);
        float _moveAngle = Mathf.Lerp(0.0f, angle, Time.deltaTime * 5.0f);
        transform.Rotate(Vector3.Cross(goal, current), _moveAngle);
    }
}
