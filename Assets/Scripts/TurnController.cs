using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TurnController : MonoBehaviour 
{

    public static TurnController controller;

    public int maxTurn = 22;

    public int currentTurn = 1;

    public Text turnText;
    public Button turnButton;
    public Button replayButton;

    public bool recordTurn = true;

    public Camera replayCam;
    public float recordDelta = 0.2f;



    public float prepareDuration = 30f;
    public float runDuration = 10f;

    public string[] movingObjectsTags;
    public ArrayList movingObjects = new ArrayList();
    public Dictionary<GameObject, ArrayList> objectsPaths = new Dictionary<GameObject, ArrayList>();

    public enum GamePhase
    {
        PREPARE, //limited time to prepare moves and actions
        RUN, //Play the move/actions
        REPLAY, //replay 
        FINISH //end of the game
    }

    public GamePhase phase;

	void Start () 
    {
        controller = this;
        SetText();
        SetPhase(GamePhase.PREPARE);

       

        if(movingObjectsTags != null)
        {
            int maxFrames = Mathf.CeilToInt(runDuration / recordDelta);
            Debug.Log("maxFrames: " + maxFrames);
            foreach(string tag in movingObjectsTags)
            {
                GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject obj in objects)
                {
                    
                    objectsPaths.Add(obj, new ArrayList(maxFrames));
                }
                //movingObjects.Add()
            }
        }
	}
	
    public void nextTurn()
    {
        SetPhase(GamePhase.RUN);
        currentTurn++;
        if (currentTurn > maxTurn)
        {
            //TODO: end of the game
            SetPhase(GamePhase.FINISH);
        }
        else
        {
            SetText();
            Invoke("FinishRun", runDuration);
        }
       
        if(recordTurn)
        {
            //TODO: Clean paths
            CleanPaths();
            InvokeRepeating("RecordFrame", 0f, recordDelta);
            
        }
    }

    void CleanPaths()
    {
        foreach (GameObject obj in objectsPaths.Keys)
        {
            objectsPaths[obj].Clear();
        }
    }

    void RecordFrame()
    {
        if (phase == GamePhase.RUN)
        {
            Vector3 pos;
            ArrayList path;
            foreach (GameObject obj in objectsPaths.Keys)
            {
                pos = obj.transform.position;
                path = objectsPaths[obj];
                path.Add(pos);
                Debug.DrawLine(pos, new Vector3(pos.x, pos.y + 10f, pos.z), Color.red, 50f);

            }
        }
    }

   

    void FinishRun()
    {
        //TODO: stop every moving object 
        SetPhase(GamePhase.PREPARE);
        CancelInvoke("RecordFrame");
        
    }

    public void ShowReplay()
    {
        StartCoroutine("Replay");
    }

    void TogglePhysics(bool mode)
    {
        Rigidbody body;
        Collider collider;
        foreach (GameObject obj in objectsPaths.Keys)
        {
            body = obj.rigidbody;
            if(body != null)
            {
                body.isKinematic = !mode;
            }
            collider = obj.GetComponent<Collider>();
            if(collider != null)
            {
                collider.enabled = mode;
            }
        }
    }

    IEnumerator Replay()
    {

        SetPhase(GamePhase.REPLAY);
        TogglePhysics(false);

        int frameNum = 0;
        bool hasNextFrame = true;
        float replayTime = 1f;

        while(hasNextFrame)
        {
            //Debug.Log("replaytime: " + replayTime);
            ArrayList frames;
            Vector3 pos;
            frameNum = Mathf.FloorToInt(replayTime / recordDelta);
            float t = (replayTime / recordDelta) - frameNum;

            //Debug.Log("FrameNum: " + frameNum);
            //Debug.Log("t: " + t);
            foreach (GameObject obj in objectsPaths.Keys)
            {
                frames = objectsPaths[obj];
                
                if (frameNum < frames.Count)
                {
                    if ((frameNum + 1) >= frames.Count)
                    {
                        pos = (Vector3)frames[frameNum];
                        //Debug.LogWarning("argh");
                    }
                    else
                    {
                        pos = Vector3.Lerp((Vector3)frames[frameNum], (Vector3)frames[frameNum + 1], t);
                        Debug.DrawLine(pos, new Vector3(pos.x, pos.y + 20f, pos.z), Color.green, 50f);
                    }
                    obj.transform.position = pos;
                    //Debug.Log("framenum: " + frameNum + "for " + obj.name +"::"+obj.GetInstanceID());
                }
                else
                {
                    hasNextFrame = false;
                }
            }
            if (hasNextFrame)
            {
                //yield return new WaitForSeconds(recordDelta);
                //frameNum++;
                yield return null;
            }
            replayTime += Time.deltaTime;
            //if(replayTime > 1f)
            //{
            //    Debug.Break();
            //}
        }
        TogglePhysics(true);
        SetPhase(GamePhase.PREPARE);
    }

    void SetText()
    {
        turnText.text = currentTurn + "/" + maxTurn;
    }


    //TODO: change phase function
    void SetPhase(GamePhase p)
    {
        phase = p;
        if(turnButton != null)
        {
            if(phase == GamePhase.PREPARE)
            {
                turnButton.interactable = true;
            }
            else
            {
                turnButton.interactable = false;
            }
        }
        if (replayButton != null)
        {
            if(phase == GamePhase.PREPARE || phase == GamePhase.FINISH)
            {
                replayButton.interactable = true;
            }
            else
            {
                replayButton.interactable = false;
            }
        }
    }
}
