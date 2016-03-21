using UnityEngine;
using System.Collections;

public class CheckRenderObject : MonoBehaviour {
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnBecameVisible()
    {
        //GameManager.outside_camera_object.Remove(gameObject.name);
        Debug.Log(Camera.current.name);
    }

    void OnWillRenderObject()
    {
        Debug.Log(Camera.current.name);
        //if (Camera.current.name == "CenterEyeAnchor")
        //{
        //    gameManager.outsideCameraObject.Remove(gameObject.name);
        //}
    }
}
