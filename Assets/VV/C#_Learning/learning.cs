using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class learning : MonoBehaviour
{

    [SerializeField]
    Transform enemy;

    [SerializeField]
    float move_speed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var direction = enemy.position - transform.position;
        
        // assume that we have : direction x = 4, y = 5 => a = 1.25
        float atan2 = Mathf.Atan2(direction.y, direction.x);
        float angle = atan2 * Mathf.Rad2Deg - 90;

        Debug.DrawRay(transform.position, direction, Color.green);
    
        //Debug.Log(atan2);
        //Debug.Log(angle);

        Quaternion angleAxist = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, angleAxist, Time.deltaTime * 2);

        //var direction = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)) - transform.position;
        //transform.eulerAngles = Vector3.forward * angle;
        direction.Normalize();
        Debug.DrawRay(transform.position, direction, Color.red);
        transform.Translate(direction * Time.deltaTime * move_speed);

    }
}
