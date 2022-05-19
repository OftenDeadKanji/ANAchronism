using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovementController : MonoBehaviour
{
	[SerializeField] private Transform player;

	[SerializeField] private VRInputManager vrInputManager;
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
			if (Physics.Raycast(vrInputManager.RightControllerTransform.position, vrInputManager.RightControllerTransform.forward, out var hit))
			{
				if (hit.collider.CompareTag("PortalPlane"))
				{
					var portalParent = hit.collider.gameObject.transform.parent;
					if (portalParent != null)
					{
						var portal = portalParent.GetComponent<Portal>();
						if (portal != null)
						{
							var connectedPortal = portal.ConnectedPortal;
							
							Vector3 connEuler = connectedPortal.transform.eulerAngles;
							Vector3 offsetEuler = portal.transform.eulerAngles;

							Vector3 eulerDiff = offsetEuler - connEuler;
                            eulerDiff.y += 180.0f;
							var rot = vrInputManager.RightControllerTransform.forward;
							rot = Quaternion.Euler(eulerDiff) * rot;
							
							var pos = portal.transform.InverseTransformPoint(hit.point);
							pos = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f)) * pos;
							var newPos = connectedPortal.transform.TransformPoint(pos);

							Debug.DrawRay(newPos + 0.01f * rot, rot, Color.red, 5);

							if (Physics.Raycast(newPos + 0.01f * rot, rot, out var hit2) && hit2.collider.CompareTag("TeleportationArea"))
							{
								player.position = new Vector3(hit2.point.x, hit2.point.y, hit2.point.z);
								player.rotation = Quaternion.Euler(eulerDiff) * player.rotation;
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
