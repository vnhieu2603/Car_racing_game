using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundEffectHandler : MonoBehaviour
{
    public AudioSource tireScreechingAudioSource;
	public AudioSource engineAudioSource;
	public AudioSource carHitAudioSource;

    float desiredEnginePitch = 0.5f;
    float tireScreechingPitch = 0.5f;

	TopDownCarController topDownCarController;

    void Awake()
	{
	    topDownCarController = GetComponentInParent<TopDownCarController>();	
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngineSFX();
		UpdateTireScreechingSFX();

	}

    void UpdateEngineSFX()
    {
        float velocityMagnitude = topDownCarController.GetVelocityMagnitude();

        //Increase the engine volume as the car goes faster
        float desiredEngineVolume = velocityMagnitude * 0.05f;

		//Keep a minimum level so it plays even if the car is idle
		desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.2f, 1.0f);

        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, desiredEngineVolume, Time.deltaTime * 10);

        //Change the pitch to add more variation to engine sound
        desiredEnginePitch = velocityMagnitude * 0.2f;
		desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2.0f);
        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);

	}

    void UpdateTireScreechingSFX()
    {
        //Handler tire screeching SFX
		if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            //if the car is braking, load the tire screech and change the pitch
            if(isBraking)
            {
                tireScreechingAudioSource.volume = Mathf.Lerp(tireScreechingAudioSource.volume, 1.0f, Time.deltaTime * 10);
                tireScreechingPitch = Mathf.Lerp(tireScreechingPitch, 0.5f, Time.deltaTime * 10);
			} else
            {
                //if car is not braking, still play screech sound if car is drifting
                tireScreechingAudioSource.volume = Mathf.Abs(lateralVelocity) * 0.05f;
				tireScreechingPitch = Mathf.Abs(lateralVelocity) * 0.1f;

			}
		}else //fade out the tire screech SFX
        {
			tireScreechingAudioSource.volume = Mathf.Lerp(tireScreechingAudioSource.volume, 0, Time.deltaTime * 10);

		}

	}
}
