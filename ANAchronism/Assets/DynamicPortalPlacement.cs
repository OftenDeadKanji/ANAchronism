using UnityEngine;

public class DynamicPortalPlacement : MonoBehaviour
{
    [SerializeField]
    VRInputManager vrInputManager;

    [SerializeField] private GameObject leftPortalGameObject;
    [SerializeField] private GameObject rightPortalGameObject;

    void Update()
    {
        CheckLeftController();
        CheckRightController();
    }

    void CheckLeftController()
    {
#if PHYSICAL_VR_DEVICE_ON
		if (vrInputManager.GetLeftControllerPrimaryButtonValue())
#else
        if (Input.GetKeyDown(KeyCode.H))
#endif
        {
            if (Physics.Raycast(vrInputManager.LeftControllerTransform.position,
                    vrInputManager.LeftControllerTransform.forward, out var hit) && hit.collider.CompareTag("Wall"))
            {
                leftPortalGameObject.transform.position = hit.point + hit.normal * 0.01f; // add sth extra to avoid z-fighting
                leftPortalGameObject.transform.rotation = Quaternion.LookRotation(hit.normal);
                
            }
        }
    }

    void CheckRightController()
    {
#if PHYSICAL_VR_DEVICE_ON
		if (vrInputManager.GetRightControllerPrimaryButtonValue())
#else
        if (Input.GetKeyDown(KeyCode.J))
#endif
        {
            if (Physics.Raycast(vrInputManager.RightControllerTransform.position,
                    vrInputManager.RightControllerTransform.forward, out var hit) && hit.collider.CompareTag("Wall"))
            {
                rightPortalGameObject.transform.position = hit.point + hit.normal * 0.01f; // add sth extra to avoid z-fighting
                rightPortalGameObject.transform.rotation = Quaternion.LookRotation(hit.normal);
            }
        }
    }
}
