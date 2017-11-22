using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: correct Aces ccalculation (aces can be counted as 1 at any moment of the game)
//TODO: Sounds & Music
//TODO: Guidance? Hints?


public class BJManager : MonoBehaviour {
	[HideInInspector]
	public DeckManager deck;
	[HideInInspector]
	public Gambler gambler;
	[HideInInspector]
	public Deck cardcollection;
	[HideInInspector]
	public CardModel card;
	public Chip chip;
	public  int playerScore;
	public int dealerScore;
	public int p2Score;
	public bool dealerTurn;
	public bool playerTurn;
	public bool p2Turn;
	public bool playerWon;
	public bool dealerWon;
	public bool insurancePossibility;
	public bool wasInsured=false;
	private int layerCount=3;
	private float doffset=0.75f*2;
	private bool flag=true;
	public int withoutInsurance;
	private bool wasInsured2;
	public bool blackjack;
	public bool split;
	public bool wasSplitted=false;
	private bool wasSplitted2 = false;


	// Use this for initialization
	public static BJManager instance=null;
	void Awake()
	{
		
		insurancePossibility = false;
		if (instance == null)
		{
			instance=this;
		} 
		else  if (instance!=this)
		{
			Destroy (gameObject);
		}
	}
	void Start () {
		Instantiate (chip, Vector2.zero, Quaternion.identity);
		InitializeGame ();
	}

	// Update is called once per frame
	public void InitializeGame()
	{

		flag = true;
		GUIManager.instance.ActivateCanvas (false);
		playerScore = 0;
		dealerScore = 0;
		p2Score = 0;
		 playerWon=false;
		 dealerWon=false;
		playerTurn = true;
		dealerTurn = false;
		p2Turn = false;
		deck.SpawnDeck ();
		deck.Giveout ();
	}

	public void Insurance()
	{
		
		if (!playerTurn && dealerScore == 21 && playerScore <= 21 && gambler.dealerHand.Count==2) {
			if (wasInsured) {
				Player.instance.dollars += ChipsManager.instance.bet - withoutInsurance / 2;
				wasInsured = false;
				wasInsured2 = true; 
			}
		}
	}



	public void CheckForInsurance()
	{
		if (insurancePossibility&&Player.instance.dollars>=ChipsManager.instance.bet/2&&!blackjack) {
			GUIManager.instance.insurance.enabled = true;
			GameObject.FindWithTag ("chip").SetActive (false);
			GUIManager.instance.DisableAllButtons (true);
			if (split)
				GUIManager.instance.split.SetActive (true);

			 
			}
	}
	void Update () {

		CheckScore ();
		Insurance ();
	}
	public void ChangeTurn()
	{
		if (playerTurn) {
			playerTurn = false;
			dealerTurn = true;
		} else {
			playerTurn = true;
			dealerTurn = false;
		}
	}
	public void AddScore(int score)
	{
		if (playerTurn) {
			playerScore += score;
		} else if (dealerTurn) {
			dealerScore += score;
		} else if (p2Turn)
			p2Score += score;

		
	}

	public void MinusScore(int score)
	{
		if (playerTurn) {
			playerScore -= score;
		} else if (dealerTurn) {
			dealerScore -= score;
		} else if (p2Turn)
			p2Score -= score;
	}
		
	public void CheckScore()
	{
		if (playerScore > 21) {
			if (wasSplitted2||wasSplitted)  
			{
				gambler.playerHand.Clear ();
				gambler.playerHand.AddRange (gambler.playerHand2);

				SwapCards ();
				wasSplitted2 = false;

			} 
			else 
			{
				deck.hiddenCard.TurnFace (true);
				LooseGame ();
			}

		}
		 else if (!playerTurn && !dealerTurn && (playerScore > dealerScore)) 
				WinGame ();
		else if (!playerTurn && !dealerTurn && (playerScore < dealerScore)) {
				LooseGame ();
			}
				
		if (dealerScore > 21)
			WinGame ();
		if (playerScore == 21) {
			deck.hiddenCard.TurnFace (true);
			if (gambler.playerHand.Count==2)
			blackjack = true; 
			WinGame ();
		}
		
	
	}

	public void SwapCards()
	{
		int temp = p2Score;
		p2Score = playerScore;
		playerScore = temp;
		float offset = 0.75f;
		float offset2 = 0f;
		GameObject[] cards = GameObject.FindGameObjectsWithTag ("playerCard");
		GameObject[] secondHand = GameObject.FindGameObjectsWithTag ("secondHand");
		foreach (GameObject element in cards)
		{
			element.transform.position = new Vector2 (5.25f+offset,-5f);
			offset += 0.75f;

		}
		foreach (GameObject element in secondHand) {
			element.transform.position = new Vector2 (offset2, -3f);
			offset2 += 0.75f;
		}
		DeckManager.instance.offset = 0.75f;
		wasSplitted = false;
		wasSplitted2 = true;


	}


	public void DealerAI()
	{ 


		playerTurn = false;
		dealerTurn = true;

		if (playerScore < 21) {
			deck.hiddenCard.FlipCard (card.cardBack, card.faces [deck.hiddenCard.cardIndex], deck.hiddenCard.cardIndex);
			card.cardIndex = cardcollection.cards [0];	
			if ((playerScore > dealerScore) || (playerScore == dealerScore) && dealerScore < 19) {

				deck.AddCard (new Vector2 (doffset, 3), card, "Dealer", true);
			
			} 
			if (playerScore == dealerScore && dealerScore == 18) {
				int randChance=Random.Range (0, 3);
				if (randChance == 1) {

					deck.AddCard (new Vector2 (doffset, 3), card, "Dealer", true);

				} else
					Draw ();
				
			} 
			if (playerScore == dealerScore && (dealerScore == 20||dealerScore==19))
				Draw ();
			doffset += 0.75f;
			if (playerScore >= dealerScore && dealerScore!=20 && dealerScore!=19&&dealerScore!=18) {
				card.GetComponent<SpriteRenderer> ().sortingOrder = layerCount;
				layerCount++;
				DealerAI ();
			} else
				dealerTurn = false;
		}
	}

	public void WinGame()
	{
		
		while (flag) {
			
			playerWon = true;
			EndGame ();
			if (blackjack) {
				GUIManager.instance.textCanvas.text = "BlackJack!\n $" + ChipsManager.instance.bet * 3;
				Player.instance.dollars += ChipsManager.instance.bet * 3;
			} else {
				GUIManager.instance.textCanvas.text = "You have won $" + ChipsManager.instance.bet*2;
				Player.instance.dollars += ChipsManager.instance.bet * 2;
			}
			flag = false;
		}
		
	}

	public void LooseGame()
	{
			if (dealerScore <= 21) {
				dealerWon = true;
				EndGame ();
				GUIManager.instance.textCanvas.text = "You have lost $" + ChipsManager.instance.bet;
				if (wasInsured2) {
					GUIManager.instance.textCanvas.text += "\nBut earned $ " + (ChipsManager.instance.bet - withoutInsurance / 2).ToString () + " back from Insurance";
				}
		}
	}

	public void Draw()
	{
		while (flag) {
			dealerWon = true;
			playerWon = true;
			EndGame ();
			GUIManager.instance.textCanvas.text = "Draw";
			Player.instance.dollars += ChipsManager.instance.bet;
			flag = false;
		}
	}



	private void EndGame ()
	{
		dealerTurn = false;
		playerTurn = false;
		GUIManager.instance.DisableAllButtons (true);
		GUIManager.instance.DisableDouble ();
		GUIManager.instance.ActivateCanvas (true);
		GUIManager.instance.betText.enabled = false;
		Destroy(GameObject.FindGameObjectWithTag("chip"));

	}


}
