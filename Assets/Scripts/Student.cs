using UnityEngine;
using System.Collections;

/* 
 * 
 */

public class Student : MonoBehaviour {

	public float avgPuzzleTime;
	public float numPuzzles;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void avgCompleteTime(float endTime){
		numPuzzles++;
		avgPuzzleTime = endTime / numPuzzles;
		print ("The Average Puzzle Completion Time is: " + avgPuzzleTime);
	}
}
