using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandard : AbstractEnemy {

    public Enemy enemyModel;

    // Use this for initialization
    void Start () {
        target = GameObject.FindWithTag("Player");
        this.acceleration = 0.1f;
        this.velocity = new Vector3(0, 0, 0);
        this.maxSpeed = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 direction = (target.transform.position - transform.position).normalized;
        this.velocity += new Vector3(direction.x * this.acceleration * Time.deltaTime, direction.y * this.acceleration * Time.deltaTime, 0);
        this.velocity = Vector3.ClampMagnitude(this.velocity, this.maxSpeed);
        transform.position += this.velocity;
    }

    void OnCollisionEnter2D(Collision2D coll) {
        Destroy(gameObject);
        target.GetComponent<Player>().OnEnemyEnter(enemyModel.health);
        //Destroy(coll.gameObject);
    }
}
