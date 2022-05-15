using System.Collections.Generic;
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
        // 1. PortalCamera same as OwningPortal
        transform.position = owningPortal.transform.position;
        transform.rotation = owningPortal.transform.rotation;

        // 2. Calculate vector: player offset from connected portal
        Vector3 playerOffsetFromConnectedPortal = Camera.main.transform.position - connectedPortal.transform.position;

        // 3. Calculate rot (as euler) diff
        Vector3 connEuler = connectedPortal.transform.eulerAngles;
        Vector3 offsetEuler = Quaternion.LookRotation(playerOffsetFromConnectedPortal).eulerAngles;

        Vector3 eulerDiff = offsetEuler - connEuler;

        // 4. Apply the diff
        var rot = this.transform.rotation.eulerAngles;
        rot += eulerDiff;
        this.transform.rotation = Quaternion.Euler(rot);

        // 6. Move camera forward at distance = mag of player offset
        this.transform.position += this.transform.forward * playerOffsetFromConnectedPortal.magnitude;

        // 7. Similary as two prev points
        Vector3 camEuler = Camera.main.transform.eulerAngles;

        Vector3 eulerDiff2 = camEuler - offsetEuler;
        rot = this.transform.rotation.eulerAngles;
        rot += eulerDiff2;
        this.transform.rotation = Quaternion.Euler(rot);

        // 8. Rotate around the portal
        this.transform.RotateAround(owningPortal.transform.position, owningPortal.transform.up, 180.0f);
    }
}
