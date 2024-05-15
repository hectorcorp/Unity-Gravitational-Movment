using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    Camera Cam;
    //public Transform target;
    public Vector3 MousePosition;
    public GameObject Object;
    public Vector3 SpawnLocation;
    public GameObject SpawnBlackHole;

    void Start()
    {
        Cam = Camera.main;
        SpawnBlackHole = Instantiate(Object,transform.position,Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //screenPos = cam.WorldToScreenPoint(target.position);

        MousePosition = Input.mousePosition;
        SpawnLocation = Cam.ScreenToWorldPoint(MousePosition);
        SpawnLocation[2] = 0f;

        if(Input.GetKey(KeyCode.Space))
        {
            SpawnBlackHole.transform.position = SpawnLocation;
        }
    }

    public GameObject getGameObject()
    {
        return SpawnBlackHole;
    }
}
