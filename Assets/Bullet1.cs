using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    bool isFire;
    Vector3 direction;
    public float speed = 10f;
    public GameObject turret;

    public void Fire(Vector3 dir)
    {
        direction = dir;
        isFire = true;
        Destroy(gameObject, 10f);
    }
    // Start is called before the first frame update
    void Start()
    {
        // GetComponent<Rigidbody>().AddForce(transform.forward *Speed);
        //GetComponent<Rigidbody>().AddForce(transform.forward * Speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            transform.Translate(direction * Time.deltaTime * speed);
        }
    }

}
