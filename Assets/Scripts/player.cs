using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviourPunCallbacks
{
    private Subscription<PlayerEvents.PlayerGripDown> playerGripDownSubscription;
    private Subscription<PlayerEvents.PlayerGripUp> playerGripUpSubscription;
    private Subscription<PlayerEvents.PlayerTriggerDown> playerTriggerDownSubscription;
    private Subscription<PlayerEvents.PlayerTriggerUp> playerTriggerUpSubscription;
    private Subscription<PlayerEvents.PlayerPrimaryDown> playerPrimaryDowSubscription;

    private IEnumerator RightCoroutine;
    private IEnumerator LeftCoroutine;
    private IEnumerator DrainCoroutine;

    private bool isRightDrawing = false;
    private bool isLeftDrawing = false;

    public float TimeBetweenPointCollection = 0.5f;

    public GameObject LeftHandAnchor;
    public GameObject RightHandAnchor;

    public bool canDraw = true;
    public int playerID = 0;
    public List<Vector3> vectors = new List<Vector3>();

    [SerializeField] GameObject meshSpawner;


    // Start is called before the first frame update
    
    public float inkLevel_Stick = 100.0f;
    public float inkLevel_Blade = 100.0f;
    public float inkMax = 100.0f;
    public float inkDrainAmount = 5.0f;
    public int inkID = 0;
    
    void Start()
    {
        playerGripDownSubscription = EventBus.Subscribe<PlayerEvents.PlayerGripDown>(Grip_performed);
        playerGripUpSubscription = EventBus.Subscribe<PlayerEvents.PlayerGripUp>(Grip_stopped);
        playerTriggerDownSubscription = EventBus.Subscribe<PlayerEvents.PlayerTriggerDown>(Trigger_performed);
        playerTriggerUpSubscription = EventBus.Subscribe<PlayerEvents.PlayerTriggerUp>(Trigger_stopped);
        playerPrimaryDowSubscription = EventBus.Subscribe<PlayerEvents.PlayerPrimaryDown>(Primary_performed);
    }

    private IEnumerator collectCoordsFromHand(bool isLeft, float gapTime)
    {
        while (true)
        { 
            if (isLeft)
            {
                print("Hello from the left coroutiner");

                vectors.Add(LeftHandAnchor.transform.position);
            }
            else
            {
                print("Hello from the right coroutiner");
                vectors.Add(RightHandAnchor.transform.position);
            }
            yield return new WaitForSeconds(gapTime);
        }
    }

    private IEnumerator DrainInk()
    {
        yield return new WaitForSeconds(0.3f);
        if (inkID == 0)
        {
            inkLevel_Stick -= inkDrainAmount;
        }
        else
        {
            inkLevel_Blade -= inkDrainAmount;
        }
        if (inkLevel_Blade <= 0 && inkID == 1)
        {
            if (isRightDrawing)
                StopCoroutine(RightCoroutine);
            if (isLeftDrawing)
                StopCoroutine(LeftCoroutine);
            StopCoroutine(DrainCoroutine);
        }
        else if (inkLevel_Stick <= 0 && inkID == 0)
        {
            if (isRightDrawing)
                StopCoroutine(RightCoroutine);
            if (isLeftDrawing)
                StopCoroutine(LeftCoroutine);
            StopCoroutine(DrainCoroutine);
        }
    }
    private void SpawnTheMesh()
    {
        if (canDraw)
        {
            //GameObject newMeshSpawner = new GameObject("Mesh Spawner");
            //MeshSpawner meshSpawner = newMeshSpawner.AddComponent<MeshSpawner>();
            //print("last vector in the input: " + vectors[vectors.Count -1]);
            //GameObject spawner = Instantiate(meshSpawner, Vector3.zero, Quaternion.identity);
            //photonView.RPC(nameof(CreateSplineMesh), RpcTarget.All, spawner);
            //spawner.GetComponent<MeshSpawner>().CreateSplineMesh(vectors);
            //PhotonNetwork.Instantiate(spawner.name, Vector3.zero, Quaternion.identity, 0);
        }

        vectors = new List<Vector3>();
        
    }

    //[PunRPC]
    //private void CreateSplineMesh(GameObject spawner)
    //{
    //    spawner.GetComponent<MeshSpawner>().CreateSplineMesh(vectors);
    //    vectors = new List<Vector3>();
    //    Debug.Log("called!!!");

    //}
    private void Trigger_performed(PlayerEvents.PlayerTriggerDown e)
    {
        //start a corotine 
        if (canDraw)
        {
            if (e.isLeft)
            {
                print("left trigger");
                LeftCoroutine = collectCoordsFromHand(true, TimeBetweenPointCollection);
                isLeftDrawing = true;
                StartCoroutine(LeftCoroutine);
            }
            else
            {
                print("right trigger");
                RightCoroutine = collectCoordsFromHand(false, TimeBetweenPointCollection);
                isRightDrawing = true;
                StartCoroutine(RightCoroutine);
            }
            DrainCoroutine = DrainInk();
            StartCoroutine(DrainCoroutine);
        }
        
    }

    private void Trigger_stopped(PlayerEvents.PlayerTriggerUp e)
    {
        if (e.isLeft)
        {
            print("Let go of left trigger");
            StopCoroutine(LeftCoroutine);
            SpawnTheMesh();
            isLeftDrawing=false;

        }
        else
        {
            print("Let go of right trigger");
            StopCoroutine(RightCoroutine);

            SpawnTheMesh();
            isRightDrawing=false;
        }
        StopCoroutine(DrainCoroutine);
    }


    private void Primary_performed(PlayerEvents.PlayerPrimaryDown e)
    {
        
        /*print("Let go of left primary");
        Vector3 us = 
            (LeftHandAnchor.transform.position + RightHandAnchor.transform.position) / 2.0f;
        Vector3 them = Vector3.zero; //their controllers centroid
        Vector3 transformationMatrix = Vector3.zero;
        // Ax = B
        // we basically need to get their controllers to line up to ours.
        transformationMatrix.x = them.x / us.x;
        transformationMatrix.y = them.y / us.y;
        transformationMatrix.z = them.z / us.z;
        //now set their location to them * transformationMatrix + some offset. */
        if (inkID > 1) inkID = 0;
        else inkID++;
        
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
