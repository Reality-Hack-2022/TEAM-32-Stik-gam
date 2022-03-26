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

    private IEnumerator coroutine;
    public float TimeBetweenPointCollection = 0.5f;

    public GameObject LeftHandAnchor;
    public GameObject RightHandAnchor;
    
    public List<Vector3> vectors = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        playerGripDownSubscription = EventBus.Subscribe<PlayerEvents.PlayerGripDown>(Grip_performed);
        playerGripUpSubscription = EventBus.Subscribe<PlayerEvents.PlayerGripUp>(Grip_stopped);
        playerTriggerDownSubscription = EventBus.Subscribe<PlayerEvents.PlayerTriggerDown>(Trigger_performed);
        playerTriggerUpSubscription = EventBus.Subscribe<PlayerEvents.PlayerTriggerUp>(Trigger_stopped);
        playerPrimaryDowSubscription = EventBus.Subscribe<PlayerEvents.PlayerPrimaryDown>(Primary_performed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator collectCoordsFromHand(bool isLeft, float gapTime)
    {
        yield return new WaitForSeconds(gapTime);
        
        if (isLeft)
        {
            vectors.Add(LeftHandAnchor.transform.position);
        }
        else
        {
            vectors.Add(RightHandAnchor.transform.position);
        }
    }
    private void Trigger_performed(PlayerEvents.PlayerTriggerDown e)
    {
        //start a corotine 
        coroutine = collectCoordsFromHand(e.isLeft, TimeBetweenPointCollection);
        StartCoroutine(coroutine);
    }

    private void Trigger_stopped(PlayerEvents.PlayerTriggerUp e)
    {
        StopCoroutine(coroutine);
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
