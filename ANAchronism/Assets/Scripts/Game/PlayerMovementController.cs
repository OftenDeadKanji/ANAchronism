using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] VRInputManager vrInputManager;
    public VRInputManager VRInputManager
    {
        set => vrInputManager = value;
    }

    void Awake()
    {
        if (vrInputManager == null)
        {
            Debug.LogError("vrInputManager is missing in PlayerMovementController!");
        }
    }

    void Update()
    {
#if PHYSICAL_VR_DEVICE_ON
        if (vrInputManager.GetRightControllerTriggerValue() > 0.1f)
#else
        if (Input.GetKeyDown(KeyCode.F))
#endif
        {
            RaycastHit hit;
            if (Physics.Raycast(vrInputManager.RightControllerTransform.position, vrInputManager.RightControllerTransform.forward, out hit))
            {
                Debug.Log("Teleport!");

                if (hit.collider.CompareTag("PortalPlane"))
                {
                    var portalParent = hit.collider.gameObject.transform.parent;
                    if (portalParent != null)
                    {
                        var portal = portalParent.GetComponent<Portal>();
                        if (portal != null)
                        {
                            var connectedPortal = portal.ConnectedPortal;

                            var portalsPosDiff = connectedPortal.transform.position - portalParent.position;
                            var portalsRotationDiff = -Quaternion.Angle(connectedPortal.transform.rotation, portalParent.rotation);
                            portalsRotationDiff += 180.0f;
                            var rot = Quaternion.Euler(0.0f, portalsRotationDiff, 0.0f) * vrInputManager.RightControllerTransform.forward;

                            RaycastHit hit2;
                            if (Physics.Raycast(hit.point + portalsPosDiff, rot, out hit2))
                            {
                                player.position = new Vector3(hit2.point.x, hit2.point.y, hit2.point.z);
                            }
                            else
                            {
                                Debug.Log("Nie ma miejsca.");
                            }
                        }
                    }
                }
                else if (hit.collider.CompareTag("TeleportationArea"))
                {
                    player.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                }
            }

        }
    }
}
