using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Transform player;

    private InputDevice leftController;
    [SerializeField] private Transform leftControllerTransform;
    private InputDevice rightController;
    [SerializeField] private Transform rightControllerTransform;

    void Start()
    {
        StartCoroutine(GetDevices(1.0f));
    }

    IEnumerator GetDevices(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);

        Debug.Log("Enumerating XR Devices...");
        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }
        Debug.Log("Finished!");
    }

    void Update()
    {
        float triggerValue = 0.0f;
#if PHYSICAL_VR_DEVICE_ON
        if (rightController.TryGetFeatureValue(CommonUsages.trigger, out triggerValue))
#else
        if (Input.GetKeyDown(KeyCode.F))
#endif
        {
#if PHYSICAL_VR_DEVICE_ON
            if (triggerValue > 0.1f)
#endif
            {
                RaycastHit hit;
                if (Physics.Raycast(rightControllerTransform.position, rightControllerTransform.forward, out hit))
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
                                var rot = Quaternion.Euler(0.0f, portalsRotationDiff, 0.0f) * rightControllerTransform.forward;
                                
                                RaycastHit hit2;
                                if (Physics.Raycast(hit.point + portalsPosDiff, rot, out hit2))
                                {
                                    player.position = new Vector3(hit2.point.x, hit2.point.y, hit2.point.z);
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
}
