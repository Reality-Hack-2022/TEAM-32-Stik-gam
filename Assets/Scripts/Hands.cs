using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using TMPro; // Add the TextMesh Pro namespace to access the various functions.
using System.Linq;

public class HandAnim : MonoBehaviour
{
    private Controls controller;
    public Animator m_animator = null;
    public bool isLeft = true;

    private void Awake()
    {
        controller = new Controls();
    }

    private void OnEnable()
    {
        controller.Enable();
    }

    private void OnDisable()
    {
        controller.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isLeft)
        {
            controller.VR.Left_Primary.performed += Primary_performed;
            controller.VR.Left_Primary.canceled -= Primary_performed; //decrement this???


            controller.VR.Left_Trigger.performed += Trigger_performed;
            controller.VR.Left_Trigger.canceled -= Trigger_performed; //decrement this???


            //Gas, Oculus LTrigger or keyboward S
            controller.VR.Left_Grip.performed  += Grip_performed;
            controller.VR.Left_Grip.canceled -= Grip_performed; //decrement this???

        }
        else //we're right handed...
        {
            //Grip, Oculus LGrip Trigger or keyboard W
            controller.VR.Right_Grip.performed += Grip_performed;
            controller.VR.Right_Grip.canceled -= Grip_performed;

            controller.VR.Right_Primary.performed += Primary_performed;
            controller.VR.Right_Primary.canceled -= Primary_performed; //decrement this???


            controller.VR.Right_Trigger.performed += Trigger_performed;
            controller.VR.Right_Trigger.canceled -= Trigger_performed; //decrement this???

        }

    }

    private void Trigger_performed(InputAction.CallbackContext obj)
    {

    }
    private void Primary_performed(InputAction.CallbackContext obj)
    {

    }



    private void Grip_performed(InputAction.CallbackContext obj)
    {

    }

}