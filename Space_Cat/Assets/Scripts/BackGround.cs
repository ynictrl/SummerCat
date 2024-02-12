using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y >= -14)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }else{
            transform.position = new Vector3(0, 14, 1);
        }
    }
}
