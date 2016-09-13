using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameplayController : MonoBehaviour {

	public void BackToMenu () {
		SceneManager.LoadScene ("menu", LoadSceneMode.Single);
	}
}
