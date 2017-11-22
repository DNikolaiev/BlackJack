using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GUIManager : MonoBehaviour {
	
	public Canvas endGame;
	public Text textCanvas;
	public GameObject doubleHit;
	public GameObject split;
	public GameObject hit;
	public GameObject stand;
	public GameObject[] buttons = new GameObject[4];
	public Text betText;
	public Text playerMoneyText;
	public Canvas insurance;
	public Gambler gambler;
	public static GUIManager instance=null;
	private bool state;
	public Canvas rules;


	void Awake()
	{
		if (instance == null)
		{
			instance=this;
		} 
		else  if (instance!=this)
		{
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		insurance.enabled = false;
		ActivateCanvas (false);
		playerMoneyText.text = Player.instance.dollars.ToString() + " $";
		betText.text = ChipsManager.instance.bet.ToString() + " $";
		rules.enabled = false;

	
	}

	void Update()
	{
		
	}

	void OnEnable()
	{
		
		if (Player.instance.dollars >= ChipsManager.instance.bet)
			doubleHit.SetActive (true);
		else
			doubleHit.SetActive (false);
	}
	public void DisableAllButtons(bool state)
	{
		foreach(GameObject button in buttons)
		{
			button.SetActive (!state);
		}
		split.SetActive (false);
	}

	public void DisableDouble()
	{
		doubleHit.SetActive (false);
	}
	public void ActivateCanvas(bool state)
	{
		doubleHit.SetActive (false);
		split.SetActive (false);
		hit.SetActive (!state);
		stand.SetActive (!state);
		doubleHit.SetActive (!state);
		endGame.enabled = state;
	}

	public void Confirm()
	{
		SceneManager.LoadScene ("overview");

	}

	public void Split()
	{
		if (Player.instance.dollars >= ChipsManager.instance.bet) {
			IncreaseBet (2);
			gambler.Split ();
			split.SetActive (false);
		}
	}

	public void Hit() 
	{
		DeckManager.instance.Hit ();
		split.SetActive (false);
		DisableDouble ();
	}
	public void Stand()
	{
		if (BJManager.instance.p2Score != 0&&!BJManager.instance.wasSplitted) // NOT WORKING PROPERLY IN SPLIT
			CheckScoreInHands ();
		
		if (!BJManager.instance.wasSplitted)
			BJManager.instance.DealerAI ();
		else {
			
			BJManager.instance.SwapCards ();

		}

	}
	public void CheckScoreInHands()
	{
		if (BJManager.instance.playerScore < BJManager.instance.p2Score && BJManager.instance.p2Score <= 21) 
		{
			Debug.Log ("VOSHLO");
			BJManager.instance.playerScore = BJManager.instance.p2Score;


		}
	

	}

	public void Double(GameObject button)
	{
			IncreaseBet (2);
			Hit ();
			Stand ();

	}

	public  void IncreaseBet(int value)
	{

			for (int i = 0; i < value - 1; i++) {
				Player.instance.dollars -= ChipsManager.instance.bet;
			}

			ChipsManager.instance.bet *= value;
		
		playerMoneyText.text = Player.instance.dollars.ToString() + " $";
		betText.text = ChipsManager.instance.bet.ToString() + " $";

	}

	public  void IncreaseBet()
	{
		// NOT WORKING MAYBE THE REASON IS FUCKING FLOATTED BET INSTEAD OF INTEGER ONE
			Player.instance.dollars -= ChipsManager.instance.bet/2;
		ChipsManager.instance.bet += ChipsManager.instance.bet/2 ;

		playerMoneyText.text = Player.instance.dollars.ToString() + " $";
		betText.text = ChipsManager.instance.bet.ToString() + " $";

	}

	public void Cancel()
	{
		DisableAllButtons (false);
		insurance.enabled = false;
		GameObject.FindWithTag ("chip").SetActive (true);

		if (BJManager.instance.split)
			split.SetActive (true);

	}

	public void AllowInsurance()
	{
		BJManager.instance.withoutInsurance = ChipsManager.instance.bet;
		IncreaseBet ();
		BJManager.instance.wasInsured = true;
		Cancel ();
	}

	public void Info()
	{

		rules.enabled = !state;
		state = !state;
	}
}
