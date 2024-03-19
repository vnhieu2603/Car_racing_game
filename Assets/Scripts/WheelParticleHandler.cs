using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelParticleHandler : MonoBehaviour
{
    float particleEmissionRate = 0;

    TopDownCarController topDownCarController;

    ParticleSystem particleSystemSmoke;
    ParticleSystem.EmissionModule particleSystemEmissionModule;
    ParticleSystem.MainModule particleSystenMainModule;
    void Awake()
	 {
	    topDownCarController = GetComponentInParent<TopDownCarController>();
        
        particleSystemSmoke = GetComponent<ParticleSystem>();

        particleSystemEmissionModule = particleSystemSmoke.emission;

        particleSystenMainModule = particleSystemSmoke.main;

        particleSystemEmissionModule.rateOverTime = 0;
	 }

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Reduce the particles over time
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 5);
		particleSystemEmissionModule.rateOverTime = particleEmissionRate;
        switch (topDownCarController.GetSurface())
        {
            case Surface.SurfaceType.Road:
                particleSystenMainModule.startColor = new Color(0.83f, 0.83f, 0.83f);
                break;
            case Surface.SurfaceType.Sand:
                particleEmissionRate = topDownCarController.GetVelocityMagnitude();
                particleSystenMainModule.startColor = new Color(0.64f, 0.42f, 0.24f);
                break;
            case Surface.SurfaceType.Grass:
                particleEmissionRate = topDownCarController.GetVelocityMagnitude();
                particleSystenMainModule.startColor = new Color(0.15f, 0.4f, 0.3f);
                break;
            case Surface.SurfaceType.Oil:
                particleEmissionRate = topDownCarController.GetVelocityMagnitude();
                particleSystenMainModule.startColor = new Color(0.2f, 0.2f, 0.2f);
                break;
        }
        if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
			//if the car tires is screeching then emit smoke. More smoke when player hit brake
			if (isBraking)
            {
                particleEmissionRate = 30;
            } else
            {
                particleEmissionRate = Mathf.Abs(lateralVelocity) * 2;
            }

		}
		
	}
}
