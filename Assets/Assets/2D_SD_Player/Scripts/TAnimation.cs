using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAnimation : MonoBehaviour {

    Animator anim;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        GetRay();
        MoveMouse();

        if (Input.GetKey(KeyCode.A))
        {
            if (anim.GetBool("isAttack") == false)
            {
                anim.SetBool("isAttack", true);
                anim.SetBool("isIdle", false);
            }

        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (anim.GetBool("isHited") == false)
            {
                anim.SetBool("isHited", true);
                anim.SetBool("isIdle", false);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (anim.GetBool("isDie") == false)
            {
                anim.SetBool("isDie", true);
                anim.SetBool("isIdle", false);
            }
        }
        else if (Input.GetKey(KeyCode.F))
        {
            if (anim.GetBool("isRun") == false)
            {
                anim.SetBool("isRun", true);
                anim.SetBool("isIdle", false);
            }
        }
        else if (Input.GetKey(KeyCode.G))
        {
            if (anim.GetBool("isWalk") == false)
            {
                anim.SetBool("isWalk", true);
                anim.SetBool("isIdle", false);
            }
        }
        else
        {
            if (anim.GetBool("isIdle") == false)
            {
                anim.SetBool("isHited", false);
                anim.SetBool("isIdle", true);
                anim.SetBool("isAttack", false);
                anim.SetBool("isWalk", false);
                anim.SetBool("isDie", false);
                anim.SetBool("isRun", false);
            }
        }
    }
    

        void MoveMouse()
        {
            //Gets the world position of the mouse on the screen        
           // Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

       // transform.position = Vector3.Lerp(transform.position, mousePosition, Time.deltaTime);
        }
    RaycastHit hit;
    public Transform GetRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.tag == "Player")
            {
                return hit.transform;
            }
        }
        return null;
    }
}
