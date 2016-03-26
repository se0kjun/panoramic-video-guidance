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
    private bool previewAvailable;
    private float previewObjectAlpha;
    private bool previewDelay;
    private float previewDelayTime;

    void Start()
    {
        managerObject = GameObject.Find("GameManager").GetComponent<GameManager>();
        previewPlane = GameObject.Find("PreviewPlane");
        previewAvailable = managerObject.previewCameraAvailable;
        previewDelay = managerObject.previewCameraDelay;
        previewDelayTime = managerObject.previewCameraDelayTime;
        outsideObject = managerObject.outsideCameraObject;
        previewObject = new Dictionary<string, PreviewTexture>();
        mainCam = Camera.main;
    }

    void Update()
    {
        previewObjectAlpha = managerObject.previewCameraAlpha;

        if (outsideObject.Count != 0 && previewAvailable)
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

                    Material planeMat = plane.GetComponent<Renderer>().material;
                    planeMat.mainTexture = previewRtt;
                    Color planeColor = planeMat.color;
                    planeColor.a = previewObjectAlpha;
                    planeMat.color = planeColor;
                    planeMat.SetFloat("_Mode", 3);
                    planeMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    planeMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    planeMat.SetInt("_ZWrite", 0);
                    planeMat.DisableKeyword("_ALPHATEST_ON");
                    planeMat.DisableKeyword("_ALPHABLEND_ON");
                    planeMat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    planeMat.renderQueue = 3000;

                    plane.GetComponent<Renderer>().material = planeMat;
                    plane.transform.localScale = new Vector3(.2f, .2f, .2f);
                    plane.transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                    plane.layer = LayerMask.NameToLayer("PreviewCamera");

                    previewObject[key] = new PreviewTexture(plane, previewCam, previewRtt);
                }
                else
                {
                    Vector3 v3Pos = Camera.main.WorldToViewportPoint(outsideObject[key].transform.position);
                    if (v3Pos.x >= 0.0f && v3Pos.x <= 1.0f && v3Pos.y >= 0.0f && v3Pos.y <= 1.0f && v3Pos.z > 0)
                    {
                        previewObject[key].previewObject.transform.GetComponent<Renderer>().enabled = false;
                    }
                    else
                    {
                        previewObject[key].previewObject.transform.GetComponent<Renderer>().enabled = true;
                    }

                    previewObject[key].previewCamera.transform.LookAt(outsideObject[key].transform.position);
                }
            }

            RemovePreview();
            ArrangePreview();
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

    void ArrangePreview()
    {
        Vector3 originPoint = new Vector3(-3.3f, 0, 2f);
        float previewPanelInterval = 0.2f;
        int index = 0;

        foreach (string key in previewObject.Keys)
        {
            float width = previewObject[key].previewObject.transform.GetComponent<Renderer>().bounds.size.y;
            previewObject[key].previewObject.transform.localPosition = 
                new Vector3(originPoint.x + ((width + previewPanelInterval) * index), originPoint.y, originPoint.z );
            index++;
        }
    }

    System.Collections.IEnumerator DelayPreview(GameObject previewObj)
    {
        yield return new WaitForSeconds(previewDelayTime);
        previewObj.GetComponent<Renderer>().enabled = false;
    }
}
