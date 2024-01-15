using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRendererHandler : MonoBehaviour
{
    TopDownCarController topDownCarController;
    TrailRenderer trailRenderer;

	void Awake()
	{
		topDownCarController = GetComponentInParent<TopDownCarController>();

        trailRenderer = GetComponent<TrailRenderer>();

        trailRenderer.emitting = false;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
