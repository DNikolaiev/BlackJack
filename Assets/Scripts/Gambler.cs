using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gambler : MonoBehaviour {

	public List<int> playerHand=new List<int>();
	public List<int> dealerHand = new List<int> ();
	public List<int> playerHand2 = new List<int> ();
	public BJManager bjman;
	public Deck deck;
	public CardModel card;
	public int aces=0;
	private bool split;
	private int countlayer=2;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame


	public void Push(int card, string reciever)
	{
		if (reciever == "Player")
			playerHand.Add (card);
		else if (reciever == "Dealer")
			dealerHand.Add (card);
		else if (reciever == "Player2")
			playerHand2.Add (card);


		deck.cards.RemoveAt (0);
	}

	public bool CheckForSplit()
	{
		
		if (playerHand [0] == playerHand [1] + 13 || playerHand [0] == playerHand [1] + 26 || playerHand [0] == playerHand [1] + 39
		    || playerHand [0] == playerHand [1] - 13 || playerHand [0] == playerHand [1] - 26 || playerHand [0] == playerHand [1] - 39) {
				split = true;
		}

		 else 
			split = false;

		CheckAllFaces ();

		if(split)
			GUIManager.instance.split.SetActive (true);

		return split;
	}

	public void CheckAllFaces()
	{
		bool firstCard = false;
		bool secondCard = false;
		int[] faceList = {8, 10, 11, 12, 21,  23, 24, 25, 34, 36, 37, 38, 47, 49, 50, 51 };
		for (int i = 0; i < faceList.Length; i++) {
			if (faceList [i] == playerHand [0])
				firstCard = true;
			else if (faceList [i] == playerHand [1])
				secondCard = true;
		}
		if (firstCard) {
			for (int i = 0; i < faceList.Length; i++) {
				if (playerHand [1] == faceList [i])
					split = true;
			}
		}
			else if (secondCard)
			{
			for (int i = 0; i < faceList.Length; i++) {
				if (playerHand [0] == faceList [i])
					split = true;
			}
		}

	}

	public void Split()
	{
		GUIManager.instance.DisableDouble ();
		int temp = playerHand[1];
		playerHand2.Add (temp);
		playerHand.RemoveAt (1);
		Destroy (GameObject.FindGameObjectWithTag ("secondHand")); 
		DecreaseScore (DeckManager.instance.plCard.cardIndex);
		BJManager.instance.p2Turn = true; BJManager.instance.playerTurn = false;
		DeckManager.instance.AddCard (new Vector2 (6f, -5f), DeckManager.instance.plCard, "Player2", true);


		card.cardIndex=deck.cards[0];
		card.GetComponent<SpriteRenderer> ().sortingOrder = countlayer;
		CardModel p2Card = DeckManager.instance.AddCard (new Vector2 (6+0.75f, -5f), card, "Player2", true);
		p2Card.tag = "secondHand";

		BJManager.instance.p2Turn = false; BJManager.instance.playerTurn = true;
		card.cardIndex=deck.cards[0];
		card.GetComponent<SpriteRenderer> ().sortingOrder = 1;
		CardModel plCard=DeckManager.instance.AddCard (new Vector2 (DeckManager.instance.offset, -3f), card, "Player", true);
		plCard.tag = "playerCard";
		bjman.wasSplitted = true;
	}

	public void CheckCardRank(int card)
	{
		if (card == 0 || card == 13 || card == 26 || card == 39)
			bjman.AddScore (2);
		if (card == 1 || card == 14 || card == 27 || card == 40)
			bjman.AddScore (3);
		if (card == 2 || card == 15 || card == 28 || card == 41)
			bjman.AddScore (4);
		if (card == 3 || card == 16 || card == 29 || card == 42)
			bjman.AddScore (5);
		if (card == 4||  card == 17 || card == 30 || card == 43)
			bjman.AddScore (6);
		if (card == 5 || card == 18 || card == 31 || card == 44)
			bjman.AddScore (7);
		if (card == 6 || card == 19 || card == 32 || card == 45)
			bjman.AddScore (8);
		if (card == 7 || card == 20 || card == 33 || card == 46)
			bjman.AddScore (9);
		if (card == 8 || card == 21 || card == 34 || card == 47)
			bjman.AddScore (10);
		if (card == 9 || card == 22 || card == 35 || card == 48) // ACES
		{	CountAces(); ; }
		if (card == 10 || card == 23 || card == 36 || card == 49)
			bjman.AddScore (10);
		if (card == 11 || card == 24 || card == 37 || card == 50)
			bjman.AddScore (10);
		if (card == 12 || card == 25 || card == 38 || card == 51)
			bjman.AddScore (10);
		
	}

	public void DecreaseScore(int card)
	{
		if (card == 0 || card == 13 || card == 26 || card == 39)
			bjman.MinusScore (2);
		if (card == 1 || card == 14 || card == 27 || card == 40)
			bjman.MinusScore (3);
		if (card == 2 || card == 15 || card == 28 || card == 41)
			bjman.MinusScore (4);
		if (card == 3 || card == 16 || card == 29 || card == 42)
			bjman.MinusScore (5);
		if (card == 4||  card == 17 || card == 30 || card == 43)
			bjman.MinusScore (6);
		if (card == 5 || card == 18 || card == 31 || card == 44)
			bjman.MinusScore (7);
		if (card == 6 || card == 19 || card == 32 || card == 45)
			bjman.MinusScore (8);
		if (card == 7 || card == 20 || card == 33 || card == 46)
			bjman.MinusScore (9);
		if (card == 8 || card == 21 || card == 34 || card == 47)
			bjman.MinusScore (10);
		if (card == 9 || card == 22 || card == 35 || card == 48) // ACES
		{	CountAces(); ; }
		if (card == 10 || card == 23 || card == 36 || card == 49)
			bjman.MinusScore (10);
		if (card == 11 || card == 24 || card == 37 || card == 50)
			bjman.MinusScore (10);
		if (card == 12 || card == 25 || card == 38 || card == 51)
			bjman.MinusScore (10);

	}

	private void CountAces()
	{
		if (bjman.playerTurn) {
			aces++;
			if (bjman.playerScore <= 10)
				bjman.AddScore (11);
			else
				bjman.AddScore (1);
			
			
		} 

		else if (bjman.dealerTurn) {
			if (bjman.dealerScore <= 10)
				bjman.AddScore (11);
			else
				bjman.AddScore (1);
		}
	
	}

//	public void CheckAces(string reciever, List<int> hand)
//	{
//		 hand = new List<int> ();
//		foreach (int element in hand) {
//			if ( reciever=="Player" && (element == 9 || element == 22 || element == 35 || element == 48)&&BJManager.instance.playerScore>21) {
//				BJManager.instance.playerScore -= 10;
//
//			}
//			if (reciever == "Dealer" && (element == 9 || element == 22 || element == 35 || element == 48) && BJManager.instance.dealerScore > 21) {
//				BJManager.instance.dealerScore -= 10;
//			}
//		}
//	}
//		


}
