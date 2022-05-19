using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ButtonInteraction : MonoBehaviour
{
    [SerializeField] private GameObject m_SphereChild;
    [SerializeField] Material m_ButtonActivateMaterial;
    [SerializeField] Material m_ButtonInactivateMaterial;
    [SerializeField] private bool isActivated = false;
    Renderer m_Renderer;

    [SerializeField] GameObject m_Canvas;

    private bool isInRange = false;

    [SerializeField] VRInputManager vrInputManager;
    public VRInputManager VRInputManager
    {
        set => vrInputManager = value;
    }

    [SerializeField] private List<GameObject> gameObjectToActivate;
    void Start()
    {
        m_Renderer = m_SphereChild.GetComponent<Renderer>();
        if (m_Renderer == null)
        {
            Debug.LogError("Renderer component not found in ButtonInteraction script!");
        }

        m_Renderer.material = isActivated ? m_ButtonActivateMaterial : m_ButtonInactivateMaterial;

        if (vrInputManager == null)
        {
            Debug.LogError("vrInputManager is missing in PlayerMovementController!");
        }

        //gameObjectToActivate = new List<GameObject>();
    }

    void FixedUpdate()
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
                if (Physics.Raycast(vrInputManager.LeftControllerTransform.position, vrInputManager.LeftControllerTransform.forward, out hit, float.PositiveInfinity, LayerMask.GetMask("Default")))
                {
                    if (hit.collider.gameObject == this.m_SphereChild)
                    {
                        ChangeButtonState();
                    }
                }
                else
                {
                    Debug.Log("Nie");
                }
            }

        }
    }
    void ChangeButtonState()
    {
        isActivated = !isActivated;
        m_Renderer.material = isActivated ? m_ButtonActivateMaterial : m_ButtonInactivateMaterial;

        foreach (var go in gameObjectToActivate)
        {
            go.SetActive(isActivated);
        }
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
