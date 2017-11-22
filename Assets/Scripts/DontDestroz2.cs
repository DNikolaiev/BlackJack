using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroz2 : MonoBehaviour {

	public static DontDestroz2 instance=null;
	void Awake()
	{
		if (instance == null)
		{
			DontDestroyOnLoad (gameObject);
			instance=this;
		} 
		else  if (instance!=this)
		{
			Destroy (gameObject);
		}
	}
}
