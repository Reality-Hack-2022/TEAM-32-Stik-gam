using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerEvents : MonoBehaviour
{
    public struct PlayerDeathEvent {
        public int playerID;

        public PlayerDeathEvent(int playerID) {
            this.playerID = playerID;
        }
    }
}
