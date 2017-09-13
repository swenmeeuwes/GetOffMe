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
            CancelTeleport();
        }
	}
    public void ChannelTeleport() {
        teleporting = true;
        animator.SetBool("isChanneling", teleporting);
        Invoke("Teleport", Mathf.Min(IntervalCheckIfTeleport, 2));
    }
    public void TeleportCheck() {
        var random = Random.Range(1, 100);
        if (random < chanceToTeleport) {
            if(!Dragged)
                ChannelTeleport();
        }
    }
    private void CancelTeleport() {
        CancelInvoke("Teleport");
        teleporting = false;
        animator.SetBool("isChanneling", teleporting);
    }
    private void Teleport() {

        float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);
        Vector3 tmp = new Vector3(Random.Range(0.0f, 2.0f) - 1, Random.Range(0.0f, 2.0f) - 1, 0).normalized * distance;

        CreatePoofCloudOnMe();
        gameObject.transform.position = tmp;
        CreatePoofCloudOnMe();

        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        teleporting = false;

        animator.SetBool("isChanneling", teleporting);
    }
    private void CreatePoofCloudOnMe() {
        var poofClouds = Resources.Load<GameObject>("Enemy/Props/Helmet");
        var cloudObject = Instantiate(poofClouds);

        var parent = new GameObject();
        parent.AddComponent<DeleteObjectDelayed>();
        parent.transform.position = transform.position;

        cloudObject.transform.position = Vector3.zero;
        cloudObject.transform.SetParent(parent.transform);
    }
    public override void OnPlayerHit(Player player)
    {
        player.AbsorbEnemy(model.health);
        base.OnPlayerHit(player);
    }
}
