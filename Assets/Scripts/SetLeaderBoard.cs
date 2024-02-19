using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetLeaderBoard : MonoBehaviour
{
	public TMP_Text positionText;
	public TMP_Text driverNameText;


	// Start is called before the first frame update
	void Start()
    {
        
    }

	public void SetPositionText(string position)
	{
		positionText.text = position;
	}

	public void SetDriverNameText(string driverName)
	{
		driverNameText.text = driverName;
	}

}
