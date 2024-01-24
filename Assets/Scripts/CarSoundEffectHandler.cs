using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CarSoundEffectHandler : MonoBehaviour
{
	public AudioSource carStartAudioSource;
	public AudioSource tireScreechingAudioSource;
	public AudioSource engineAudioSource;
	public AudioSource carHitAudioSource;

    float desiredEnginePitch = 0.5f;
    float tireScreechingPitch = 0.5f;

    private bool hasPlayed = false;
	TopDownCarController topDownCarController;

    void Awake()
	{
	    topDownCarController = GetComponentInParent<TopDownCarController>();	
	}
	// Start is called before the first frame update
	void Start()
    {
		if (!hasPlayed)
		{
            Debug.Log("da chay vao true");
            carStartAudioSource.volume = 10.0f;

			hasPlayed = true;
		} else
        {
			Debug.Log("da chay vao false");

			carStartAudioSource.volume = 0f;
        }
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
			} else
            {
                tireScreechingAudioSource.volume = Mathf.Abs(lateralVelocity) * 0.05f;	
			}
		}else //fade out the tire screech SFX
        {
			tireScreechingAudioSource.volume = Mathf.Lerp(tireScreechingAudioSource.volume, 0, Time.deltaTime * 10);
		}
	}

    void UpdateCollisionHitSFX(Collision2D collision2D)
    {
        float relativeVelocity = collision2D.relativeVelocity.magnitude;
        float volume = relativeVelocity * 0.1f;

        carHitAudioSource.volume = volume;
        if (!carHitAudioSource.isPlaying)
        {
            carHitAudioSource.Play();
        }
    }
}
