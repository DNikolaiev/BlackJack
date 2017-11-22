using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour {
	
	public AnimationCurve positionCurve;
	public Deck deck;
	public CardModel card;
	public CardModel fakeDeck;
	public Vector2 deckPos;
	public AudioClip[] cardSounds;
	public float deckoffset=0.025f;
	public float offset=0.75f;


	public CardModel plCard;
	CardModel dCard;
	public BJManager bj;

	public Gambler gambler;
	private int countlayer=2;
	[HideInInspector]
	public CardModel hiddenCard;
	public static DeckManager instance=null;


	// Use this for initialization
	void Awake() {
		deckPos = deck.transform.position;

		if (instance == null)
		{
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


	public void Hit()
	{
		offset += 0.75f;
		card.cardIndex = deck.cards [0];
		card.GetComponent<SpriteRenderer> ().sortingOrder = countlayer;
		plCard = AddCard (new Vector2 (offset, -3), card, "Player",true);
		plCard.tag = "playerCard";



		countlayer++;
	}

	public void SpawnDeck()
	{
		for (int i = 0; i < 48; i++) {
			deckPos.x += deckoffset;
			CardModel fakeCard = Instantiate (fakeDeck, deckPos, Quaternion.identity);


		}
	}

	public void Giveout()
	{
		
		int cardCount=1;
		for (int i = 0; i < 2; i++) {
			

			cardCount++;
			offset += 0.75f*i;
			card.GetComponent<SpriteRenderer> ().sortingOrder = i;

			// player's card
			card.cardIndex=deck.cards[0];
			bj.playerTurn = true;
			plCard=AddCard (new Vector2 (offset, -3f), card, "Player",false);
			plCard.TurnFace (true);
			if (i == 1)
				plCard.tag = "secondHand";
			else
				plCard.tag = "playerCard";


			// dealer's card
			card.cardIndex=deck.cards[0];
			bj.ChangeTurn ();
			dCard=AddCard (new Vector2 (offset, 3f), card, "Dealer",false);
			if (i < 1) {
				dCard.TurnFace (true);
				if (dCard.cardIndex==9||dCard.cardIndex==22||dCard.cardIndex==35||dCard.cardIndex==48 && Player.instance.dollars>=ChipsManager.instance.bet/2)
				bj.insurancePossibility = true;
			}
			else {
				hiddenCard = dCard;
				dCard.TurnFace (false);
			}
			bj.ChangeTurn ();
		}
		if (bj.playerScore == 21)
			bj.blackjack = true;
		bj.split= gambler.CheckForSplit ();
		bj.CheckForInsurance ();
	}

	public CardModel AddCard(Vector2 position, CardModel card,string reciever, bool flip)
	{
		CardModel playerCard = Instantiate (card, deckPos, Quaternion.identity);
		playerCard.transform.position = position;
		if (flip)
		playerCard.FlipCard (card.cardBack, playerCard.faces [playerCard.cardIndex], playerCard.cardIndex);
		int randomNumber = Random.Range (0, cardSounds.Length);
		AudioManager.instance.PlaySound (cardSounds [randomNumber]);
		gambler.Push (card.cardIndex, reciever);
		gambler.CheckCardRank (card.cardIndex);
		Destroy (GameObject.FindWithTag ("fake"));
		return playerCard;
	}



}
