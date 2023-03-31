using UnityEngine;

public class WebcamTexture : MonoBehaviour
{
    [SerializeField] private int materialIndex = 0;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private bool play;
    private WebCamTexture webCamTexture;
    private Texture initialTexture;

    // Start is called before the first frame update
    void Start()
    {
        Texture.allowThreadedTextureCreation = true;
        webCamTexture = new WebCamTexture();

        if (WebCamTexture.devices.Length > 0)
        {
            AttachWebCamTexture();
        }
    }

    private void AttachWebCamTexture()
    {
        // Get the size of the object's bounding box
        Vector3 objectSize = meshRenderer.bounds.size;

        // Set the size of the webcam texture to match the object's aspect ratio
        if (objectSize.x > objectSize.y) webCamTexture.requestedWidth = (int)(objectSize.x / objectSize.y * webCamTexture.requestedHeight);
        else webCamTexture.requestedHeight = (int)(objectSize.y / objectSize.x * webCamTexture.requestedWidth);

        initialTexture = meshRenderer.materials[materialIndex].GetTexture("_MainTex");

    }

    private void Update()
    {
        if (WebCamTexture.devices.Length > 0)
        {
            if (play && !webCamTexture.isPlaying)
            {
                meshRenderer.materials[materialIndex].SetTexture("_MainTex", webCamTexture);
                webCamTexture.Play();
            }

            if (!play && webCamTexture.isPlaying)
            {
                meshRenderer.materials[materialIndex].SetTexture("_MainTex", initialTexture);
                webCamTexture.Stop();
            }
        }
    }
}
