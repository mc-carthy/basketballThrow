using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour {

	private Animator menuAnim, ballAnim;

	private void Awake () {
		menuAnim = GameObject.FindGameObjectWithTag ("menuAnim").GetComponent<Animator> ();
		ballAnim = GameObject.FindGameObjectWithTag ("ballAnim").GetComponent<Animator> ();
	}

	public void PlayGame () {
		SceneManager.LoadScene ("main", LoadSceneMode.Single);
	}

	public void SelectBall () {
		menuAnim.Play ("slideOut");
		ballAnim.Play ("slideIn");
	}

	public void BackToMenu () {
		menuAnim.Play ("slideIn");
		ballAnim.Play ("slideOut");
	}
}
