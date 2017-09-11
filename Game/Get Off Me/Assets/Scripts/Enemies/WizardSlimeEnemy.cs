using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSlimeEnemy : SeekingEntity {

    private bool teleporting = false;
    public float chanceToTeleport;
    public float IntervalCheckIfTeleport;
	// Use this for initialization
	protected override void Start () {
        base.Start();
        InvokeRepeating("TeleportCheck", IntervalCheckIfTeleport, IntervalCheckIfTeleport);
    }
	
	// Update is called once per frame
	protected override void UpdateEntity () {
        if (!teleporting) {
            base.UpdateEntity();
            return;
        }
        if (Dragged) {
            CancelInvoke("Teleport");
            teleporting = false;
        }
	}
    public void ChannelTeleport() {
        teleporting = true;
        Invoke("Teleport", Mathf.Min(IntervalCheckIfTeleport, 2));
    }
    public void TeleportCheck() {
        var random = Random.Range(1, 100);
        Debug.Log(random);
        if (random < chanceToTeleport) {
            if(!Dragged)
                ChannelTeleport();
        }
    }
    private void Teleport() {
        float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);
        Vector3 tmp = new Vector3(Random.Range(0.0f, 2.0f) - 1, Random.Range(0.0f, 2.0f) - 1, 0).normalized * distance;
        gameObject.transform.position = tmp;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        teleporting = false;
    }
    public override void OnPlayerHit(Player player)
    {
        player.AbsorbEnemy(model.health);
        base.OnPlayerHit(player);
    }
}
