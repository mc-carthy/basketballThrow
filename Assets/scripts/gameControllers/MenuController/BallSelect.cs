using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BallSelect : MonoBehaviour {

	private List<Button> buttons = new List<Button>();

	private void Awake () {
		GetButtonsAndAddListeners ();
	}

	public void SelectBall () {
		int index = int.Parse (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

		if (GameController.instance != null) {
			GameController.instance.SetBallIndex (index);
		}
	}

	private void GetButtonsAndAddListeners () {
		GameObject[] btns = GameObject.FindGameObjectsWithTag ("menuBall");

		foreach (GameObject btn in btns) {
			buttons.Add(btn.GetComponent<Button>());
			btn.GetComponent<Button>().onClick.AddListener (() => SelectBall ());
		}
	}
}
