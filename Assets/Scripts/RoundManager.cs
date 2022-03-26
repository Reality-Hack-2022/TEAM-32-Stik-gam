using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    int playerOneScore = 0;
    int playerTwoScore = 0;
    public float PrepTime = 30f;
    public float TimeBetweenRounds = 10f;
    PlayerHealth[] PlayerHealths;
    Subscription<PlayerEvents.PlayerDeathEvent> playerDeathSubscription;
    Subscription<PlayerEvents.PlayerToggleDraw> playerToggleDrawSubscription;
    Subscription<PlayerEvents.PlayerPrimaryDown> playerPressPrimaryDownSubscription;
    public Sprite count_3;
    public Sprite count_2;
    public Sprite count_1;
    public Sprite fight_sprite;
    public Sprite draw_sprite;
    //public GameObject win_UI;
    player[] players;
    List<int> ReadyPlayers;

    void Start()
    {
        playerDeathSubscription = EventBus.Subscribe<PlayerEvents.PlayerDeathEvent>(PlayerDied);
        PlayerHealths = GameObject.FindObjectsOfType<PlayerHealth>(); // Change this from start to whenever both players have joined
        
        playerToggleDrawSubscription = EventBus.Subscribe<PlayerEvents.PlayerToggleDraw>(DisablePlayerDrawing);
        playerPressPrimaryDownSubscription = EventBus.Subscribe<PlayerEvents.PlayerPrimaryDown>(PlayerPressStart);
        players = GameObject.FindObjectsOfType<player>();

        // Only the largest Round Manager should survive so it can manage all players!
        RoundManager[] r_managers = GameObject.FindObjectsOfType<RoundManager>();
        int largest_rm = 0;
        // Get most populated Round Manager and delete the rest
        for (int r = 0; r < r_managers.Length; ++r)
        {
            if (r_managers[r].players.Length > r_managers[largest_rm].players.Length) {
                largest_rm = r;
            }
        }
        for (int r = 0; r < r_managers.Length; ++r) {
            if (r != largest_rm) {
                StartCoroutine(r_managers[r].SelfDestruct());
            }
        }

        //Assign player_ids
        for (int i = 0; i < PlayerHealths.Length; ++i) {
            PlayerHealths[i].PlayerID = i;
            players[i].playerID = i;
        }
        
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
        EventBus.Publish(new PlayerEvents.PlayerToggleDraw(0, false, false));
        EventBus.Publish(new PlayerEvents.PlayerToggleDraw(1, false, false));

        // Show winning player that they won
        
        // Show losing player they lost :(

        // Start next round
        StartCoroutine(StartRound());
    }

    IEnumerator StartRound() {
        yield return new WaitForSeconds(TimeBetweenRounds);
        // Preparation phase
        // Notify players of new phase
        StartCoroutine(PrepPhaseUIShow());
        // Reenable player abilities to draw
        EventBus.Publish(new PlayerEvents.PlayerToggleDraw(0, true, false));
        EventBus.Publish(new PlayerEvents.PlayerToggleDraw(1, true, false));
        yield return new WaitForSeconds(PrepTime);

        // Battle phase
        StartCoroutine(BattlePhaseUIShow());
        
    }

    IEnumerator PrepPhaseUIShow() {
        GameObject countDownObj = new GameObject();
        SpriteRenderer seven_up = countDownObj.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        seven_up.sprite = draw_sprite;
        yield return new WaitForSeconds(PrepTime - 3);
        // Count down from 3 with UI
        seven_up.sprite = count_3;
        yield return new WaitForSeconds(1f);
        seven_up.sprite = count_2;
        yield return new WaitForSeconds(1f);
        seven_up.sprite = count_1;
        yield return new WaitForSeconds(1f);
        Destroy(countDownObj);
    }

    IEnumerator BattlePhaseUIShow() {
        // Create object to display fight sprite
        GameObject fightObj = new GameObject();
        SpriteRenderer fight_sprite_scene = fightObj.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        fight_sprite_scene.sprite = fight_sprite;
        yield return new WaitForSeconds(3f);
        Destroy(fightObj);

        // Reenable player damage
        foreach (PlayerHealth h in PlayerHealths)
        {
            h.Alive = true;
        }
    }

    IEnumerator Results(int winner) {
        yield return new WaitForSeconds(3f);
    }

    void DisablePlayerDrawing(PlayerEvents.PlayerToggleDraw e)
    {
        foreach (player user in players)
        {
            if (e.playerID == user.playerID)
            {
                user.canDraw = e.canDraw;
            }
        }

    }

    void PlayerPressStart(PlayerEvents.PlayerPrimaryDown e) {
        int playerID = e.playerID;
        if (!ReadyPlayers.Contains(playerID)) {
            ReadyPlayers.Add(playerID);
            if (ReadyPlayers.Count >= 2) {
                StartCoroutine(StartRound());
            }
        }
    }

    IEnumerator SelfDestruct() {
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
}
