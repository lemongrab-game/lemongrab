using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class PlayerGenerator : MonoBehaviour 
{


    public TextAsset playerNameXML;

    void Awake()
    {

        GameObject[] team = GameObject.FindGameObjectsWithTag("Team1");

        foreach (GameObject teamMate in team)
        {
            PlayerInput input  = teamMate.GetComponent<PlayerInput>();
            if (input != null)
            {
                BasicPlayer player = GenerateRandomPlayer();
                input.playerData = player;
            }
        }
        
    }

    public BasicPlayer GenerateRandomPlayer()
    {
        BasicPlayer player = new BasicPlayer();
        player.name = GetRandomPlayerName(playerNameXML);
        player.position = GetRandomPosition();
        //TODO: player.race = "Human";
        player.clubID = UnityEngine.Random.Range(0, 100);
        player.age = UnityEngine.Random.Range(16, 25);
        player.level = UnityEngine.Random.Range(1, 100);
        player.health = UnityEngine.Random.Range(5, 20);

        player.speed = UnityEngine.Random.Range(1, 20);
        player.strength = UnityEngine.Random.Range(1, 20);
        player.intelligence = UnityEngine.Random.Range(1, 20);
        player.agility = UnityEngine.Random.Range(1, 20);
        player.endurance = UnityEngine.Random.Range(1, 20);

        player._mental = UnityEngine.Random.Range(1, 20);
        player._wisdom = UnityEngine.Random.Range(1, 20);
        player._luck = UnityEngine.Random.Range(1, 20);

        return player;
    }


    public string GetRandomPlayerName(TextAsset txt)
    {
        string[] tags = new string[3] {"Firstname","Lastname","Land"};
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(txt.text);

        Dictionary<string, string> replacments = new Dictionary<string, string>();


        string playerName = "[Firstname] [Lastname] from [Land]";
        ArrayList values = new ArrayList();

        foreach(string tag in tags)
        {
            values.Clear();
            XmlNodeList itemList = xmlDoc.GetElementsByTagName(tag +"s");

            foreach (XmlNode itemInfo in itemList)
            {
                XmlNodeList itemContent = itemInfo.ChildNodes;

                foreach (XmlNode content in itemContent)
                {
                    values.Add(content.InnerText);
                }
            }
            playerName = playerName.Replace("["+tag+"]",  (string) values[UnityEngine.Random.Range(0, values.Count)]);
        }

        return playerName;
    }

    public BasicPlayer.PositionChoice GetRandomPosition()
    {
        Array values = Enum.GetValues(typeof(BasicPlayer.PositionChoice));
        System.Random rand = new System.Random();
        BasicPlayer.PositionChoice position = (BasicPlayer.PositionChoice)values.GetValue(rand.Next(values.Length));
        return position;
    }
}
