using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{

    public float speed = 2;
    float turn_smooth_velocity;
    public float turn_smooth_time = 1f;


    public Transform cam;

    Animator anim;
    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        //Time.timeScale = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {

        movement();
    }

    void set_speed(float value)
    {
        if (speed < value)
        {
            speed += 8 * Time.deltaTime;
        }
        else if (speed > value)
        {
            speed -= 10 * Time.deltaTime;
        }
    }


    void movement()
    {
        float Vertical_Input = Input.GetAxis("Vertical");
        float Horizontal_Input = Input.GetAxis("Horizontal");

        Debug.Log(Vertical_Input.ToString());
            

        if (Vertical_Input != 0 || Horizontal_Input != 0)
        {
            if (Input.GetButton("Sprint"))
            {
                set_speed(5.5f);
                if (anim.GetInteger("state") != 3)
                {

                    anim.SetInteger("state", 3);
                }
            }

            else if (Input.GetButton("Walk"))
            {
                set_speed(1.7f);
                if (anim.GetInteger("state") != 2)
                {

                    anim.SetInteger("state", 2);
                }
            }
            else
            {

                set_speed(4.2f);

                if (anim.GetInteger("state") != 1)
                {

                    anim.SetInteger("state", 1);
                }
            }
        }
        else if (Input.GetButton("Vertical") == false && Input.GetButton("Horizontal") == false)
        {
            anim.SetInteger("state", 0);
        }



        Vector3 direction = new Vector3(Horizontal_Input, 0, Vertical_Input).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turn_smooth_velocity, turn_smooth_time);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 movedir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(movedir.normalized * speed * Time.deltaTime);
        }
    }
}