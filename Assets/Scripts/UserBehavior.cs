using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/* ToDo List
 * 
 */

public class UserBehavior : MonoBehaviour {

	public PlayerCharacter pc;
	public Text inputText;
	public List<string> inputList = new List<string>();
	public float clickTime, lastClick, lastFrame, averageTime, adaptClicks, adaptAttempts, adaptHints, adaptTime, adaptButton;
	public int clicks, i, numHelp, remCount, performance, currLevel, hintInd, numAttempts;
	public bool run, showHelp, perfect, acceptable, bad;
	public Rect windowRect = new Rect(0, 0, 0, 0);
	public string[] hints, idealInputs, pCommands, aCommands, bCommands, avgs;

	// Use this for initialization
	void Start () {
		i = 0; numHelp = -1; remCount = 1;
		run = false; showHelp = false; perfect = true; acceptable = true; bad = true;
		windowRect = new Rect (Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2);

		//reading in text file
		TextAsset hintTxt = (TextAsset)Resources.Load("hints", typeof(TextAsset));
		hints = hintTxt.text.Split ("\n" [0]);
		TextAsset inputTxt = (TextAsset)Resources.Load("idealInput", typeof(TextAsset));
		idealInputs = inputTxt.text.Split ("\n" [0]);
		TextAsset avgTxt = (TextAsset)Resources.Load("averages", typeof(TextAsset));
		avgs = avgTxt.text.Split ("\n" [0]);

		currLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex;
		print (currLevel);
		if (currLevel < 4) {
			print ("Pre Loops");
			hintInd = currLevel - 1;
			hintInd = hintInd * 4;
			print (hints [hintInd]);
		} else if (currLevel >= 4) {
			print ("In Loops");
			hintInd = currLevel - 2;
			hintInd = hintInd * 4;
			print (hints [hintInd]);
		}

		splitInputs ();

		//reading in global averages
		string temp = avgs[0];
		print (temp);
		string tClicks = temp.Substring (7);
		adaptClicks = float.Parse(tClicks);

		temp = avgs[1];
		print (temp);
		string tAttempts = temp.Substring (10);
		adaptAttempts = float.Parse(tAttempts);

		temp = avgs [2];
		print (temp);
		string tHints = temp.Substring (6);
		adaptHints = float.Parse (tHints);

		temp = avgs [3];
		print (temp);
		string tTime = temp.Substring (5);
		adaptTime = float.Parse (tTime);

		temp = avgs [4];
		print (temp);
		string buttonTime = temp.Substring (13);
		adaptButton = float.Parse (buttonTime);

		temp = avgs [5];
		print (temp);
		string lAttempts = temp.Substring (16);
		numAttempts = int.Parse (lAttempts);
	}
	
	// Update is called once per frame
	void Update () {
		if (run) {
			if (Time.frameCount % 24 == 0) {
				moveChar ();
			}
		}
	}

	void OnGUI(){
		GUI.skin.window.wordWrap = true; //wraps text in graphics window

		if (GUI.Button (new Rect (Screen.width * .01f, Screen.height * .78f, Screen.width * .1f, Screen.height * .1f), "Left")) {
			inputText.text += "Left\n";
			inputList.Add("Left");
			measureTime ();
			remCount = 1;
		}
		if (GUI.Button(new Rect(Screen.width * .11f, Screen.height * .78f, Screen.width * .1f, Screen.height * .1f), "Right")){
			inputText.text += "Right\n";
			inputList.Add("Right");
			measureTime ();
			remCount = 1;
		}
		if (GUI.Button (new Rect (Screen.width * .01f, Screen.height * .88f, Screen.width * .1f, Screen.height * .1f), "Delete")) {
			popList ();
			measureTime ();
		}
		if (GUI.Button(new Rect(Screen.width * .11f, Screen.height * .88f, Screen.width * .1f, Screen.height * .1f), "Up")){
			if (pc.wallUp == false) {
				inputText.text += "Up\n";
				inputList.Add("Up");
				measureTime ();
				remCount = 1;
			}
		}
		if (GUI.Button (new Rect (Screen.width * .88f, Screen.height * .88f, Screen.width * .1f, Screen.height * .1f), "Run")) {
			inputText.text = "";

			run = true;
			//when click run call function to loop through each set of commands and compare user input to those commands
		}

		if (GUI.Button (new Rect (Screen.width * .78f, Screen.height * .88f, Screen.width * .1f, Screen.height * .1f), "Help")) {
			if (!showHelp){
				showHelp = true;
				if (numHelp < hints.Length - 1) {
					numHelp++;
				} 
			} 
			else{
				showHelp = false;
			}
			//mouse click is treated the same as touch for a tap of the screen
			//LeanTouch asset has simple tap feature

		}
//		print (inputList.Count);	
		if (showHelp) {
			windowRect = GUI.Window (0, windowRect, DoMyWindow, hints[hintInd + numHelp]);
		}
	}

	void DoMyWindow(int windowID) {
		if (GUI.Button (new Rect (Screen.width/5, Screen.height/2.5f, 100, 20), "Got it!!!"))
			showHelp = false;

	}

	void popList(){
		int tempLen = inputList.Count;
		print (tempLen - remCount);
		string remString = inputList [tempLen - remCount];
		inputList.RemoveAt (tempLen - remCount);

		int fSlice = inputText.text.LastIndexOf (remString);
		inputText.text = inputText.text.Remove (fSlice);
		remCount++;
	}

	void measureTime(){
		clicks++;
		clickTime = Time.frameCount;

		lastClick = clickTime - lastFrame;
		averageTime = clickTime / clicks;
		lastFrame = clickTime;

		if (clicks > 10) {
			print ("The average click time is: " + averageTime);
			if (lastClick - averageTime > 50) {
				print ("Longer than expected");
			} else if (lastClick - averageTime < 25) {
				print ("Faster than expected");
			} else {
				print ("On Average");
			}
		}
		print ("The last click time was: " + lastClick);
	}

	void moveChar(){
		if (i < inputList.Count) {
			if (inputList [i].Contains ("L")) {
				pc.transform.Rotate (new Vector3(0,0,-90));
			} else if (inputList [i].Contains ("R")) {
				pc.transform.Rotate (new Vector3(0,0,90));
			} else if (inputList [i].Contains ("D")) {
				print ("clearing");
			} else if (inputList [i].Contains ("U")) {
				if (pc.wallUp == false) {
					pc.transform.Translate (new Vector3 (0, 1.25f, 0));
				} else {
					numAttempts++;
					string rewriteStr = "";
					rewriteStr = string.Concat(rewriteStr, "Click: ", adaptClicks.ToString(), "\n");
					rewriteStr = string.Concat(rewriteStr, "Attempts: ", adaptAttempts.ToString(), "\n");
					rewriteStr = string.Concat(rewriteStr, "Hints: 0\n");
					rewriteStr = string.Concat("Time: ", adaptTime.ToString(), "\n");
					rewriteStr = string.Concat(rewriteStr, "Button Time: ", adaptButton.ToString(), "\n");
					UnityEngine.SceneManagement.SceneManager.LoadScene (currLevel);
				}
			} else {
				print ("How did you get here");
			}
			i++;
		} else {
			i = 0;
			compareInput ();
			run = false;
		}
	}

	void splitInputs(){
		for (int j = 0; j < idealInputs.Length; j++) {
			if (idealInputs [j] [0].ToString() == "P") {  //perfect path
				string temp = idealInputs[j];
				string commands = temp.Substring(9);
				pCommands = commands.Split ("," [0]);
			} else if (idealInputs [j] [0].ToString() == "A") { //acceptable path
				string temp = idealInputs[j];
				string commands = temp.Substring(9);
				aCommands = commands.Split ("," [0]);
			} else if (idealInputs [j] [0].ToString() == "B") {  //bad path
				string temp = idealInputs[j];
				string commands = temp.Substring(5);
				bCommands = commands.Split ("," [0]);
			}
		}
	}

	void compareInput(){
		for (int j = 0; j < inputList.Count; j++) {
			if(j < pCommands.Length){
				if (pCommands [j] != inputList [j]) {
					performance += 5;
				}
			} if(j < aCommands.Length){
				if (aCommands [j] != inputList [j]) {
					performance += 3;
				}
			} if (j < bCommands.Length) {
				if (bCommands [j] != inputList [j]) {
					performance += 1;
				}
			} else {
				print ("Didn't work");
			}
		}
		inputList.Clear ();
		loadLevel ();
	}

	void loadLevel(){

		string rewriteStr = "";

		currLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex;
		print (currLevel);

//		reset hint count in text file

		//compare average number of button clicks against level number of button clicks
		if (clicks < adaptClicks) {
			performance += 2;
		} else if (clicks == adaptClicks) {
			performance += 1;
		} else if (clicks > adaptClicks) {
			performance -= 2;
		}
		adaptClicks = adaptClicks * currLevel; //make it psuedo total 
		adaptClicks = adaptClicks + clicks; //add new value
		adaptClicks = adaptClicks / currLevel; //reaverage it
		rewriteStr = string.Concat(rewriteStr, "Click: ", adaptClicks.ToString(), "\n");

		//compare number of attempts made to complete this level against average
		//comparing float and int 
		if (numAttempts < adaptAttempts) {
			performance += 2;
		} else if (numAttempts == adaptAttempts) {
			performance += 1;
		} else if (numAttempts > adaptAttempts) {
			performance -= 2;
		}
		adaptAttempts = adaptAttempts * currLevel; //make it psuedo total 
		adaptAttempts = adaptAttempts + numAttempts; //add new value
		adaptAttempts = adaptAttempts / currLevel; //reaverage it
		rewriteStr = string.Concat(rewriteStr, "Attempts: ", adaptAttempts.ToString(), "\n");

		//take in number of hints used comapre against average?
		if (numHelp == 0) {
			performance += 2;
		} else if (numHelp == 1) {
			performance += 1;
		} else if (numHelp > 2) {
			performance -= 2;
		}
		rewriteStr = string.Concat(rewriteStr, "Hints: 0\n");

		//compare average time to complete this puzzle against level completion time
		if (Time.frameCount < adaptTime) {
			performance += 2;
		} else if (Time.frameCount == adaptTime) {
			performance += 1;
		} else if (Time.frameCount > adaptTime) {
			performance -= 2;
		}
		adaptTime = adaptTime * currLevel; //make it psuedo total 
		adaptTime = adaptTime + Time.frameCount; //add new value
		adaptTime = adaptTime / currLevel; //reaverage it
		rewriteStr = string.Concat(rewriteStr, "Time: ", adaptTime.ToString(), "\n");

		//comparing average button click time against level button click
		if (averageTime < adaptButton) {
			performance += 2;
		} else if (averageTime == adaptButton) {
			performance += 1;
		} else if (averageTime > adaptButton) {
			performance -= 2;
		}
		adaptButton = adaptButton * currLevel; //make it psuedo total 
		adaptButton = adaptButton + averageTime; //add new value
		adaptButton = adaptButton / currLevel; //reaverage it
		rewriteStr = string.Concat(rewriteStr, "Button Time: ", adaptButton.ToString(), "\n");

		rewriteStr = string.Concat(rewriteStr, "Level Attempts: 0\n");

		System.IO.File.WriteAllText ("Assets/Resources/averages.txt", rewriteStr);

		if (performance >= 9) {
			print ("Perfect!!");
			//Load 2 levels ahead
			UnityEngine.SceneManagement.SceneManager.LoadScene (currLevel + 2);
		} else if (performance > 5) {
			print ("Average!!");
			//Load next level
			UnityEngine.SceneManagement.SceneManager.LoadScene (currLevel + 1);
		} else if (performance > 2) {
			print ("Bad!!");
			//reload same level
			UnityEngine.SceneManagement.SceneManager.LoadScene (currLevel);
		} else {
			print ("Terrible!!");
			//load prevoius level
			UnityEngine.SceneManagement.SceneManager.LoadScene (currLevel - 1);
		}
	}
}