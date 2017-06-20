using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {

	private Text scoreText;
	public static int score = 0;


    private void Start()
    {
		scoreText = GetComponent <Text>();
    }

    public void Score(int points){
		Debug.Log ("Scored points");
		score += points;
		scoreText.text = score.ToString ();
	}


    public static void Reset()
    {
		score = 0;
		//scoreText.text = score.ToString ();
    }


}
