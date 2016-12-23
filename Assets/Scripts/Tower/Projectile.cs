using UnityEngine;

public enum projecttileType {
	ROCK, ARROW, FIREBALL
};

public class Projectile : MonoBehaviour {

	[SerializeField]
	private int attackStrength;

	[SerializeField]
	private projecttileType pT;

	public int AttackStrength {
		get {
			return attackStrength;
		}
	}
	
	public projecttileType PT {
		get {
			return pT;
		}
	}
}
