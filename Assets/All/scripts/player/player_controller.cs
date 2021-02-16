using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{

    public float speed = 2;
    float turn_smooth_velocity;
    public float turn_smooth_time = 1f;
    float max_speed_wait = 0;

    public float walk_speed;
    public float run_speed;
    public float sprint_speed;

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
        aiming();
    }

    void set_speed(float value)
    {
        if (speed < value)
        {
            speed += 13 * Time.deltaTime;
          
        }
        else if (speed > value)
        {
            speed -= 15 * Time.deltaTime;
        }
        if (anim.GetFloat("movement") != speed) { 
            anim.SetFloat("movement", speed);
        }
    }


    void movement()
    {
        float Vertical_Input = Input.GetAxis("Vertical");
        float Horizontal_Input = Input.GetAxis("Horizontal");

        if (turn_smooth_time != 0.1f)
        {
            turn_smooth_time = 0.1f;
        }

     
        if (Vertical_Input != 0 || Horizontal_Input != 0)
        {
            anim.SetInteger("state", 1);
            if (Input.GetButton("Sprint"))
            {
               
                  set_speed(sprint_speed);

             

            }

            else if (Input.GetButton("Walk"))
            {
                set_speed(walk_speed);
             
            }
            else
            {

                set_speed(run_speed);

             
            }
        }
        else if (Input.GetButton("Vertical") == false && Input.GetButton("Horizontal") == false)
        {
            set_speed(0f);
            anim.SetInteger("state", 0);
        }



        Vector3 direction = new Vector3(Horizontal_Input, 0, Vertical_Input).normalized;

    
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turn_smooth_velocity, turn_smooth_time);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 movedir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(movedir.normalized * speed * Time.deltaTime);
        
    }

    void aiming()
    {
        if (turn_smooth_time != 0.05f)
        {
            turn_smooth_time = 0.05f;
        }

        if (Input.GetButton("Fire2"))
        {
            anim.SetInteger("state", 2);
        }
    }


}