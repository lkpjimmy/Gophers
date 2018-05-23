using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour 
{
	public GameObject gopherPrefab;
	public GameObject hammerPrefab;
	public GameObject hitEffectPrefab;

	public List<Gopher> gopherList = new List<Gopher>();
	public List<Transform> platformList = new List<Transform>();
	public List<Image> heartList = new List<Image>();

	public Text moneyText;
	public Image timeBar;

	public GameObject gameOverCanvas;
	public Text gameOverText;

	private float hp = 4.0f;
	private int coin = 0;

	private float gameTime = 60.0f;
	private bool isGameOver = false;

	private KeyCode[] keyCodes = 
	{
		KeyCode.Keypad1,
		KeyCode.Keypad2,
		KeyCode.Keypad3,
		KeyCode.Keypad4,
		KeyCode.Keypad5,
		KeyCode.Keypad6,
		KeyCode.Keypad7,
		KeyCode.Keypad8,
		KeyCode.Keypad9,
	};
		
	// Use this for initialization
	void Start ()
	{
		StartCoroutine (SpawnRandomly ());
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isGameOver == true) {
			return;
		}

		MonitorGopherStatus ();
		MonitorInput ();
		MonitorTimeAndHP ();
	}

	IEnumerator SpawnRandomly ()
	{
		while (isGameOver == false) {
			float time = (float)Random.Range (0.25f, 0.5f);
			yield return new WaitForSeconds (time);
			SpawnGopher ();
		}
	}

	int GetNumberOfGopher ()
	{
		int sum = 0;

		for (int i = 0; i < gopherList.Capacity; i++) {
			if (gopherList [i] != null && gopherList [i].isAlived == true) {
				sum++;
			}
		}

		return sum;
	}

	void SpawnGopher()
	{
		if (GetNumberOfGopher () >= 6) {
			return;
		}

		int index = (int)Random.Range (0, 8);
		while (gopherList[index] != null && gopherList [index].isAlived == true) {
			index = (int)Random.Range (0, 8);
		}

		GameObject gopher = (GameObject)Instantiate (gopherPrefab, platformList [index].position, gopherPrefab.transform.rotation);

		gopherList [index] = gopher.GetComponent<Gopher> ();
		gopherList [index].isAlived = true;
	}

	void MonitorGopherStatus ()
	{
		for (int i = 0; i < gopherList.Capacity; i++) {
			if (gopherList [i] != null && gopherList [i].transform.position.y >= -1.1f) {
				gopherList [i].isAppearing = true;
			}

			if (gopherList [i] != null && gopherList [i].isAppearing == true && gopherList [i].isAlived == true && gopherList [i].transform.position.y < -1.1f) {
				Destroy (gopherList [i].gameObject);
				gopherList [i].isAppearing = false;
				gopherList [i].isAlived = false;
				gopherList [i] = null;

				DeductHP ();
			}
		}
	}

	void MonitorInput ()
	{
		for (int i = 0; i < keyCodes.Length; i++) {
			if (Input.GetKeyDown (keyCodes [i]))
			{
				GameObject hitEffect = (GameObject)Instantiate (hitEffectPrefab, platformList [i].position, hitEffectPrefab.transform.rotation);
				hitEffect.transform.position = new Vector3 (hitEffect.transform.position.x, 1.0f, hitEffect.transform.position.z);

				Destroy (hitEffect, 0.2f);

				GameObject hammer = (GameObject)Instantiate (hammerPrefab, platformList [i].position, hammerPrefab.transform.rotation);
				hammer.transform.position = new Vector3 (hammer.transform.position.x + 0.75f, 1.5f, hammer.transform.position.z);

				Destroy (hammer, 0.2f);

				if (gopherList [i] != null && gopherList [i].isAppearing == true) {
					Destroy (gopherList [i].gameObject);
					gopherList [i].isAppearing = false;
					gopherList [i].isAlived = false;
					gopherList [i] = null;

					AddCoin ();
				}
				else {
					DeductHP ();
				}
			}
		}
	}

	void MonitorTimeAndHP() {
		gameTime -= Time.deltaTime;
		timeBar.fillAmount = gameTime / 60.0f;

		if (gameTime <= 0) {
			gameOverCanvas.SetActive (true);
			gameOverText.text = "You Win!";
			isGameOver = true;
		}
		else if (hp <= 0.0f) {
			gameOverCanvas.SetActive (true);
			gameOverText.text = "Game Over";
			isGameOver = true;			
		}

	}

	void DisplayCoin (int coin)
	{
		moneyText.text = "X " + coin.ToString ();
	}

	void DisplayHeart (float hp)
	{
		// hp is 3.5, index is 3
		int index = (int)Mathf.Floor (hp);
		heartList [index].fillAmount = hp - Mathf.Floor (hp);
	}

	public void AddCoin ()
	{
		coin = coin + 1;
		DisplayCoin (coin);

	}

	public void DeductHP ()
	{
		hp = hp - 0.5f;
		DisplayHeart (hp);

	}
}
