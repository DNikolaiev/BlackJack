using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class DontDestroy : MonoBehaviour {

	// Use this for initialization
	public static DontDestroy instance=null;
	public GameObject onButton;
	public GameObject offButton;
	public Canvas rules;

	bool state;
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
	
	// Update is called once per frame
	void Update () {
		
		
	}
	void Start()
	{
		onButton.SetActive (true);
		offButton.SetActive (false);
		rules.enabled = false;

	}

	public void Mute()
	{

		AudioManager.instance.source1.volume = 0;
		onButton.SetActive (false);
		offButton.SetActive (true);

	}

	public void UnMute()
	{
		AudioManager.instance.source1.volume = 0.2f;
		offButton.SetActive (false);
		onButton.SetActive (true);

	}

	public void Info()
	{
		
		rules.enabled = !state;
		state = !state;
	}



}
