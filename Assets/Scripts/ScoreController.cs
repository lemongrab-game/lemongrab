using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {

    public int team1Score = 0;
    public int team2Score = 0;

    public Text team1Text;
    public Text team2Text;


    public static ScoreController controller;

	void Start () 
    {
        controller = this;
	}


    public void AddScore(int teamNum, int amount)
    {
        if(teamNum == 1)
        {
            team1Score += amount;
        }
        else if (teamNum == 2)
        {
            team2Score += amount;
        }
        UpdateScore();
    }

    void UpdateScore()
    {
        team1Text.text = team1Score.ToString();
        team2Text.text = team2Score.ToString();
    }
	

}
