using UnityEngine;
using UnityEngine.XR;

public class HMDInfoManager : MonoBehaviour
{
    void Start()
    {
        if (!XRSettings.isDeviceActive)
        {
            Debug.Log("No headset plugged");
        }
        else if (XRSettings.loadedDeviceName == "Mock HMD" || XRSettings.loadedDeviceName == "MockHMDDisplay")
        {
            Debug.Log("Using Mock HMD");
        }
        else
        {
            Debug.Log("We have a headset: " + XRSettings.loadedDeviceName);
        }
    }

    void Update()
    {
        
    }
}
