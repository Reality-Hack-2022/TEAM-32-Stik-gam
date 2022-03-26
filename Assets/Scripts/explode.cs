using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour
{
    public GameObject explodePrefab;
    private Subscription<PlayerEvents.PlayerDeathEvent> playerDeathSubscription;


    private void Start()
    {
        playerDeathSubscription = EventBus.Subscribe<PlayerEvents.PlayerDeathEvent>(Death);

    }

    private void Death(PlayerEvents.PlayerDeathEvent e)
    {
        gameObject.transform.GetChild(0).GetComponent<MeshFilter>().mesh = null;
        Instantiate(gameObject, transform);
    }

    
}
