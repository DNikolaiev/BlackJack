using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

	public List<int> cards;


	public void CreateDeck()
	{

			cards.Clear ();

		for (int i = 0; i < 52; i++) {
			cards.Add (i);
		}

		int n = cards.Count;
		while (n > 1) {
			n--;
			int k = Random.Range (0, n + 1);
			int temp = cards [k];
			cards [k] = cards [n];
			cards [n] = temp; 
		}
	}
		

	public int Pull ()
	{
		int hand = cards [0];
		return hand;
	}



	// Use this for initialization
	void Start () {

		cards = new List<int> ();
		CreateDeck ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
