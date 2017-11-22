using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
public class MenuController : MonoBehaviour {

	// Use this for initialization
	public Canvas confirmationPanel;
	public Text text;
	public InputField playerName;
	private bool nameEntered;
	public GameObject yes;
	public GameObject confirm;
	public Text playerProfile;
	public GameObject wanted;
	public AudioClip hitSound;
	public AudioClip exitSound;

	public static MenuController instance=null;
	GameObject inputField;
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
	void Start()
	{
		wanted.SetActive (false);
		 inputField = GameObject.Find ("InputField");
		inputField.SetActive (false);
		
		confirmationPanel.enabled = false;
		GetData ();
		playerProfile.text = Player.instance.Name;

	}
	void PlaySound()
	{
		AudioManager.instance.PlaySound (hitSound);
	}
	public void EnterName()
	{
		PlaySound ();
		inputField.SetActive (true);

		text.text = "What's your name, fella?";
		Player.instance.Name = playerName.text.ToString ();
		confirm.SetActive (true);
		yes.SetActive (false);



	}
	public void Confirm()
	{
		PlaySound ();
		Player.instance.Name = playerName.text.ToString ();
			File.Delete (Application.persistentDataPath + "/playerData.bin");
			Cancel ();
			NewGame ();

	}

	public void Cancel()
	{

		confirmationPanel.enabled = false;
		inputField.SetActive (false);
	}
	public void NewGame()
	{
		
		PlaySound ();
		if (!File.Exists (Application.persistentDataPath + "/playerData.bin")&&Player.instance.Name.Length!=0) {
			
			SetToDefault ();
			SaveGame ();
			Application.LoadLevel ("overview");
		} else {
			yes.SetActive (true);
			confirm.SetActive (false);
			confirmationPanel.enabled = true;
			GameObject inputField = GameObject.Find ("InputField");
			inputField.SetActive (false);

		}
	}
	void SetToDefault()
	{
		Player.instance.dollars = 1000;
	}
	public void LoadGame()
	{
		PlaySound ();
		if (File.Exists (Application.persistentDataPath + "/playerData.bin")) {
			GetData ();
			Application.LoadLevel ("overview");
		}

	}

	public void GetData()
	{
		if (File.Exists (Application.persistentDataPath + "/playerData.bin")) {
			wanted.SetActive (true);
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerData.bin", FileMode.Open, FileAccess.Read);
			int dollars = (int)bf.Deserialize (file);
			string name = (string)bf.Deserialize (file);

			file.Close ();
			Player.instance.dollars = dollars;
			Player.instance.Name = name;

			Debug.Log ("Loaded");
		}
	}

	public void Exit()
	{
		AudioManager.instance.PlaySound (exitSound);
		Application.Quit ();

	}

	public void SaveGame()
	{
		
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerData.bin");
		bf.Serialize (file, Player.instance.dollars);
		bf.Serialize (file, Player.instance.Name);
		file.Close ();
		Debug.Log ("Saved");
	}


}
