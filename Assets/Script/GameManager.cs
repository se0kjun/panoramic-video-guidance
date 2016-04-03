using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public static int renderObjectRadius = 5;

    public string XMLFileName;
    public Camera myCam;

	[HideInInspector]
	public VideoXMLParser parser = VideoXMLParser.Instance;

    public GameObject movieObject;
    private List<MarkerXMLWrapper> objectData;

    [HideInInspector]
    public VideoXMLWrapper videoData;

    public Dictionary<string, GameObject> outsideCameraObject
    {
        get; set;
    }
    private List<MarkerXMLWrapper> spawningObject;

    [Header("Preview Camera Option")]
    [Space(10)]
    public bool previewCameraAvailable;
    [Range(0, 1)]
    public float previewCameraAlpha;
    public bool previewCameraDelay;
    [Range(1, 30)]
    public float previewCameraDelayTime;


    [Header("Arrow Option")]
    [Space(10)]
    public bool arrowAvailable;
    public bool arrowVisible;
    [Range(0, 1)]
    public float arrowAlpha;

    [Space(10)]
    [Header("Move Camera Option")]
    public bool moveCameraAvailable;
    public bool cameraEase;

    void Awake()
    {
        if (XMLFileName != null)
        {
            parser.LoadXML(XMLFileName);
            objectData = parser.ParseMarkerXML();
            videoData = parser.ParseVideoXML();
        }
        outsideCameraObject = new Dictionary<string, GameObject>();
        spawningObject = new List<MarkerXMLWrapper>();
    }

    void Start () {
        MovieTexture tmp = movieObject.GetComponent<Renderer>().material.mainTexture as MovieTexture;
        if (tmp != null)
        {
            tmp.Play();
        }
    }

    void Update () {
        int curr_frame = Mathf.RoundToInt(Time.time * videoData.FPS);

        foreach(MarkerXMLWrapper data in objectData)
        {
            if(data.StartFrame < curr_frame && data.EndFrame > curr_frame)
            {
                spawningObject.Add(data);
            }
        }

        spawningObject.RemoveAll(item => (item.EndFrame < curr_frame));
    }

    public Dictionary<string, GameObject> GetOutsideCameraObject()
    {
        return outsideCameraObject;
    }

    public List<MarkerXMLWrapper> GetSpawnObject()
    {
        return spawningObject;
    }
}
