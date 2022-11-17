using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movezom : MonoBehaviour
{
    public float speed;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("walk", true);
        float zmove = Time.deltaTime * speed;
        transform.Translate(Vector3.forward * zmove);
    }
}
