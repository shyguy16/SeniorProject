﻿using UnityEngine;
using System.Collections;

public class VrHand : MonoBehaviour
{
    private SteamVR_TrackedController Controller;//used for controller update
    private float ProjectileSpeed;
    private float FlySpeed;
    private float MaxSpeed;
    private Rigidbody Rb;

    // Use this for initialization
    private void Start()
    {
        Controller = GetComponent<SteamVR_TrackedController>();
        Controller.PadClicked += Attack;
        Controller.MenuButtonClicked += Pause; //add Pause menu here
        ProjectileSpeed = VrPlayer.Instance.ProjectileSpeed;
        FlySpeed = VrPlayer.Instance.FlySpeed;
        MaxSpeed = VrPlayer.Instance.MaxSpeed;
        Rb = VrPlayer.Instance.Rb;

    }

    void Attack(object sender, ClickedEventArgs e)
    {
        if (VrPlayer.Instance.Shootable())
        {
            GameObject g = Manager.Instance.GetBullet();//small possiblility to get rid of ammo without spawning
            if(g != null)
            {
                g.GetComponent<Bullet>().Reset();
                g.transform.position = this.transform.position; //this might wanna make an empty object infront of controller or with an offset

                //Gives bullets funky rotations, FIX???
                g.transform.rotation = new Quaternion(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);
                g.GetComponent<Rigidbody>().AddForce(new Vector3(this.transform.forward.x, this.transform.forward.y, this.transform.forward.z) * ProjectileSpeed, ForceMode.Impulse);//needs to be tested!!!
            }
            else
                Debug.Log("NOT ENOUGH BULLETS");
        }
    }

    void Pause(object sender, ClickedEventArgs e)
    {
        Debug.Log("FOUND");
        Manager.Instance.TogglePause();
    }

    void Fly()
    {
        //float triggerpress = SteamVR_Controller.Input((int)Controller.controllerIndex).hairTriggerDelta;
        if (Rb.velocity.magnitude < MaxSpeed)
            Rb.AddForce(-Controller.transform.forward * FlySpeed);// * triggerpress);//needs drag force
        SteamVR_Controller.Input((int)Controller.controllerIndex).TriggerHapticPulse();
        ;
    }



    // Update is called once per frame
    void Update()
    {
        if(Manager.Instance.IsUpdatable())
        {
            if(Controller.triggerPressed)
            {
                Fly();
            }
        }
    }
}
