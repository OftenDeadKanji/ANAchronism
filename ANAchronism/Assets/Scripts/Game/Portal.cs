using UnityEngine;

public class Portal : MonoBehaviour
{
	[SerializeField] private Portal connectedPortal;
    public Portal ConnectedPortal
    {
        get => connectedPortal;
    }

    [SerializeField] private GameObject portalPlane;

	void Start()
	{
        

        Debug.Log(portalPlane.transform.position);
        foreach (Transform child in this.transform)
        {
            if (child.CompareTag("PortalCamera"))
            {
                PortalCamera portalCamera = child.GetComponent<PortalCamera>();
                if (portalCamera != null)
                {
                    portalCamera.OwningPortal = this;
                    portalCamera.ConnectedPortal = connectedPortal;
                }

                break;
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

                    portalPlane.GetComponent<MeshRenderer>().material = portalRenderPlaneMaterial;

                    break;
                }
            }
        }
    }
}
