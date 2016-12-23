using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public enum gameStatus {
	NEXT, PLAY, GAMEOVER, WIN
}

public class GameManager : Singleton<GameManager> {
	[SerializeField]
	private int totalWaves = 10;
	[SerializeField]
	private Text totalMoneyLbl;
	[SerializeField]
	private Text currentWaveLbl;
	[SerializeField]
	private Text totalEscapeLbl;
	[SerializeField]
	private GameObject spawnPoint;
	[SerializeField]
	private GameObject[] enemies;
	[SerializeField]
	private int totalEnemies = 3;
	[SerializeField]
	private int enemiesPerSpawn;
	[SerializeField]
	private Text playBtnLbl;
	[SerializeField]
	private Button playBtn;

	private int waveNumber = 0;
	private int totalMoney = 10;
	private int totalEscaped = 0;
	private int roundEscaped = 0;
	private int totalKilled = 0;
	private int whichEnemiesToSpawn = 0;
	private gameStatus currentState = gameStatus.PLAY;
	private AudioSource audioSource;
	private int enemiesToSpawn = 0;

	public AudioSource AudioSource {
		get {
			return audioSource;
		}
	}

	public int TotalEscaped {
		get {
			return totalEscaped;
		}

		set {
			totalEscaped = value;
		}
	}

	public int RoundEscaped {
		get {
			return roundEscaped;
		}

		set {
			roundEscaped = value;
		}
	}

	public int TotalKilled {
		get {
			return totalKilled;
		}

		set {
			totalKilled = value;
		}
	}

	public List<Enemy> EnemyList = new List<Enemy>();

	const float spawnDelay = 0.5f;

	public int TotalMoney {
		get {
			return totalMoney;
		}

		set {
			totalMoney = value;
			totalMoneyLbl.text = totalMoney.ToString();
		}
	}

	// Use this for initialization
	void Start () {
		playBtn.gameObject.SetActive(false);
		audioSource = GetComponent<AudioSource>();
		ShowMenu();
	}

	void Update() {
		handleEscape();
	}

	IEnumerator spawn() {
		if(enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies) {
			for(int i = 0; i < enemiesPerSpawn; i++) {
				if(EnemyList.Count < totalEnemies) {
					GameObject newEnemy = Instantiate(enemies[Random.Range(0,2)]) as GameObject;
					newEnemy.transform.position = spawnPoint.transform.position;
					
				}
			}
			yield return new WaitForSeconds(spawnDelay);
			StartCoroutine(spawn());
		}
	}

	public void RegisterEnemy(Enemy enemy) {
		EnemyList.Add(enemy);
	}

	public void UnregisterEnemy(Enemy enemy) {
		EnemyList.Remove(enemy);
		Destroy(enemy.gameObject);
	}

	public void DestroyAllEnemies() {
		foreach (Enemy enemy in EnemyList) {
			Destroy(enemy.gameObject);
		}
		EnemyList.Clear();
	}

	public void AddMoney(int amount) {
		TotalMoney += amount;
	}

	public void SubstractMoney(int amount) {
		TotalMoney -= amount;
	}

	public void IsWaveOver() {
		totalEscapeLbl.text = "Escape " + totalEscaped + "/10";
		// if wave is over
		if((RoundEscaped + totalKilled) == totalEnemies) {
			SetCurrentGameState();
			ShowMenu();
		}
	}

	public void ShowMenu() {
		switch (currentState) {
			case gameStatus.GAMEOVER : 
			playBtnLbl.text = "Play Again"; 
			AudioSource.PlayOneShot(SoundManager.Instance.GameOver);
			break;

			case gameStatus.NEXT : playBtnLbl.text = "Next Wave"; break;
			case gameStatus.PLAY : playBtnLbl.text = "Play"; break;
			case gameStatus.WIN : playBtnLbl.text = "Play"; break;
		}

		playBtn.gameObject.SetActive(true);
	}

	public void PlayBtnPressed() {
		Debug.Log("Pressed Play Btn");

		switch (currentState) {
			case gameStatus.NEXT: 
			waveNumber += 1; 
			totalEnemies += waveNumber; 
			break;

			default : 
			totalEnemies = 3; 
			totalEscaped = 0; 
			totalMoney = 10;
			enemiesToSpawn = 0;
			TowerManager.Instance.DestroyAllTowers();
			TowerManager.Instance.RenameTagsBuildSites();
			totalMoneyLbl.text = totalMoney.ToString();
			totalEscapeLbl.text = "Escaped " + totalEscaped + "/10";
			audioSource.PlayOneShot(SoundManager.Instance.NewGame);
			break;
		}
		DestroyAllEnemies();
		totalKilled = 0;
		roundEscaped = 0;
		currentWaveLbl.text = "Wave " + (waveNumber + 1);
		StartCoroutine(spawn());
		playBtn.gameObject.SetActive(false);
	}

	public void SetCurrentGameState() {
		if(totalEscaped >= 10) {
			currentState = gameStatus.GAMEOVER;
		} else if (waveNumber == 0 && (totalKilled + roundEscaped) == 0) {
			currentState = gameStatus.PLAY;
		} else if (waveNumber >= totalWaves) {
			currentState = gameStatus.WIN;
		} else {
			currentState = gameStatus.NEXT;
		}
	}

	private void handleEscape() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			TowerManager.Instance.disableDragSprite();
			TowerManager.Instance.towerBtnPressed = null;
		}
	}
}
