using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    int playerOneScore = 0;
    int playerTwoScore = 0;
    public float PrepTime = 30f;
    PlayerHealth[] PlayerHealths;
    Subscription<PlayerEvents.PlayerDeathEvent> playerDeathSubscription;
    Subscription<PlayerEvents.PlayerToggleDraw> playerToggleDrawSubscription;
    public Sprite count_3;
    public Sprite count_2;
    public Sprite count_1;

    player[] players;
    // Start is called before the first frame update
    void Start()
    {
        playerDeathSubscription = EventBus.Subscribe<PlayerEvents.PlayerDeathEvent>(PlayerDied);
        PlayerHealths = GameObject.FindObjectsOfType<PlayerHealth>(); // Change this from start to whenever both players have joined
        StartCoroutine(PrepPhaseUIShow());
        playerToggleDrawSubscription = EventBus.Subscribe<PlayerEvents.PlayerToggleDraw>(DisablePlayerDrawing);

        players = GameObject.FindObjectsOfType<player>(); 
    }

    void PlayerDied(PlayerEvents.PlayerDeathEvent e) {
        // Get who died
        int playerWhoDied = e.playerID;
        print(playerWhoDied + " just died :(");

        // Increment score of killer
        if (playerWhoDied == 1)
        {
            playerTwoScore += 1;
        }
        else {
            playerOneScore += 1;
        }

        // Disable drawing abilities of both Players

        // Start next round
        StartCoroutine(StartRound());
    }

    IEnumerator StartRound() {
        // Preparation phase
        // Notify players of new phase
        StartCoroutine(PrepPhaseUIShow());
        // Reenable player abilities to draw
        yield return new WaitForSeconds(PrepTime);

        // Battle phase
        BattlePhaseUIShow();
        // Reenable player damage
        foreach (PlayerHealth h in PlayerHealths) {
            h.Alive = true;
        }
    }

    IEnumerator PrepPhaseUIShow() {
        //yield return new WaitForSeconds(PrepTime - 3);
        // Count down from 3 with UI
        GameObject countDownObj = new GameObject();
        SpriteRenderer seven_up = countDownObj.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        seven_up.sprite = count_3;
        yield return new WaitForSeconds(1f);
        seven_up.sprite = count_2;
        yield return new WaitForSeconds(1f);
        seven_up.sprite = count_1;
        yield return new WaitForSeconds(1f);
        Destroy(countDownObj);
    }

    void BattlePhaseUIShow() { 
    
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
