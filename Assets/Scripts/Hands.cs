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
    public int playerID = -1;
    public bool isLeft = true;
    private List<Vector3> transforms;

    // keep a copy of the executing script
    private IEnumerator coroutine;
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

    public IEnumerator CollectData(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            transforms.Add(transform.position);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isLeft)
        {
            controller.VR.Left_Primary.performed += Primary_performed;

            controller.VR.Left_Trigger.performed += Trigger_performed;
            controller.VR.Left_Trigger.canceled += Trigger_stopped; //decrement this???

            controller.VR.Left_Grip.performed  += Grip_performed;
            controller.VR.Left_Grip.canceled += Grip_stopped; //decrement this???

        }
        else //we're right handed...
        {
            //Grip, Oculus LGrip Trigger or keyboard W
            controller.VR.Right_Grip.performed += Grip_performed;
            controller.VR.Right_Grip.canceled += Grip_stopped;

            controller.VR.Right_Primary.performed += Primary_performed;

            controller.VR.Right_Trigger.performed += Trigger_performed;
            controller.VR.Right_Trigger.canceled += Trigger_stopped; //decrement this???

        }

    }




    private void Trigger_performed(InputAction.CallbackContext obj)
    {
        EventBus.Publish(new PlayerEvents.PlayerTriggerDown(playerID, isLeft));
    }

    private void Trigger_stopped(InputAction.CallbackContext obj)
    {
        EventBus.Publish(new PlayerEvents.PlayerTriggerUp(playerID, isLeft));
    }


    private void Primary_performed(InputAction.CallbackContext obj)
    {
        EventBus.Publish(new PlayerEvents.PlayerPrimaryDown(playerID, isLeft));

    }

    private void Grip_performed(InputAction.CallbackContext obj)
    {
        EventBus.Publish(new PlayerEvents.PlayerGripDown(playerID, isLeft));
    }

    private void Grip_stopped(InputAction.CallbackContext obj)
    {
        EventBus.Publish(new PlayerEvents.PlayerGripUp(playerID, isLeft));
    }

}