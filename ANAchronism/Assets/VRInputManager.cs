using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRInputManager : MonoBehaviour
{
    #region Left Controller
    private InputDevice leftController;
    public bool GetLeftControllerPrimaryButtonValue()
    {
        if (!isReady)
        {
            return false;
        }

        leftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool value);
        return value;
    }
    public float GetLeftControllerTriggerValue()
    {
        if (!isReady)
        {
            return 0.0f;
        }

        leftController.TryGetFeatureValue(CommonUsages.trigger, out float value);
        return value;
    }
    public Vector2 GetLeftControllerPrimaryAxisValue()
    {
        if (!isReady)
        {
            return Vector2.zero;
        }

        leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 value);
        return value;
    }
    [SerializeField] private Transform leftControllerTransform;

    public Transform LeftControllerTransform
    {
        get => leftControllerTransform;
    }
    #endregion

    #region Right Controller
    private InputDevice rightController;

    public bool GetRightControllerPrimaryButtonValue()
    {
        if (!isReady)
        {
            return false;
        }

        rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool value);
        return value;
    }

    public float GetRightControllerTriggerValue()
    {
        if (!isReady)
        {
            return 0.0f;
        }

        rightController.TryGetFeatureValue(CommonUsages.trigger, out float value);
        return value;
    }

    public Vector2 GetRightControllerPrimaryAxisValue()
    {
        if (!isReady)
        {
            return Vector2.zero;
        }

        rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 value);
        return value;
    }

    [SerializeField] private Transform rightControllerTransform;

    public Transform RightControllerTransform
    {
        get => rightControllerTransform;
    }
    #endregion

    private bool isReady = false;

    void Start()
    {
        StartCoroutine(GetDevices(1.0f));
    }

    IEnumerator GetDevices(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Debug.Log("Enumerating XR Devices...");

        GetLeftControllerDevices();
        GetRightControllerDevices();

        Debug.Log("Finished!");

        isReady = true;
    }

    void GetLeftControllerDevices()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            Debug.Log("Left controller[s]:");
            foreach (var item in devices)
            {
                Debug.Log(item.name + item.characteristics);
            }

            leftController = devices[0];
        }
    }

    void GetRightControllerDevices()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            Debug.Log("Right controller[s]:");
            foreach (var item in devices)
            {
                Debug.Log(item.name + item.characteristics);
            }

            rightController = devices[0];
        }
    }
}
