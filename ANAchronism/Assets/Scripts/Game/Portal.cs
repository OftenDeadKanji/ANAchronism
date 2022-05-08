using UnityEngine;

public class Portal : MonoBehaviour
{

    private PortalCamera portalCamera;
    public PortalCamera PortalCamera
    {
        get => portalCamera;
    }
    private GameObject portalRenderPlane;

    [SerializeField] private Portal connectedPortal;
    public Portal ConnectedPortal
    {
        get => connectedPortal;
    }

    void Start()
	{
        foreach (Transform child in this.transform)
        {
            if (child.CompareTag("PortalCamera"))
            {
                portalCamera = child.GetComponent<PortalCamera>();
                if (portalCamera != null)
                {
                    portalCamera.OwningPortal = this;
                    portalCamera.ConnectedPortal = connectedPortal;
                }
            }
            else if (child.CompareTag("PortalPlane"))
            {
                portalRenderPlane = child.gameObject;
            }
        }

		var portalRenderPlaneMaterial = new Material(Shader.Find("Unlit/ScreenCutoutShader"));
        if (connectedPortal != null)
        {
            foreach (Transform child in connectedPortal.transform)
            {
                if (child.CompareTag("PortalCamera"))
                {
                    Camera cam = child.GetComponent<Camera>();
                    cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
                    portalRenderPlaneMaterial.mainTexture = cam.targetTexture;

                    portalRenderPlane.GetComponent<MeshRenderer>().material = portalRenderPlaneMaterial;

                    break;
                }
            }
        }
    }
}
