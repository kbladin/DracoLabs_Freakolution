using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	private GameObject[] players;
	// Use this for initialization
	void Start () {
		//players = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		players = GameObject.FindGameObjectsWithTag("Player");

		// Update all sprites
		GameObject[] spriteGos = GameObject.FindGameObjectsWithTag("Sprite");
		foreach (GameObject SpriteGo in spriteGos) {
			SpriteGo.transform.LookAt(SpriteGo.transform.position + transform.rotation * Vector3.forward,
			                 transform.rotation * Vector3.up);
		}

		// Update camera position
		Vector3 meanPos = new Vector3(0,0,0);
		foreach (GameObject player in players) {
			if (player.GetComponent<Player>().Alive)
			meanPos += player.transform.position;
		}
		meanPos /= players.Length;

		Vector3 relativeCamPosition = new Vector3 (0, 3f, -5f);

		Vector3 goalPosition = meanPos + relativeCamPosition;
		goalPosition += MaxDistance (players) * 0.12f * (goalPosition - meanPos);

		transform.position = Delay(transform.position,
		                           goalPosition,
		                           0.03f);

		/*transform.LookAt (Delay(transform.position - relativeCamPosition,
		                        relativeCamPosition,
		                        0.1f));*/
	}

	private float MaxDistance(GameObject[] gos){
		float maxDistance = 0;
		foreach (GameObject Go1 in gos) {
			foreach (GameObject Go2 in gos) {
				float tmpDistance = (Go1.transform.position - Go2.transform.position).magnitude;
				maxDistance = (tmpDistance > maxDistance ? 
				               tmpDistance : maxDistance);
			}
		}
		return maxDistance;
	}

	private float Delay (float x, float goal, float speed) {
		return x + speed * (goal - x);
	}

	private Vector3 Delay (Vector3 x, Vector3 goal, float speed) {
		return x + speed * (goal - x);
	}
}
