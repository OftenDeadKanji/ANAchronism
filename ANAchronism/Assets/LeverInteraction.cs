using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LeverInteraction : MonoBehaviour
{
    private bool isActivated = false;
    [SerializeField] private GameObject m_OffChild;
    [SerializeField] private GameObject m_OnChild;

    [SerializeField] GameObject m_Canvas;

    private bool isInRange = false;

    private InputDevice leftController;
    [SerializeField] private Transform leftControllerTransform;

    void Start()
    {
        m_OffChild.SetActive(!isActivated);
        m_OnChild.SetActive(isActivated);
    }

    void Update()
    {
#if PHYSICAL_VR_DEVICE_ON
        if (leftController.TryGetFeatureValue(CommonUsages.trigger, out triggerValue))
#else
        if (Input.GetKeyDown(KeyCode.G))
#endif
        {
#if PHYSICAL_VR_DEVICE_ON
            if (triggerValue > 0.1f)
#endif
            {
                if (isInRange)
                {
                    RaycastHit hit;
                    Debug.DrawRay(leftControllerTransform.position, leftControllerTransform.forward, Color.black);
                    if (Physics.Raycast(leftControllerTransform.position, leftControllerTransform.forward, out hit, float.PositiveInfinity, LayerMask.GetMask("Default")))
                    {
                        if (hit.collider.gameObject == this.m_OnChild || hit.collider.gameObject == this.m_OffChild)
                        {
                            changeLeverState();
                        }
                    }
                    else
                    {
                        Debug.Log("Nie");
                    }
                }
            }
        }
    }

    void changeLeverState()
    {
        isActivated = !isActivated;

        m_OffChild.SetActive(!isActivated);
        m_OnChild.SetActive(isActivated);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            isInRange = true;

            m_Canvas.SetActive(true);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            isInRange = false;

            m_Canvas.SetActive(false);
        }
    }
}
