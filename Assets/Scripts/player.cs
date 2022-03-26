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

    private IEnumerator RightCoroutine;
    private IEnumerator LeftCoroutine;

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

    private void SpawnTheMesh()
    {
        GameObject newMeshSpawner = new GameObject("Mesh Spawner");
        MeshSpawner meshSpawner = newMeshSpawner.AddComponent<MeshSpawner>();
        meshSpawner.CreateSplineMesh(vectors);

        vectors = new List<Vector3>();
        
    }
    private void Trigger_performed(PlayerEvents.PlayerTriggerDown e)
    {
        //start a corotine 
        if (e.isLeft)
        {
            print("left trigger");
            LeftCoroutine = collectCoordsFromHand(e.isLeft, TimeBetweenPointCollection);

            StartCoroutine(LeftCoroutine);

        }
        else
        {
            print("right trigger");
            RightCoroutine = collectCoordsFromHand(e.isLeft, TimeBetweenPointCollection);

            StartCoroutine(RightCoroutine);


        }
    }

    private void Trigger_stopped(PlayerEvents.PlayerTriggerUp e)
    {
        if (e.isLeft)
        {
            print("Let go of left trigger");
            StopCoroutine(LeftCoroutine);
            SpawnTheMesh();

        }
        else
        {
            print("Let go of right trigger");
            StopCoroutine(RightCoroutine);
            SpawnTheMesh();
        }
    }


    private void Primary_performed(PlayerEvents.PlayerPrimaryDown e)
    {
        if (e.isLeft)
        {
            print("Let go of left primary");
            Vector3 us = 
                (LeftHandAnchor.transform.position + RightHandAnchor.transform.position) / 2.0f;
            Vector3 them = Vector3.zero; //their controllers centroid
            Vector3 transformationMatrix = Vector3.zero;
            // Ax = B
            // we basically need to get their controllers to line up to ours.
            transformationMatrix.x = them.x / us.x;
            transformationMatrix.y = them.y / us.y;
            transformationMatrix.z = them.z / us.z;
            //now set their location to them * transformationMatrix + some offset. 
        }
        else
        {
            print("Let go of right primary");

        }
    }

    private void Grip_performed(PlayerEvents.PlayerGripDown e)
    {
        if (e.isLeft)
        {
            print("Left grip");
        }
        else
        {
            print("Right grip");
        }
    }

    private void Grip_stopped(PlayerEvents.PlayerGripUp e)
    {
        if (e.isLeft)
        {
            print("Let go of left grip");

        }
        else
        {
            print("Let go of right grip");

        }
    }
}
