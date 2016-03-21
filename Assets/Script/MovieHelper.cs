//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class MovieHelper : MonoBehaviour {
//    public GameObject position_anchor_object;

//    private int mode = 1;
//    private GameObject tracking_space;
//    private GameObject spawn_object;
//    private GameObject movieObject;
//    private VideoXMLWrapper videoData;

//    private List<string> spawn_objectlist;
//    public static List<MarkerXMLWrapper> tracking_object = new List<MarkerXMLWrapper>();

//	void Start () {
//        movieObject = GameObject.Find("GameManager").GetComponent<GameManager>().movieObject;
//        videoData = GameObject.Find("GameManager").GetComponent<GameManager>().videoData;
//        spawn_objectlist = new List<string>();
//        spawn_object = GameObject.Find("SpawnObject");
//        tracking_space = GameObject.Find("TrackingSpace");
//        spawn_object.SetActive(false);
//    }

//    void Update () {
//        if (tracking_object.Count > 0)
//        {
//            NotTrackObject();
//            foreach (MarkerXMLWrapper a in tracking_object)
//            {
//                int current_frame = Time.frameCount;
//                int frame_idx = 0;
//                int prev_frame = int.MaxValue;
//                foreach (MarkerWrapper b in a.TrackList)
//                {
//                    if (prev_frame > (current_frame - b.FrameID))
//                    {
//                        prev_frame = current_frame - b.FrameID;
//                        frame_idx = a.TrackList.IndexOf(b);
//                    }
//                }

//                GameObject prev_object;
//                if ((prev_object = GameObject.Find(a.MarkerID.ToString())) != null)
//                {
//                    prev_object.transform.localPosition = Util.GetVideoPoint(
//                        videoData,
//                        a.TrackList[frame_idx].PositionX,
//                        a.TrackList[frame_idx].PositionY);
//                }
//                else
//                {
//                    spawn_objectlist.Add(a.MarkerID.ToString());
//                    GameObject tmp = Instantiate(position_anchor_object);
//                    tmp.tag = "Finish";
//                    //tmp.AddComponent<CheckRenderObject>();
//                    tmp.name = a.MarkerID.ToString();
//                    tmp.transform.parent = movieObject.transform;
//                    tmp.transform.localScale = new Vector3(1f, 1f, 1f);
//                    tmp.transform.localPosition = Util.GetVideoPoint(
//                        videoData,
//                        a.TrackList[frame_idx].PositionX,
//                        a.TrackList[frame_idx].PositionY);
//                    GameManager.outside_camera_object.Add(a.MarkerID.ToString(), tmp);
//                }
//            }
//        }
//        else
//        {
//            NotTrackObject();
//        }
//	}

//    void NotTrackObject()
//    {
//        foreach (string name in spawn_objectlist)
//        {
//            if (!tracking_object.Exists(item => item.MarkerID.ToString() == name))
//            {
//                Destroy(GameObject.Find(name));
//                GameManager.outside_camera_object.Remove(name);
//            }
//        }
//    }
//}
