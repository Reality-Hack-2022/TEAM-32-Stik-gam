using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{

    Subscription<PlayerEvents.PlayerDeathEvent> playerDeathSubscription;
    Subscription<PlayerEvents.PlayerToggleDraw> playerToggleDrawSubscription;

    player[] players;
    // Start is called before the first frame update
    void Start()
    {
        playerDeathSubscription = EventBus.Subscribe<PlayerEvents.PlayerDeathEvent>(PlayerDied);
        playerToggleDrawSubscription = EventBus.Subscribe<PlayerEvents.PlayerToggleDraw>(DisablePlayerDrawing);

        players = GameObject.FindObjectsOfType<player>(); 
    }

    void PlayerDied(PlayerEvents.PlayerDeathEvent e) {
        int playerWhoDied = e.playerID;
        print(playerWhoDied + " just died :(");
    }

    void DisablePlayerDrawing(PlayerEvents.PlayerToggleDraw e)
    {
        foreach (player user in players)
        {
            if (e.playerID == user.playerID)
            {
                user.canDraw = false;
            }
        }

    }
}
