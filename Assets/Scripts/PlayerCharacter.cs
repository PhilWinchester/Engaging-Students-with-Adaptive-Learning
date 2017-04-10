using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCharacter : MonoBehaviour {

	public bool wallUp, wallLeft, wallRight, wallDown;
	public Transform sightStart, sightUp, sightLeft, sightRight, sightDown;
	public RaycastHit2D hitUp, hitLeft, hitRight, hitDown;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		wallUp = false; wallLeft = false; wallRight = false; wallDown = false;

		hitUp = Physics2D.Linecast (sightStart.position, sightUp.position,1 << LayerMask.NameToLayer("Wall"));
		hitLeft = Physics2D.Linecast (sightStart.position, sightLeft.position,1 << LayerMask.NameToLayer("Wall"));
		hitRight = Physics2D.Linecast (sightStart.position, sightRight.position,1 << LayerMask.NameToLayer("Wall"));
		hitDown = Physics2D.Linecast (sightStart.position, sightDown.position,1 << LayerMask.NameToLayer("Wall"));

		Debug.DrawLine (sightStart.position, sightUp.position, Color.red);
		Debug.DrawLine (sightStart.position, sightRight.position, Color.red);
		Debug.DrawLine (sightStart.position, sightLeft.position, Color.red);
		Debug.DrawLine (sightStart.position, sightDown.position, Color.red);

		if (hitUp && hitUp.collider.gameObject.tag == "Wall") {
			wallUp = true;
		}
	}
}
