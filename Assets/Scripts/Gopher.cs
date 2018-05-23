using UnityEngine;
using System.Collections;

public class Gopher : MonoBehaviour
{
	public bool isAlived = false;
	public bool isAppearing = false;

	public float timeElapsed = 0.0f;
	public float timeMovedUp = 0.0f;
	public float timeMovedDown = 0.0f;

	private Vector3 oldPosition;
	private Vector3 newPosition;

	// Use this for initialization
	void Start () 
	{
		oldPosition = this.transform.position;
		newPosition = new Vector3 (oldPosition.x, oldPosition.y + 3.0f, oldPosition.z);
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeElapsed += Time.deltaTime;

		if (timeMovedUp <= 1.0f) {
			timeMovedUp += Time.deltaTime;
			this.transform.position = Vector3.Lerp (oldPosition, newPosition, timeMovedUp);
		}

		if (timeElapsed >= 2.0f && timeMovedDown <= 1.0f) {
			timeMovedDown += Time.deltaTime;
			this.transform.position = Vector3.Lerp (newPosition, oldPosition, timeMovedDown);
		}
	}
}