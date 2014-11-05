using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerInput))]
public class Blocker : MonoBehaviour, IPlayerAction 
{

    public float sizeMultiplier = 4;
    public float duration = 5f;

    private Vector3 blockScale = Vector3.zero;
    private Vector3 originalScale;

    private float originalRadius;


    private Transform trans;
    private SphereCollider collider;

	void Start () 
    {
        GetComponent<PlayerInput>().AddAction("BLOCK", this);
        trans = GetComponentInChildren<MeshRenderer>().transform;
        collider = GetComponent<SphereCollider>();
        if (collider == null)
        {
            this.enabled = false;
        }
        else
        {
            originalRadius = collider.radius;
        }
        originalScale = trans.localScale;
	}



    void IPlayerAction.BuildAction()
    {
        //TODO: adjust mass, friction ...
        if(originalRadius == collider.radius)
        {
            collider.radius = originalRadius * sizeMultiplier;
            trans.localScale = new Vector3(originalScale.x * sizeMultiplier, originalScale.y, originalScale.z * sizeMultiplier);
        }

    }

    bool IPlayerAction.IsActive()
    {
        return true;
    }


    bool IPlayerAction.ExecuteAction()
    {

        Invoke("BackToNormal", duration);
        return true;
    }

    void IPlayerAction.CancelAction()
    {
        BackToNormal();
    }


    void BackToNormal()
    {
        collider.radius = originalRadius;
        trans.localScale = originalScale;
    }
	
}
