using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController instance;

	private BallCreator ballCreator;
	private int index;

	private void Awake () {
		MakeSingleton ();
		ballCreator = GetComponent<BallCreator> ();
	}

	private void OnLevelWasLoaded () {
		if (SceneManager.GetActiveScene ().name == "main") {
			CreateBall (index);
		}
	}
	public void SetBallIndex (int index) {
		this.index = index;
	}

	public void CreateBall (int index) {
		ballCreator.CreateBall (index);
	}

	private void MakeSingleton () {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}
}
