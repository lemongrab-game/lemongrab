using UnityEngine;
using System.Collections;


public class BasicPlayer
{

    public string name;
    public enum PositionChoice
    {
        LEFT_WING = 0,
        RIGHT_WING = 1,
        LEFT_CENTER = 2,
        RIGHT_CENTER = 3,
        DEFENSE = 4
    }

    public PositionChoice position;

    public PlayerRace race;

    public int clubID;

    public int age;

    public int level;

    public int health;

    //stats
    public float speed;
    public float strength;
    public float intelligence;
    public float agility;
    public float endurance;

    //hidden stats
    public float _mental;
    public float _wisdom;
    public float _luck;


    public PlayerSkill[] skills;

    public string DisplayDescription()
    {

        return "Name: " + name +
               "\n Position: " + position +
               "\n Club: " + clubID +
               "\n Age: " + age + " years old" +
               "\n Level: " + level +
               "\n health: " + health +
               "\n ***STATS***" +
               "\n Speed: " + speed +
               "\n Strength: " + strength +
               "\n Intelligence: " + intelligence +
               "\n Agility: " + agility +
               "\n Endurance: " + endurance;
    }

}
