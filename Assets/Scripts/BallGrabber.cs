using UnityEngine;
using System.Collections;


[RequireComponent(typeof(PlayerInput))]
public class BallGrabber : MonoBehaviour, IPlayerAction
{

    public Vector3 ballPos;
    
    private GameObject ball;

    public bool hasBall = false;


    public LineRenderer line;

    public Vector3 targetPosition = Vector3.zero;


	
	void Start () 
    {
        GetComponent<PlayerInput>().AddAction("PASS", this);
        ball = GameObject.Find("RoundBall");
        line = GetComponent<LineRenderer>();
	}
	

    void IPlayerAction.BuildAction()
    {
        
        if (hasBall)
        {
            if (targetPosition == Vector3.zero)
            {
                //Input the pass
                line.SetPosition(0, ball.transform.position);
                Vector3 lineEndPosition = Vector3.zero;
                line.enabled = false;
                Vector3 mousePos = PlayerInput.GetMousePosition();
                if (mousePos != null)
                {
                    lineEndPosition =  new Vector3(mousePos.x, 1, mousePos.z);
                    line.SetPosition(1,lineEndPosition);
                    line.enabled = true;
                }
               

                if (Input.GetMouseButtonUp(0))
                {
                    //save position
                    Debug.Log("mouseup " + Time.time);
                    targetPosition = lineEndPosition;
                }
            }
           
        }
    }

    bool IPlayerAction.IsActive()
    {
        return hasBall;
    }

    bool IPlayerAction.ExecuteAction()
    {

        if (hasBall && targetPosition != Vector3.zero)
        {
            ReleaseBall();
            ball.rigidbody.AddForce((targetPosition - transform.position) * 10);
            return true;
        }
        return false;
    }

    void IPlayerAction.CancelAction()
    {
        targetPosition = Vector3.zero;
    }



    void OnCollisionEnter(Collision collision)
    {
        //Grab the ball
        GameObject go = collision.gameObject;
        if(go == ball)
        {
            GrabBall();
        }
        

        //If the other player has the ball
        if (GameController.IsOpponent(gameObject, go))
        {
            if(GameController.carryObject(go, ball))
            {
                Tacle(collision);

            }

        }
    }

    void GrabBall()
    {
        ball.transform.parent = transform;
        ball.collider.enabled = false;
        ball.rigidbody.isKinematic = true;
        ball.transform.localPosition = ballPos;
        hasBall = true;
    }

    

    void ReleaseBall()
    {
        ball.transform.localPosition = new Vector3(0, 3, 0);
        ball.transform.parent = null;
        ball.collider.enabled = true;
        ball.rigidbody.isKinematic = false;
        hasBall = false;
    }

    void Tacle(Collision collision)
    {
        ReleaseBall();
        //TODO: relative to contact force
        ball.rigidbody.AddForce(new Vector3(Random.Range(200, 500), Random.Range(200, 1000), Random.Range(200, 500)));

    }


    
}
