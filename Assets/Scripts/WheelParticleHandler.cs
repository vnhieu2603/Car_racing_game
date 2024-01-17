using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelParticleHandler : MonoBehaviour
{
    float particleEmissionRate = 0;

    TopDownCarController topDownCarController;

    ParticleSystem particleSystemSmoke;
    ParticleSystem.EmissionModule particleSystemEmissionModule;

     void Awake()
	 {
	    topDownCarController = GetComponentInParent<TopDownCarController>();
        
        particleSystemSmoke = GetComponent<ParticleSystem>();

        particleSystemEmissionModule = particleSystemSmoke.emission;

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
        if(topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
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
