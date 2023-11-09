using UnityEngine;

public class SphereGizmo : MonoBehaviour
{
    [SerializeField] private Color color = Color.red;
    private void OnDrawGizmos() {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
