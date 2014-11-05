using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public interface IPlayerAction
{

    void BuildAction();
    bool IsActive();
    bool ExecuteAction();
    void CancelAction();
}

[SelectionBase]
public class PlayerInput : MonoBehaviour 
{
    public bool isSelected = false;
    public GameObject graphics;
    public float maxMagnitude = 10f;

    public SpriteRenderer marker;

    private Transform trans;

    private GameObject[] team;
    public int clickCount = 0;

    private LineRenderer moveLine;

    public Vector3 savedDirection = Vector3.zero;
    public float savedMagnitude = 0f;

    private Rigidbody body;

    public Vector3 camPosition = new Vector3(0, 20, 20);
    public float camAngle = 40f;

    public Text speedText;
    private Canvas playerCanvas;

    private Dictionary<string, IPlayerAction> actions = new Dictionary<string,IPlayerAction>();
    public string currentAction;

    public GameObject actionButton;

    private bool skipFrame = false;

    public BasicPlayer playerData;
    public Text playerDescription;

    void Awake()
    {
        trans = transform;
        body = rigidbody;
        team = GameObject.FindGameObjectsWithTag(gameObject.tag);
        moveLine = GetComponent<LineRenderer>();
        moveLine.SetPosition(0,trans.position);
        moveLine.enabled = false;
       
        playerCanvas = GetComponentInChildren<Canvas>();
        playerCanvas.gameObject.SetActive(false);
        
    }

    void Start()
    {
        if(playerData != null && playerDescription != null)
        {
            playerDescription.text = playerData.DisplayDescription();
        }
    }

    public static Vector3 GetMousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.green, 0.2f);
        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Ground")))
        {
            return new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
        return Vector3.zero;
    }

    public void AddAction(string key, IPlayerAction action)
    {
        actions.Add(key, action);
        //Add button if not exist
        if (actionButton != null)
        {
            playerCanvas.gameObject.SetActive(true);

            GameObject newButton = (GameObject)Instantiate(actionButton);
            newButton.name = key;
            Text t = newButton.GetComponentInChildren<Text>();
            if (t != null)
            {
                t.text = key;
            }
            RectTransform rect = newButton.GetComponent<RectTransform>();
            rect.SetParent(playerCanvas.gameObject.transform, false);
            //TODO: dynamic
            float xPos = 50f + (100f * (actions.Count - 1f));
            //Debug.Log(key + " action position: " + xPos);

            if (actions.Count == 1)
            {
                rect.anchoredPosition.Set(0, 0);
            }
            rect.localScale = Vector3.one;
            rect.localRotation = Quaternion.identity;
            rect.localPosition = new Vector3(xPos, -20, 0);
            playerCanvas.gameObject.SetActive(false);
        }
        
    }
	
	void Update () 
    {

        if(skipFrame)
        {
            skipFrame = false;
            return;
        }

        if (isSelected && currentAction != null && actions.ContainsKey(currentAction))
        {
           
            IPlayerAction action = actions[currentAction];
            action.BuildAction();
            return;
           
        }


	    if(isSelected && clickCount > 1)
        {

            if(Input.GetMouseButton(0))
            {
                Vector3 mousePos = PlayerInput.GetMousePosition();
                if (mousePos != null)
                {
                    graphics.transform.position = new Vector3(
                        mousePos.x,
                        graphics.transform.position.y,
                        mousePos.z
                    );
                    if(moveLine != null)
                    {
                        float magnitude = Mathf.Min(Vector3.Magnitude(graphics.transform.position - trans.position), maxMagnitude);
                        moveLine.SetPosition(0, trans.position);
                        moveLine.SetPosition(1, trans.position + Vector3.Normalize(trans.position - graphics.transform.position) * magnitude * 4f);
                        if(!moveLine.enabled)
                        {
                            moveLine.enabled = true;
                        }
                        if(speedText != null)
                        {
                            
                            float speed = Mathf.Floor(magnitude / maxMagnitude *100f);
                            speedText.text = "Speed: " + speed + " %";
                        }
                        //TODO: rebounds??
                    }
                }
               
            }
            if(Input.GetMouseButtonUp(0))
            {

                savedDirection = - graphics.transform.localPosition;
                savedMagnitude = Mathf.Min(Vector3.Magnitude(graphics.transform.position - trans.position), maxMagnitude);
                graphics.transform.localPosition = Vector3.zero;
                clickCount = 1;
                //moveLine.enabled = false;
               
            }
        }
	}


    void OnMouseDown()
    {
        //Unselect everthing else
        PlayerInput input;
        foreach(GameObject teamMate in team)
        {
            input = teamMate.GetComponent<PlayerInput>();
            if (teamMate != null && input != null && input.gameObject != gameObject)
            {
                input.Unselect();
            }
        }
        Select();
    }


    public void Select()
    {
        isSelected = true;
        trans.localScale = new Vector3(1.2f, 1.2f, 1.2f); //TODO: Change color 
        marker.gameObject.SetActive(true);
        clickCount++;
       
        //TODO: find the best angle
        Transform camTrans = Camera.main.transform;
        float z = (gameObject.tag == "Team1") ? +camPosition.z : -camPosition.z;
        float yRot = (gameObject.tag == "Team1") ? 0 : 180;
        camTrans.position = trans.position - new Vector3(0, -camPosition.y, z);
        Vector3 angles = camTrans.rotation.eulerAngles;
        camTrans.rotation = Quaternion.Euler(new Vector3(angles.x, yRot, angles.z));
        //speedText.transform.gameObject.SetActive(true);
        
        
        //update available actions
        playerCanvas.gameObject.SetActive(true);
        UpdateAvailableActions();
        
    }

    public void Unselect()
    {
        isSelected = false;
        trans.localScale = Vector3.one;
        marker.gameObject.SetActive(false);
        clickCount = 0;
        playerCanvas.gameObject.SetActive(false);
    }


    public void UpdateAvailableActions()
    {
        int i = 0; 
        Button[] buttons = playerCanvas.GetComponentsInChildren<Button>();
        foreach(Button b in buttons)
        {
            Text t = b.GetComponentInChildren<Text>();
            if(t != null)
            {
                if(actions.ContainsKey(t.text))
                {
                    IPlayerAction action = actions[t.text];
                    //Debug.Log(t.text + " is active: " + action.IsActive());
                    RectTransform rect = b.GetComponent<RectTransform>();
                    float xPos = 50f + (100f * (i));
                    
                    rect.localPosition = new Vector3(xPos, -20,0);
                    //Debug.Log("button " + rect.localPosition.ToString());
                    b.interactable = action.IsActive();
                    i++;
                }
            }
            //if(b.name == "ActionButton")
            //{
            //    b.gameObject.SetActive(false);
            //}
         }
    }


    public void PlayerAction()
    {
        Unselect();

        
        if(savedDirection != Vector3.zero)
        {
            body.AddForce(savedDirection * 100f * savedMagnitude);
            //Debug.Log("Addforce: "+  savedMagnitude);
            savedDirection = Vector3.zero;
            savedMagnitude = 0f;
        }
        
        //Actions
        foreach(string key in actions.Keys)
        {
            IPlayerAction action = actions[key];
            action.ExecuteAction();
        }

        moveLine.enabled = false;
        currentAction = null;
        
    }


    public void CancelAction()
    {
        savedDirection = Vector3.zero;
        savedMagnitude = 0;
        moveLine.enabled = false;
        speedText.text = "";
        foreach (string key in actions.Keys)
        {
            IPlayerAction action = actions[key];
            action.CancelAction();
        }
        skipFrame = true;
        currentAction = null;

    }

    public void SetCurrentAction(string action)
    {
        CancelAction();
        currentAction = action;
        skipFrame = true;
    }

}


