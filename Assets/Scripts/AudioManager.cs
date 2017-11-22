using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public AudioClip[] backgroundSounds;
	public AudioSource source1;
	public AudioSource source2;
	public AudioClip currentMusic;
	public float sourcetime;

	public static AudioManager instance=null;
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
		source1.GetComponent<AudioSource> ();
		PlayBackGroundMusic ();

	}

	void PlayBackGroundMusic()
	{
		int randomNumber = Random.Range (0, backgroundSounds.Length);
		source1.clip = backgroundSounds [randomNumber];
		source1.Play ();



	}

	public void PlaySound(AudioClip audio)
	{
		source2.clip = audio;
		source2.Play ();
	}
	// Update is called once per frame
	void Update () {
		sourcetime = source1.time;
		if (!source1.isPlaying&&sourcetime==0) {
			PlayBackGroundMusic ();
		}
	}


}
