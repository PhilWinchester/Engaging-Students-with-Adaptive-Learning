using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	// Use this for initialization
	void Start () {
			
		switch(Random.Range(0,4)){
			case 0:
				this.transform.Rotate (0, 0, 90);
				break;
			case 1: 
				this.transform.Rotate (0, 0, -90);
				break;
			case 2: 
				this.transform.Rotate (0, 0, 180);
				break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
