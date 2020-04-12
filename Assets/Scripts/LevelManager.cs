using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    private void Awake()
    {
        Debug.Log("Fiskens");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Transform[] GetWaypoints()
    {
        return waypoints;
    }
}
