using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class CarSurfaceHandle : MonoBehaviour
{
    [Header("Surface detection")]
    public LayerMask surfaceLayer;
 
    Collider2D carCollider;

    //hit check 
    Collider2D[] surfaceCollidersHit = new Collider2D[10];
    Vector3 lastSampleSurfacePosition = Vector3.one * 1000;

    //Surface Type
    Surface.SurfaceType drivingOnSurfice = Surface.SurfaceType.Road;

  
    public void Awake()
    {
        carCollider = GetComponentInChildren<Collider2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - lastSampleSurfacePosition).sqrMagnitude < 0.75f)
        {
            return;
        }
        ContactFilter2D contactFilter = new ContactFilter2D(); 
        contactFilter.layerMask = surfaceLayer;
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = true;
        int numberOfHit = Physics2D.OverlapCollider(carCollider, contactFilter, surfaceCollidersHit);
        float lastSurfaceZvalue = -1000;
        for(int i = 0; i < numberOfHit; i++)
        {
             Surface surface = surfaceCollidersHit[i].GetComponent<Surface>();
            if(surface.transform.position.z > lastSurfaceZvalue)
            {
                drivingOnSurfice = surface.surfaceType;
                lastSurfaceZvalue = surface.transform.position.z;
            }
        }
        if(numberOfHit == 0)
        {
            drivingOnSurfice = Surface.SurfaceType.Road;
        }
        lastSampleSurfacePosition = transform.position;
        Debug.Log($"Driving on {drivingOnSurfice}");
    }
    public Surface.SurfaceType GetCurrentSurface()
    {
        return drivingOnSurfice;
    }

}
