using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private Subscription<PlayerEvents.PlayerGripDown> playerGripDownSubscription;
    private Subscription<PlayerEvents.PlayerGripUp> playerGripUpSubscription;
    private Subscription<PlayerEvents.PlayerTriggerDown> playerTriggerDownSubscription;
    private Subscription<PlayerEvents.PlayerTriggerUp> playerTriggerUpSubscription;
    private Subscription<PlayerEvents.PlayerPrimaryDown> playerPrimaryDowSubscription;
    private Subscription<PlayerEvents.PlayerPrimaryUp> playerPrimaryUpSubscription;

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void Trigger_performed(PlayerEvents.PlayerTriggerDown e)
    {
        if(e.isLeft)
        {

        }
        else
        {

        }
    }

    private void Trigger_stopped(PlayerEvents.PlayerTriggerUp e)
    {
        if (e.isLeft)
        {

        }
        else
        {

        }
    }


    private void Primary_performed(PlayerEvents.PlayerPrimaryDown e)
    {
        if (e.isLeft)
        {

        }
        else
        {

        }
    }

    private void Grip_performed(PlayerEvents.PlayerGripDown e)
    {
        if (e.isLeft)
        {

        }
        else
        {

        }
    }

    private void Grip_stopped(PlayerEvents.PlayerGripUp e)
    {
        if (e.isLeft)
        {

        }
        else
        {

        }
    }
}
