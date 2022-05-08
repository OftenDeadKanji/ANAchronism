using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    private Portal owningPortal;
    public Portal OwningPortal
    {
        set => owningPortal = value;
    }

    private Portal connectedPortal;
    public Portal ConnectedPortal
    {
        set => connectedPortal = value;
    }

    void Update()
    {
        Vector3 playerOffsetFromPortal = Camera.main.transform.position - connectedPortal.gameObject.transform.position;
        transform.position = owningPortal.gameObject.transform.position + playerOffsetFromPortal;

        float angularPortalRotationDiff = Quaternion.Angle(owningPortal.transform.rotation, connectedPortal.transform.rotation);

        transform.RotateAround(owningPortal.transform.position, Vector3.up, angularPortalRotationDiff + 180.0f);
        transform.rotation = Camera.main.transform.rotation;
    }
}
