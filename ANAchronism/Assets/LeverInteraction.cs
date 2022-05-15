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

    [SerializeField] VRInputManager vrInputManager;
    public VRInputManager VRInputManager
    {
        set => vrInputManager = value;
    }


    void Start()
    {
        m_OffChild.SetActive(!isActivated);
        m_OnChild.SetActive(isActivated);

        if (vrInputManager == null)
        {
            Debug.LogError("vrInputManager is missing in PlayerMovementController!");
        }
    }

    void Update()
    {
#if PHYSICAL_VR_DEVICE_ON
        if (vrInputManager.GetLeftControllerTriggerValue() > 0.1f)
#else
        if (Input.GetKeyDown(KeyCode.G))
#endif
        {
            if (isInRange)
            {
                RaycastHit hit;
                Debug.DrawRay(vrInputManager.LeftControllerTransform.position, vrInputManager.LeftControllerTransform.forward, Color.black);
                if (Physics.Raycast(vrInputManager.LeftControllerTransform.position, vrInputManager.LeftControllerTransform.forward, out hit, float.PositiveInfinity, LayerMask.GetMask("Default")))
                {
                    if (hit.collider.gameObject == this.m_OnChild || hit.collider.gameObject == this.m_OffChild)
                    {
                        ChangeLeverState();
                    }
                }
                else
                {
                    Debug.Log("Nie");
                }
            }
            
        }
    }

    void ChangeLeverState()
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
