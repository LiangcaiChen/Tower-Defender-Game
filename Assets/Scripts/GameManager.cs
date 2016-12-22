using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {
	public GameObject spawnPoint;
	public GameObject[] enemies;
	public int maxEnemiesOnScreen;
	public int totalEnemies;
	public int enemiesPerSpawn;

	private int enemiesOnScreen = 0;
	const float spawnDelay = 0.5f;

	// Use this for initialization
	void Start () {
		StartCoroutine(spawn());
	}

	IEnumerator spawn() {
		if(enemiesPerSpawn > 0 && enemiesOnScreen < totalEnemies) {
			for(int i = 0; i < enemiesPerSpawn; i++) {
				if(enemiesOnScreen < maxEnemiesOnScreen) {
					GameObject newEnemy = Instantiate(enemies[1]) as GameObject;
					newEnemy.transform.position = spawnPoint.transform.position;
					enemiesOnScreen += 1;
				}
			}
			yield return new WaitForSeconds(spawnDelay);
			StartCoroutine(spawn());
		}
	}

	public void removeEnemyFromScreen() {
		if(enemiesOnScreen > 0) {
			enemiesOnScreen -= 1;
		}
	}

}
