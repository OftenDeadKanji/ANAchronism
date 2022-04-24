using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    [SerializeField] private Portal owningPortal;
    [SerializeField] private Portal connectedPortal;

    void Start()
    {
        if (gameObject.transform.parent != null)
        {
            owningPortal = gameObject.transform.parent.GetComponent<Portal>();
            if (owningPortal == null)
            {
                Debug.LogError("CameraPortal parent is not a portal!");
            }
            else
            {
                connectedPortal = owningPortal.connectedPortal;
            }
        }
    }

    void Update()
    {
        Vector3 playerOffsetFromPortal = Camera.main.transform.position - connectedPortal.gameObject.transform.position;
        //transform.position = owningPortal.gameObject.transform.position + playerOffsetFromPortal;

        float angularPortalRotationDiff =
            Quaternion.Angle(owningPortal.transform.rotation, connectedPortal.transform.rotation);

        Quaternion portalRotationDiff = Quaternion.AngleAxis(angularPortalRotationDiff, Vector3.up);

        Vector3 newCameraDirection = portalRotationDiff * Camera.main.transform.forward;

        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}
