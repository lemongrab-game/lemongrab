using UnityEngine;
using System.Collections;

public class LineBehavior : MonoBehaviour 
{
    [Range(1,2)]
    public int teamNum; //name of the team who scores when this line is crossed with the ball


    private GameObject ball;

	void Start () 
    {
        ball = GameObject.Find("RoundBall");
	}
	
	void OnTriggerEnter(Collider other)
    {
        if (teamNum != null)
        {
            //Debug.Log("other.tag: " + other.tag);
            if (other.tag == "Team"+teamNum) //is it the good player? 
            {
                if(GameController.carryObject(other.gameObject,ball))
                {
                    ScoreController.controller.AddScore(teamNum, 3);
                    return;
                }
            }
        }
    }
}
