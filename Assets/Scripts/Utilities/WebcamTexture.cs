using UnityEngine;

public class WebcamTexture : MonoBehaviour
{
    [SerializeField] private int materialIndex = 0;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private bool play;
    private WebCamTexture webcamTexture;
    private Texture initialTexture;

    // Start is called before the first frame update
    void Start()
    {
        Texture.allowThreadedTextureCreation = true;
        webcamTexture = new WebCamTexture();

        if (WebCamTexture.devices.Length > 0)
        {
            AttachWebCamTexture();
        }

        if (WebCamTexture.devices.Length > 0)
        {
            if (play && !webcamTexture.isPlaying)
            {
                meshRenderer.materials[materialIndex].SetTexture("_MainTex", webcamTexture);
                webcamTexture.Play();
            }

            if (!play && webcamTexture.isPlaying)
            {
                meshRenderer.materials[materialIndex].SetTexture("_MainTex", initialTexture);
                webcamTexture.Stop();
            }
        }
    }

    private void AttachWebCamTexture()
    {
        // Get the size of the object's bounding box
        Vector3 objectSize = meshRenderer.bounds.size;

        // Set the size of the webcam texture to match the object's aspect ratio
        if (objectSize.x > objectSize.y) webcamTexture.requestedWidth = (int)(objectSize.x / objectSize.y * webcamTexture.requestedHeight);
        else webcamTexture.requestedHeight = (int)(objectSize.y / objectSize.x * webcamTexture.requestedWidth);

        initialTexture = meshRenderer.materials[materialIndex].GetTexture("_MainTex");

    }
}
