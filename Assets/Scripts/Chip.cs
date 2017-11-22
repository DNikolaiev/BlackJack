using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour {

	public Sprite[] chips;
	public int[] values = {1, 10, 50, 100, 500};
	public SpriteRenderer spRend;
	public int chipValue;

	// Use this for initialization
	void Awake()
	{
		spRend=GetComponent<SpriteRenderer> ();
	}
	void Start () {
		
		CreateChip (chipValue);
	}


	public void CreateChip(int chipIndex)
	{
		if (chipIndex==0)
			chipValue = values [0];
		if (chipIndex==1)
			chipValue = values [1];
		if (chipIndex==2)
			chipValue = values [2];
		if (chipIndex==3)
			chipValue = values [3];
		if(chipIndex==4)
			chipValue = values [4];

		spRend.sprite = chips [chipIndex];
		

	}

	// Update is called once per frame
	void Update () {


	}


	//TODO: CALCULATE A BET
	//TODO: spawn several chips with bounded values
}
