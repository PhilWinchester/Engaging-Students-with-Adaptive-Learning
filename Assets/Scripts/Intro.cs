using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.skin.window.wordWrap = true; //wraps text in graphics window

		if (GUI.Button (new Rect (Screen.width * .5f, Screen.height * .78f, Screen.width * .1f, Screen.height * .1f), "Ready?")) {
			UnityEngine.SceneManagement.SceneManager.LoadScene (1);
		}
	}
}
