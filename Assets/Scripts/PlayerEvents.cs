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
    public struct PlayerGripDown
    {
        public int playerID;
        public bool isLeft;

        public PlayerGripDown(int playerID, bool isLeft)
        {
            this.playerID = playerID;
            this.isLeft = isLeft;
        }
    }
    public struct PlayerPrimaryDown
    {
        public int playerID;
        public bool isLeft;

        public PlayerPrimaryDown(int playerID, bool isLeft)
        {
            this.playerID = playerID;
            this.isLeft = isLeft;
        }
    }
    
    public struct PlayerToggleDraw
    {
        public int playerID;
        public bool canDraw;
        public bool isLeft;
        public PlayerToggleDraw(int playerID, bool canDraw, bool isLeft)
        {
            this.playerID = playerID;
            this.canDraw = canDraw;
            this.isLeft = isLeft;
        }
    }

    public struct PlayerTriggerDown
    {
        public int playerID;
        public bool isLeft;

        public PlayerTriggerDown(int playerID, bool isLeft)
        {
            this.playerID = playerID;
            this.isLeft = isLeft;
        }
    }

    public struct PlayerTriggerUp
    {
        public int playerID;
        public bool isLeft;

        public PlayerTriggerUp(int playerID, bool isLeft)
        {
            this.playerID = playerID;
            this.isLeft = isLeft;
        }
    }

    public struct PlayerPrimaryUp
    {
        public int playerID;
        public bool isLeft;

        public PlayerPrimaryUp(int playerID, bool isLeft)
        {
            this.playerID = playerID;
            this.isLeft = isLeft;
        }
    }

    public struct PlayerGripUp
    {
        public int playerID;
        public bool isLeft;
        public PlayerGripUp(int playerID, bool isLeft)
        {
            this.playerID = playerID;
            this.isLeft = isLeft;
        }
    }
}
