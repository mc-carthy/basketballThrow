using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController instance;

	private BallCreator ballCreator;
	private int index;
	private AudioSource source;
	[SerializeField]
	private AudioClip rimHit0, rimHit1, bounce0, bounce1, net;
	private float volume = 1.0f;
	private int balls = 10;
	private Text ballText;

	private void Awake () {
		MakeSingleton ();
		ballCreator = GetComponent<BallCreator> ();
		source = GetComponent<AudioSource> ();
	}

	private void OnLevelWasLoaded () {
		if (SceneManager.GetActiveScene ().name == "main") {
			CreateBall ();
			ballText = GameObject.FindGameObjectWithTag ("ballText").GetComponent<Text> ();
			ballText.text = "Balls: " + balls.ToString ();
		}
	}
	public void SetBallIndex (int index) {
		this.index = index;
	}

	public void CreateBall () {
		ballCreator.CreateBall (index);
	}

	public void IncrementBalls (int increment) {
		balls += increment;
		if (balls > 10) {
			balls = 10;
		}
		ballText.text = "Balls: " + balls.ToString ();
	}

	public void DecrementBalls () {
		balls--;
		ballText.text = "Balls: " + balls.ToString ();
	}

	public void PlaySound (int id) {
		switch (id) {
		case 0:
			source.PlayOneShot (net, volume);
			break;
		case 1:
			if (Random.Range (0, 2) > 1) {
				source.PlayOneShot (rimHit0, volume);
			} else {
				source.PlayOneShot (rimHit1, volume);
			}			
			break;
		case 2:
			if (Random.Range (0, 2) > 1) {
				source.PlayOneShot (bounce0, volume);
			} else {
				source.PlayOneShot (bounce1, volume);
			}
			break;
		case 3:
			if (Random.Range (0, 2) > 1) {
				source.PlayOneShot (bounce0, volume / 2);
			} else {
				source.PlayOneShot (bounce1, volume / 2);
			}			
			break;
		case 4:
			if (Random.Range (0, 2) > 1) {
				source.PlayOneShot (rimHit0, volume / 2);
			} else {
				source.PlayOneShot (rimHit1, volume / 2);
			}					
			break;
		}
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
