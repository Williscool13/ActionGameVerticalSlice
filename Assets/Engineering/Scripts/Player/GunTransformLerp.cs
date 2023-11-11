using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class GunTransformLerp : MonoBehaviour
{
    [SerializeField] Vector3 aimLocalPosition;
    [SerializeField] Transform aimTarget;
    [SerializeField] Vector3 sprintLocalPosition;
    [SerializeField] Vector3 sprintLocalEulerRotation;

    [SerializeField] Rig aimRig;
    [SerializeField] Rig sprintRig;

    [SerializeField] float lerpSpeed = 10f;
    [SerializeField] float rotationSpeed = 30f;
    float targetZRotation;
    private void Start() {
        targetZRotation = transform.rotation.eulerAngles.z;
    }
    void Update()
    {
        if (aiming) {
            /*// Calculate the rotation needed to face the target object in local space
            Vector3 directionToTarget = aimTarget.position - transform.TransformPoint(transform.localPosition);//.TransformPoint(aimLocalPosition);

            // Convert the direction to local space
            Vector3 localDirectionToTarget = transform.InverseTransformDirection(directionToTarget);

            // Calculate the rotation from the local direction
            Quaternion localRotationToTarget = Quaternion.LookRotation(localDirectionToTarget, transform.up);


            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, localRotationToTarget, Time.deltaTime * rotationSpeed);
*/


            Vector3 lookDirection = aimTarget.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, transform.up);
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, targetZRotation);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimLocalPosition, Time.deltaTime * lerpSpeed);

            aimRig.weight = Mathf.Lerp(aimRig.weight, 1f, Time.deltaTime * lerpSpeed);
            sprintRig.weight = Mathf.Lerp(sprintRig.weight, 0f, Time.deltaTime * lerpSpeed);
            return;
        }


        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(sprintLocalEulerRotation), rotationSpeed * Time.deltaTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition, sprintLocalPosition, Time.deltaTime * lerpSpeed);

        aimRig.weight = Mathf.Lerp(aimRig.weight, 0f, Time.deltaTime * lerpSpeed);
        sprintRig.weight = Mathf.Lerp(sprintRig.weight, 1f, Time.deltaTime * lerpSpeed);
        
    }

    
    bool aiming = true;

    [ContextMenu("SetLerpTargetSprint")]
    public void SetLerpTargetSprint() {
        aiming = false;
    }
    [ContextMenu("SetLerpTargetAim")]
    public void SetLerpTargetAim() {
        aiming = true;
    }

    // Custom function to extract correct Euler angles
    private Vector3 ExtractCorrectEulerAngles(Quaternion rotation) {
        // Ensure the pitch (x) angle is in the range [-90, 90] degrees
        float pitch = Mathf.Atan2(2 * (rotation.y * rotation.z + rotation.w * rotation.x), rotation.w * rotation.w - rotation.x * rotation.x - rotation.y * rotation.y + rotation.z * rotation.z) * Mathf.Rad2Deg;

        // Ensure the yaw (y) and roll (z) angles are in the range [-180, 180] degrees
        float yaw = Mathf.Asin(-2 * (rotation.x * rotation.z - rotation.w * rotation.y)) * Mathf.Rad2Deg;
        float roll = Mathf.Atan2(2 * (rotation.x * rotation.y + rotation.w * rotation.z), rotation.w * rotation.w + rotation.x * rotation.x - rotation.y * rotation.y - rotation.z * rotation.z) * Mathf.Rad2Deg;

        return new Vector3(pitch, yaw, roll);
    }
}
