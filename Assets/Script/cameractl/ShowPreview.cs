using UnityEngine;
using System.Collections.Generic;

public class PreviewTexture
{
    public GameObject previewObject;
    public GameObject previewCamera;
    public RenderTexture previewRtt;

    public PreviewTexture(GameObject obj, GameObject cam, RenderTexture rtt)
    {
        previewObject = obj;
        previewCamera = cam;
        previewRtt = rtt;
    }
}

public class ShowPreview : MonoBehaviour
{
    public LayerMask previewCameraLayer;

    private Dictionary<string, GameObject> outsideObject;
    private GameObject previewPlane;
    private GameManager managerObject;
    private Camera mainCam;

    private Dictionary<string, PreviewTexture> previewObject;

    //settings of preview object
    private float previewObjectAlpha;
    private float previewObjectSize;
    private float previewObjectDelay;

    void Start()
    {
        managerObject = GameObject.Find("GameManager").GetComponent<GameManager>();
        previewPlane = GameObject.Find("PreviewPlane");
        outsideObject = managerObject.outsideCameraObject;
        previewObject = new Dictionary<string, PreviewTexture>();
        mainCam = Camera.main;
    }

    void Update()
    {
        if (outsideObject.Count != 0)
        {
            foreach (string key in outsideObject.Keys)
            {
                if (!previewObject.ContainsKey(key))
                {
                    RenderTexture previewRtt =
                        new RenderTexture(Screen.width / 4, Screen.height / 4, 16, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
                    previewRtt.filterMode = FilterMode.Point;
                    previewRtt.anisoLevel = 0;
                    previewRtt.antiAliasing = 1;
                    previewRtt.Create();

                    GameObject previewCam = new GameObject();
                    previewCam.AddComponent<Camera>();
                    previewCam.GetComponent<Camera>().targetTexture = previewRtt;
                    previewCam.GetComponent<Camera>().cullingMask = previewCameraLayer;
                    previewCam.transform.LookAt(outsideObject[key].transform.position);

                    GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    plane.transform.parent = previewPlane.transform;
                    plane.GetComponent<Renderer>().material.mainTexture = previewRtt;
                    plane.transform.localPosition = Vector3.zero;
                    plane.transform.localScale = new Vector3(.1f, .1f, .1f);
                    plane.transform.localEulerAngles = Vector3.zero;
                    plane.layer = LayerMask.NameToLayer("PreviewCamera");

                    previewObject[key] = new PreviewTexture(plane, previewCam, previewRtt);
                }
            }

            RemovePreview();
        }
    }

    void RemovePreview()
    {
        List<string> key_list = new List<string>();

        foreach (string key in previewObject.Keys)
        {
            GameObject tmp;
            if (!outsideObject.TryGetValue(key, out tmp))
            {
                Destroy(previewObject[key].previewObject);
                Destroy(previewObject[key].previewCamera);
                key_list.Add(key);
            }
        }

        foreach(string key in key_list)
        {
            previewObject.Remove(key);
        }
    }
}