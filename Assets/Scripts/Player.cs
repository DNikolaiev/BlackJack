using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour {
	
	public int dollars;
	public string Name;


	public static Player instance=null;
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
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
