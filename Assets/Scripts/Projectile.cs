using UnityEngine;

public enum projecttileType {
	ROCK, ARROW, FIREBALL
};

public class Projectile : MonoBehaviour {

	[SerializeField]
	private int attactStrength;

	[SerializeField]
	private projecttileType pT;

	public int AttactStrength {
		get {
			return attactStrength;
		}
	}
	
	public projecttileType PT {
		get {
			return pT;
		}
	}
}
