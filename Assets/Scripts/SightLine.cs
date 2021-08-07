using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SightLine : MonoBehaviour
{
    public Transform EyePoint;
    public string TargetTag = "Player";
    public float FieldOfView = 45f;

    private bool IsTargetInSightLine { get; set; } = false;
    public Vector3 LastKnownSighting { get; set; } = Vector3.zero;

    private SphereCollider ThisCollider;

    private void Awake()
    {
        ThisCollider = GetComponent<SphereCollider>();
        LastKnownSighting = transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(TargetTag))
        {
            UpdateSight(other.transform);
        }
    }

    private bool HasClearLineOfSightToTarget(Transform target)
    {
        RaycastHit info;
        Vector3 DirToRatget = (target.position - EyePoint.position).normalized;
        if (Physics.Raycast(EyePoint.position, DirToRatget, out info, ThisCollider.radius))
            if (info.transform.CompareTag(TargetTag))
                return true;
        return false;
    }

     private bool TargetInFOV(Transform target)
    {
        Vector3 dirToTarget = target.position - EyePoint.position;
        float angle = Vector3.Angle(EyePoint.forward, dirToTarget);

        if (angle <= FieldOfView)
        {
            return true;
        }

        return false;
    }

    private void UpdateSight(Transform target)
    {
        IsTargetInSightLine = HasClearLineOfSightToTarget(target) && TargetInFOV(target);

        if (IsTargetInSightLine)
            LastKnownSighting = target.position;
    }
}
