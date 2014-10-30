using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float scaleHealth = 0.0f;
		//Check if parent is an Enemy or Player, to know which script to get values from
		if (transform.parent != null && transform.parent.tag == "Enemy")
			scaleHealth = GetComponentInParent<Enemy> ().GetHealth () / GetComponentInParent<Enemy> ().maxHealth;
		else if(transform.parent != null && transform.parent.tag == "Player")
			scaleHealth = GetComponentInParent<Player> ().GetHealth () / GetComponentInParent<Player> ().maxHealth;

		float r = Mathf.Min(2 * (1 - scaleHealth), 1f);
		float g = Mathf.Min(2 * scaleHealth, 1f);

		//float r = Mathf.Min(scaleHealth - 0.5f, 0.5f) * 2f;
		GetComponentsInChildren<MeshRenderer> ()[1].material.color = new Color (r, g, 0); // Indicator
		GetComponentsInChildren<Transform>()[1].localScale = new Vector3(scaleHealth, 1f, 0f);
		GetComponentsInChildren<Transform> () [1].localPosition = new Vector3(0.5f*scaleHealth-0.5f, 0f, 0f);
		//GetComponent <MeshRenderer> ().material.color = new Color (0, 0, 0);
	}
}
