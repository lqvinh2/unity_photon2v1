using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinCoSTanCTan : MonoBehaviour
{
    enum aa{ 
      sin, cos, tan, cotan, moveCircle
    }

    [SerializeField]
    aa SCTCT;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    [SerializeField]
    float amplitude = 3;

    [SerializeField]
    float frequnce = 2;

    // Update is called once per frame
    void Update()
    {
        float x, y, z;

        switch (SCTCT)
        {
            case aa.sin:

                x = transform.position.x;
                y = Mathf.Sin(Time.time * frequnce) * amplitude;
                z = transform.position.z;
                transform.position = new Vector3(x,y,z);
                break;
            case aa.cos:
                x = Mathf.Sin(Time.time * frequnce) * amplitude;
                y = transform.position.y;
                z = transform.position.z;
                transform.position = new Vector3(x, y, z);

                break;
            case aa.tan:

                x = transform.position.x;
                y = Mathf.Tan(Time.time);
                z = transform.position.z;
                transform.position = new Vector3(x, y, z);
                break;
            case aa.cotan:
                break;
            case aa.moveCircle:
                x = Mathf.Cos(Time.time * frequnce) * amplitude;
                y = Mathf.Sin(Time.time * frequnce) * amplitude;
                z = transform.position.z;
                transform.position = new Vector3(x, y, z);
                break;
            default:
                break;
        }
        

        Debug.Log(Time.time);
    }
}
