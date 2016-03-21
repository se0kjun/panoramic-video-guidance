using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OVRArrowHelper : MonoBehaviour {
    private Dictionary<string, GameObject> outsideObject;

    public GameObject floatingArrowObject;
    private List<GameObject> arrowObjects;

    void Start () {
        arrowObjects = new List<GameObject>();
        outsideObject = GameObject.Find("GameManager").GetComponent<GameManager>().outsideCameraObject;
    }
	
	void Update () {
        EdgeObjectArrow();
    }

    private void EdgeObjectArrow()
    {
        RemoveArrow();
        if (outsideObject.Count != 0)
        {
            foreach (string key in outsideObject.Keys)
            {
                GameObject arrowObj;
                if (!(arrowObj = GameObject.Find("arrow" + key)))
                {
                    GameObject arrow = Instantiate(floatingArrowObject);
                    arrow.name = "arrow" + key;
                    arrow.transform.parent = transform;
                    arrow.GetComponent<ArrowPosition>().SetTrackingObject(outsideObject[key]);
                    arrowObjects.Add(arrow);
                }
            }
        }
    }

    private void RemoveArrow()
    {
        GameObject removeComp = null;
        foreach(GameObject obj in arrowObjects)
        {
            string id = obj.name.Split("arrow".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries)[0];
            if (!outsideObject.ContainsKey(id))
            {
                removeComp = obj;
                Destroy(GameObject.Find("arrow" + id));
                break;
            }
        }

        if (removeComp != null)
            arrowObjects.Remove(removeComp);
    }
}
