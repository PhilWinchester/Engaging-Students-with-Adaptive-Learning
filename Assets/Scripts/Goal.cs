using UnityEngine;
using System.Collections;

/* ToDo List
 * rework to have endtime show in seconds...not frames
 * 
 */

public class Goal : MonoBehaviour {

	public PlayerCharacter pc;
	public Student stud;
	public float endtime;
	public bool finished;

	// Use this for initialization
	void Start () {
		finished = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Player") {
			print ("YOU WIN");
			endtime = Time.frameCount;
			print ("You completed the puzzle in: " + endtime);
			pc.wallDown = true; pc.wallLeft = true; pc.wallRight = true; pc.wallUp = true;
			finished = true;
		}
	}

	void OnGUI(){
		if (finished) {
			if (GUI.Button (new Rect (Screen.width * .88f, Screen.height * .88f, Screen.width * .1f, Screen.height * .1f), "Restart")) {
				
			}
		}
	}
}
