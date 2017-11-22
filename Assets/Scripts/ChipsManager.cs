using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChipsManager : MonoBehaviour {
	public Chip chip;
	public Text betText;
	public Text playerMoneytext;
	public int bet;
	public GameObject[] buttons;
	public Canvas canvas;
	public GameObject dealButton;
	public AudioClip[] chipSounds;
	// Use this for initialization
	public static ChipsManager instance=null;
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
	void Start () {
		
		bet = 0;


	}

	private void ControleChips()
	{
		for (int i=0; i<buttons.Length; i++) {
			if (Player.instance.dollars < chip.values [i + 1]) {
				buttons [i].SetActive (false);
			} else if (Player.instance.dollars >= chip.values [i + 1])
				buttons [i].SetActive (true);
				
		}
		if (bet == 0)
			dealButton.SetActive (false);
		else
			dealButton.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		if (Application.loadedLevelName == "overview") {
			

			ControleChips ();
			betText.text = bet.ToString () + " $";
			playerMoneytext.text = Player.instance.dollars + " $";
			canvas.enabled = true;
		} else if (Application.loadedLevelName == "main")
			canvas.enabled = false;
		else if (Application.loadedLevelName == "menu")
			canvas.enabled = false;
		}




	public void SetBid(GameObject button)
	{
		string name = button.name;
		chip.CreateChip (int.Parse(name));
		Chip chipToSet = Instantiate (chip, Vector2.zero, Quaternion.identity);
		bet += chipToSet.chipValue;
		Player.instance.dollars -= chipToSet.chipValue;
		int randomNumber = Random.Range (0, chipSounds.Length);
		AudioManager.instance.PlaySound (chipSounds [randomNumber]);
	}

	public void Deal()
	{
		AudioManager.instance.PlaySound (MenuController.instance.hitSound);
		SceneManager.LoadScene ("main");
	}

	public void AllIn()
	{
		if (Player.instance.dollars >= 10) {
			Player.instance.dollars += bet;
			int temp = Player.instance.dollars;
			Player.instance.dollars = 0;
			bet = temp;
			Chip chipToSet = Instantiate (chip, Vector2.zero, Quaternion.identity);
		}
		AudioManager.instance.PlaySound (chipSounds [0]);
	}


	public void Reset()
	{
		AudioManager.instance.PlaySound (MenuController.instance.hitSound);
		int temp = bet;
		bet = 0;
		Player.instance.dollars+= temp;
		GameObject[] chipsOnTable = GameObject.FindGameObjectsWithTag ("chip");
		foreach (GameObject clone in chipsOnTable) {
			Destroy (clone);
		}

	}

	public void Exit()
	{
		Application.LoadLevel ("menu");
		AudioManager.instance.PlaySound (MenuController.instance.hitSound);
	}

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnMenuLoaded;
	}

	void OnDisable()
	{
		MenuController.instance.SaveGame ();
	}



	void OnMenuLoaded(Scene scene, LoadSceneMode mode)
	{
		if (Application.loadedLevelName == "overview") {
			bet = 0;
			betText.text = bet.ToString () + " $";

		}
	}



}
