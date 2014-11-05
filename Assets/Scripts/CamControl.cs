using UnityEngine;
using System.Collections;

public class CamControl : MonoBehaviour 
{

    public float hSpeed = 10f;
    public float vSpeed = 10f;

    private Transform trans;



    void Awake()
    {
        trans = transform;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 angles = trans.rotation.eulerAngles;

        if(angles.y > 100f)
        {
            v = -v;
            h = -h;
        }

        trans.Translate(Vector3.right * h * Time.deltaTime * hSpeed,Space.World);

        trans.Translate(Vector3.forward * v * Time.deltaTime * vSpeed, Space.World);


        
    }


}
