using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	[SerializeField]
	private Transform exitPoint;
	[SerializeField]
	private Transform[] waypoints;
	[SerializeField]
	private float navigationUpdate;

	private int target = 0;
	private Transform enemy;
	private float navigationTime = 0;

	// Use this for initialization
	void Start () {
		enemy = GetComponent<Transform> ();
		GameManager.Instance.RegisterEnemy(this);
	}
	
	// Update is called once per frame
	void Update () {
		if(waypoints != null) {
			navigationTime += Time.deltaTime;
			if(navigationTime > navigationUpdate) {
				if(target < waypoints.Length) {
					enemy.position = Vector2.MoveTowards(enemy.position, waypoints[target].position, navigationTime);
				} else {
					enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navigationTime);
				}
				navigationTime = 0;
			}
		}
	}

	
	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "CheckPoint") {
			target += 1;
		} else if (other.tag == "Finish") {
			GameManager.Instance.UnregisterEnemy(this);
		}
	}

	
}
