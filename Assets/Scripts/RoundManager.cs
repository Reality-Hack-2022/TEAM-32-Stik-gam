using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{

    Subscription<PlayerEvents.PlayerDeathEvent> playerDeathSubscription;

    // Start is called before the first frame update
    void Start()
    {
        playerDeathSubscription = EventBus.Subscribe<PlayerEvents.PlayerDeathEvent>(PlayerDied);
    }

    void PlayerDied(PlayerEvents.PlayerDeathEvent e) {
        int playerWhoDied = e.playerID;
        print(playerWhoDied + " just died :(");
    }
}
