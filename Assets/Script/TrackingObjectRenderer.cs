using UnityEngine;
using System.Collections.Generic;

public class TrackingObjectRenderer : MonoBehaviour {
    public GameObject positionAnchorObject;

    private GameManager gameManager;
    private List<MarkerXMLWrapper> spawningObject;
    private GameObject movieObject;
    private List<string> spawnObjectList;
    public VideoXMLWrapper videoData;

    void Start () {
        movieObject = GameObject.Find("GameManager").GetComponent<GameManager>().movieObject;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawnObjectList = new List<string>();
        videoData = gameManager.videoData;
    }
	
	void Update () {
        spawningObject = gameManager.GetSpawnObject();
        NotTrackObject();

        foreach (MarkerXMLWrapper obj in spawningObject)
        {
            int current_frame = Mathf.RoundToInt(Time.time * videoData.FPS);
            int frame_idx = 0;
            int prev_frame = int.MaxValue;
            foreach (MarkerWrapper b in obj.TrackList)
            {
                if (prev_frame > (current_frame - b.FrameID) && (current_frame - b.FrameID) >= 0)
                {
                    prev_frame = current_frame - b.FrameID;
                    frame_idx = obj.TrackList.IndexOf(b);
                }
            }

            GameObject prev_object;
            // when object exists
            if ((prev_object = GameObject.Find(obj.MarkerID.ToString())) != null)
            {
                Vector2 uv = Util.GetUV(videoData, obj.TrackList[frame_idx].PositionX, obj.TrackList[frame_idx].PositionY);
                Vector3 uvMap3D = Util.UvTo3D(uv, movieObject);
                float angle = Vector3.Angle(Vector3.zero, uvMap3D);
                prev_object.transform.localPosition = Vector3.MoveTowards(uvMap3D, Vector3.zero, 1.0f);
                prev_object.transform.localEulerAngles = new Vector3(180 - (angle + 90), 0.0f, -90.0f);
                if (gameManager.outsideCameraObject.ContainsKey(obj.MarkerID.ToString()))
                {
                    gameManager.outsideCameraObject[obj.MarkerID.ToString()].transform.localPosition = Vector3.MoveTowards(uvMap3D, Vector3.zero, 1.0f);
                }
                else
                {
                    GameObject originalObj = prev_object;
                    originalObj.transform.localPosition = uvMap3D;

                    gameManager.outsideCameraObject.Add(obj.MarkerID.ToString(), prev_object);
                }
            }
            else
            {
                // when object doesn't exist
                spawnObjectList.Add(obj.MarkerID.ToString());

                // spawn object
                GameObject tmp = Instantiate(positionAnchorObject);
                tmp.tag = "Finish";
                tmp.AddComponent<CheckRenderObject>();
                tmp.name = obj.MarkerID.ToString();
                tmp.transform.parent = movieObject.transform;
                Vector2 uv = Util.GetUV(videoData, obj.TrackList[frame_idx].PositionX, obj.TrackList[frame_idx].PositionY);
                Vector3 objpos = Util.UvTo3D(uv, movieObject);
                float angle = Vector3.Angle(Vector3.zero, objpos);
                tmp.transform.localPosition = Vector3.MoveTowards(objpos, Vector3.zero, 1.0f);
                tmp.transform.localEulerAngles = new Vector3(180 - (angle + 90), 0.0f, -90.0f);

                GameObject originalObj = tmp;
                originalObj.transform.localPosition = objpos;

                gameManager.outsideCameraObject.Add(obj.MarkerID.ToString(), originalObj);
            }
        }
    }

    void NotTrackObject()
    {
        if (spawnObjectList != null)
        {
            foreach (string name in spawnObjectList)
            {
                if (!spawningObject.Exists(item => item.MarkerID.ToString() == name))
                {
                    Destroy(GameObject.Find(name));
                    gameManager.outsideCameraObject.Remove(name);
                }
            }
        }
    }
}
