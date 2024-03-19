using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    // Start is called before the first frame update
    public enum SurfaceType { Road, Grass, Sand, Oil};

    [Header("Surface")]
    public SurfaceType surfaceType;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
