using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel : MonoBehaviour {
	
	public Sprite cardBack;
	SpriteRenderer sprRend;
	public int cardIndex;
	public Sprite[] faces;
	public bool isTurned = false;
	public AnimationCurve scaleCurve;
	public float duration=0.75f;


	public void TurnFace (bool showFace)
	{
		if (showFace) {
			sprRend.sprite = faces [cardIndex];
			isTurned = true;
		}
		else {
			sprRend.sprite = cardBack;
			isTurned = false;
		}
	}
	// Use this for initialization
	void Awake () {
		sprRend = GetComponent<SpriteRenderer> ();


	}
	
	// Update is called once per frame
	void Update () {

	}





	// Use this for initialization

	public void FlipCard (Sprite startImage, Sprite endImage, int cardIndex)
	{
		StopCoroutine (Flip (startImage, endImage, cardIndex));
		StartCoroutine (Flip (startImage, endImage, cardIndex));
	}

	IEnumerator Flip(Sprite startImage, Sprite endImage, int cardIndex)
	{
		sprRend.sprite = startImage;

		float time = 0f;
		while (time <= 1f) {
			float scale = scaleCurve.Evaluate (time);
			time += Time.deltaTime / duration;

			Vector3 localScale = transform.localScale;
			localScale.x = scale;
			transform.localScale = localScale;

			if (time >= 0.35f) {
				sprRend.sprite = endImage;
			}

			yield return new WaitForFixedUpdate();
		}


		if (!isTurned) {
			
			TurnFace (true);
		}

	}

}
