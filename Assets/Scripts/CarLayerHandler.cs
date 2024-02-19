using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLayerHandler : MonoBehaviour
{
    List<SpriteRenderer> defaultLayerSpriteRenderers = new List<SpriteRenderer>();
    List<Collider2D> overpassColliderList = new List<Collider2D>();
	List<Collider2D> underpassColliderList = new List<Collider2D>();

    Collider2D carCollider;

	bool isDrivingOnOverpass = false;
	void Awake()
	{
		foreach (SpriteRenderer spriteRenderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            if(spriteRenderer.sortingLayerName == "Default")
            {
                defaultLayerSpriteRenderers.Add(spriteRenderer);
            }
        }


        foreach (GameObject overpassColliderObject in GameObject.FindGameObjectsWithTag("OverpassCollider"))
        {
            overpassColliderList.Add(overpassColliderObject.GetComponent<Collider2D>());
        }

		foreach (GameObject underpassColliderObject in GameObject.FindGameObjectsWithTag("UnderpassCollider"))
		{
			underpassColliderList.Add(underpassColliderObject.GetComponent<Collider2D>());
		}

        carCollider = GetComponentInChildren<Collider2D>();

        carCollider.gameObject.layer = LayerMask.NameToLayer("ObjectOnUnderpass");
	}

	// Start is called before the first frame update
	void Start()
    {
        UpdateSortingAndCollisionLayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	void UpdateSortingAndCollisionLayers()
    {
        if(isDrivingOnOverpass)
        {
            SetSortingLayer("RacetrackOverpass");
        }else
        {
			SetSortingLayer("Default");
		}

        SetCollisionWithOverpass();
	}
     
    void SetCollisionWithOverpass()
    {
        foreach (Collider2D collider2D in overpassColliderList)
        {
            Physics2D.IgnoreCollision(carCollider, collider2D, !isDrivingOnOverpass);
        }

		foreach (Collider2D collider2D in underpassColliderList)
		{
            if(isDrivingOnOverpass)
            {
				Physics2D.IgnoreCollision(carCollider, collider2D, true);
			} else
            {
				Physics2D.IgnoreCollision(carCollider, collider2D, false);
			}
		}
	}

    void SetSortingLayer(string layerName)
    {
        foreach(SpriteRenderer spriteRenderer in defaultLayerSpriteRenderers)
        {
            spriteRenderer.sortingLayerName = layerName;
        }
    }
    void OnTriggerEnter2D(Collider2D collider2D)
	{
		if(collider2D.CompareTag("UnderpassTrigger")) {
            isDrivingOnOverpass = false;

			carCollider.gameObject.layer = LayerMask.NameToLayer("ObjectOnUnderpass");

			UpdateSortingAndCollisionLayers();
        } else if(collider2D.CompareTag("OverpassTrigger"))
        {
            isDrivingOnOverpass = true;

			carCollider.gameObject.layer = LayerMask.NameToLayer("ObjectOnOverpass");


			UpdateSortingAndCollisionLayers();

		}
	}
}
