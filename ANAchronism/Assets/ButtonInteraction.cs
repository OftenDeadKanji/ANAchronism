using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ButtonInteraction : MonoBehaviour
{
    [SerializeField] private GameObject m_SphereChild;
    [SerializeField] Material m_ButtonActivateMaterial;
    [SerializeField] Material m_ButtonInactivateMaterial;
    private bool isActivated = false;
    Renderer m_Renderer;

    [SerializeField] GameObject m_Canvas;

    private bool isInRange = false;

    private InputDevice leftController;
    [SerializeField] private Transform leftControllerTransform;

    void Start()
    {
        m_Renderer = m_SphereChild.GetComponent<Renderer>();
        if (m_Renderer == null)
        {
            Debug.LogError("Renderer component not found in ButtonInteraction script!");
        }

        m_Renderer.material = isActivated ? m_ButtonActivateMaterial : m_ButtonInactivateMaterial;
    }

    void FixedUpdate()
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
                        if (hit.collider.gameObject == this.m_SphereChild)
                        {
                            changeButtonState();
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
    public void changeButtonState()
    {
        isActivated = !isActivated;
        m_Renderer.material = isActivated ? m_ButtonActivateMaterial : m_ButtonInactivateMaterial;
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
