using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public static int renderObjectRadius = 5;

    public string XMLFileName;
    public Camera myCam;
    public bool arrowVisible;

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
        int curr_frame = Time.frameCount;

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
