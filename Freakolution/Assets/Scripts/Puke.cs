using UnityEngine;
using System.Collections;

public class Puke : MonoBehaviour {

	public Chemicals pukeChemicals;
	public float damage;
	public Player player;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetPlayer( Player p) {
		player = p;
	}
	public void SetPukeChemicals(Chemicals c) {
		pukeChemicals = c;
	}
	public void SetDamage(float d) {
		damage = d;
	}

	void OnParticleCollision(GameObject other) {
		if (other.tag == "Enemy") {
			Enemy enemy = other.GetComponent<Enemy>();
			enemy.LoseHealth(damage, pukeChemicals, player);
			//Destroy(other);// body.AddForce(direction * 5);
		}
	}
}
