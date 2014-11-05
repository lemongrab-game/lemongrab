using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{


    private GameObject[] team1;
    private GameObject[] team2;

    public static GameController controller;

    public int currentTeam = 1;

    void Start()
    {
        team1 = GameObject.FindGameObjectsWithTag("Team1");
        team2 = GameObject.FindGameObjectsWithTag("Team2");
        controller = this;
    }

    public void Go()
    {
        TurnController.controller.nextTurn();
        GoTeam(team1);
        GoTeam(team2);
        
    }

    public void GoTeam(GameObject[] team)
    {
        PlayerInput input;
        foreach (GameObject player in team)
        {
            input = player.GetComponent<PlayerInput>();
            if (input != null)
            {
                input.PlayerAction();
            }
        }
    }



    public void TeamTurn()
    {
        GameObject[] teamWanted = (currentTeam == 2) ? team1 : team2;
        int idx = Random.Range(0, teamWanted.GetLength(0));

        teamWanted[idx].GetComponent<PlayerInput>().Select();

        currentTeam = (currentTeam == 1) ? 2 : 1;
    }










    public static bool IsOpponent(GameObject go1, GameObject go2)
    {

        if(go1.tag.Contains("Team") && go2.tag.Contains("Team"))
        {
            if(go1.tag != go2.tag)
            {
                return true;
            }
        }
        return false;
    }



    public static bool carryObject(GameObject go, GameObject carriedObject)
    {

        foreach (Transform t in go.GetComponentsInChildren<Transform>())
        {
            if (t.gameObject == carriedObject)
            {
                return true;
            }
        }
        return false;
      
    }


		
}
