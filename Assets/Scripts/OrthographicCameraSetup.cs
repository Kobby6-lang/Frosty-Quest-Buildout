using UnityEngine;

public class OrthographicCameraSetup : MonoBehaviour
{
    public GameObject cameraObject;
    public float orthographicSize = 5.0f;
    public float nearClipPlane = 0.1f;
    public float farClipPlane = 1000.0f;


    void Start()
    {
        Camera cam = cameraObject.GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = orthographicSize;
        cam.nearClipPlane = nearClipPlane;
        cam.farClipPlane = farClipPlane;
    }
}

