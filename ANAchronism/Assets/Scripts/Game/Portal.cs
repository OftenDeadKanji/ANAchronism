using UnityEngine;

public class Portal : MonoBehaviour
{
	[SerializeField] public Portal connectedPortal;
    [SerializeField] private GameObject portalPlane;

	void Start()
	{
		var portalRenderPlaneMaterial = new Material(Shader.Find("Unlit/ScreenCutoutShader"));
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
