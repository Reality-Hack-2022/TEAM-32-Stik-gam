using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int PlayerID = -1;
    public bool Alive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator killme() {
        yield return new WaitForSeconds(1.5f);
        Death();
    }

    public void Death() {
        EventBus.Publish(new PlayerEvents.PlayerDeathEvent(PlayerID));
    }
}
