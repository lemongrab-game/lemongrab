using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerInput))]
public class WallBuilder : MonoBehaviour, IPlayerAction
{

    public GameObject wall;

    public float duration;

    private GameObject createdWall;

    private Vector3 targetPosition = Vector3.zero;

    private float posY;
	
	void Start () 
    {
        GetComponent<PlayerInput>().AddAction("WALL", this);
        if(wall != null)
        {
            posY = wall.renderer.bounds.size.y/2;
        }
	}

    void IPlayerAction.BuildAction()
    {
        if (wall != null && targetPosition == Vector3.zero)
        {
            //Debug.Log("wall non null");
            if (createdWall == null)
            {
                createdWall = (GameObject)Instantiate(wall);
            }
            createdWall.SetActive(true);
            //Debug.Log("createdWall: " + createdWall.ToString());


            Vector3 mousePos = PlayerInput.GetMousePosition();
            if (mousePos != null)
            {
                createdWall.transform.position = new Vector3(mousePos.x,posY,mousePos.z);
            }


            if (Input.GetMouseButtonUp(0))
            {
                //save position
                //Debug.Log("mouseup " + Time.time);
                targetPosition = mousePos;
                //Debug.Log("targetPosition: " + targetPosition);
            }
        }
    }


    bool IPlayerAction.IsActive()
    {
        //enough mana??
        return true;
    }

    bool IPlayerAction.ExecuteAction()
    {
        Invoke("Deactivate", duration);
        return true;
    }
    void IPlayerAction.CancelAction()
    {
        targetPosition = Vector3.zero;
        //set active false;
        if(createdWall != null)
        {
            createdWall.SetActive(false);
        }
    }

    void Deactivate()
    {
        if (createdWall != false)
        {
            createdWall.SetActive(false);
        }
    }
}
