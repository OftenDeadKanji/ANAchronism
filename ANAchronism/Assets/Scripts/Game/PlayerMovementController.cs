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

        obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.position = Vector3.zero;
        obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
    }

    private GameObject obj;
    void Update()
    {
#if PHYSICAL_VR_DEVICE_ON
        if (vrInputManager.GetRightControllerTriggerValue() > 0.1f)
#else
        if (Input.GetKeyDown(KeyCode.F))
#endif
        {
            if (Physics.Raycast(vrInputManager.RightControllerTransform.position, vrInputManager.RightControllerTransform.forward, out var hit))
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

                            var portalsPosDiff = connectedPortal.transform.position - portal.transform.position;

                            var newHitPos = hit.point + portalsPosDiff;
                            

                            // 3. Calculate rot (as euler) diff
                            Vector3 connEuler = connectedPortal.transform.eulerAngles;
                            Vector3 offsetEuler = portal.transform.eulerAngles;

                            Vector3 eulerDiff = offsetEuler - connEuler;
                            
                            var rot = vrInputManager.RightControllerTransform.forward;
                            
                            rot = Quaternion.Euler(eulerDiff) * rot;
                            
                            var pos = portal.transform.InverseTransformPoint(hit.point);
                            pos = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f)) * pos;
                            var newPos = connectedPortal.transform.TransformPoint(pos);

                            //obj.transform.position = newPos;
                            Debug.Log(hit.point);
                            Debug.Log(portalsPosDiff);

                            Debug.DrawLine(newPos, newPos + rot * 2, Color.black, 5);
                            if (Physics.Raycast(newPos, rot, out var hit2))
                            {
                                if (hit2.collider.CompareTag("TeleportationArea"))
                                {
                                    player.position = new Vector3(hit2.point.x, hit2.point.y, hit2.point.z);
                                    player.rotation = Quaternion.Euler(eulerDiff) * player.rotation;
                                }
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
