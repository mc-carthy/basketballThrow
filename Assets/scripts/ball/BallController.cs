using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BallController : MonoBehaviour {

	public int dots = 30;

	private float power = 2f;
	private float life = 1f;
	private float deadSense = 25f;
	private Vector2 startPos;
	private bool shoot, aiming, hitGround;
	private GameObject dotHolder;
	private List<GameObject> projectilePath = new List<GameObject>();
	private Rigidbody2D rb;
	private Collider2D col;
	private int touchedGround = 0;
	private bool touchedRim;

	private void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		col = GetComponent<Collider2D> ();
	}

	private void Start () {
		dotHolder = GameObject.FindGameObjectWithTag ("dotHolder");
		rb.isKinematic = true;
		col.enabled = false;
		startPos = transform.position;
		projectilePath = dotHolder.transform.Cast<Transform> ().ToList ().ConvertAll (t => t.gameObject);

		for (int i = 0; i < projectilePath.Count; i++) {
			projectilePath [i].GetComponent<Renderer>().enabled = false;
		}
	}

	private void Update () {
		Aim ();

		if (hitGround) {
			life -= Time.deltaTime;
			Color c = GetComponent<Renderer> ().material.GetColor ("_Color");
			GetComponent<Renderer> ().material.SetColor ("_Color", new Color (c.r, c.g, c.b, life));

			if (life <= 0) {
				if (GameController.instance != null) {
					GameController.instance.CreateBall ();
				}
				Destroy (gameObject);
			}
		}
	}

	private void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag == "ground") {
			hitGround = true;
			touchedGround++;

			if (touchedGround <= 3) {
				if (GameController.instance != null) {
					if (Random.Range (0, 2) > 1) { 
						GameController.instance.PlaySound (2);
					} else {
						GameController.instance.PlaySound (3);
					}
				}
			}
		}

		if (col.gameObject.tag == "rim") {
			touchedRim = true;
			if (GameController.instance != null) {
				if (Random.Range (0, 2) > 1) { 
					GameController.instance.PlaySound (1);
				} else {
					GameController.instance.PlaySound (4);
				}
			}
		}

		if (col.gameObject.tag == "backboard") {
			touchedRim = true;
			if (GameController.instance != null) {
				if (Random.Range (0, 2) > 1) { 
					GameController.instance.PlaySound (2);
				} else {
					GameController.instance.PlaySound (3);
				}
			}
		}
	}

	private void OnTriggerEnter2D (Collider2D trig) {
		if (trig.tag == "net") {
			GameController.instance.PlaySound (0);
			if (touchedRim) {
				GameController.instance.IncrementBalls (1);
			} else {
				GameController.instance.IncrementBalls (2);
			}
		}
	}

	private void Aim () {
		if (!shoot) {
			if (Input.GetAxis("Fire1") == 1) {
				if (!aiming) {
					aiming = true;
					startPos = Input.mousePosition;
					CalculatePath ();
					ShowPath ();
				} else {
					CalculatePath ();
				}
			} else if (aiming && !shoot) {
				if (InDeadZone (Input.mousePosition) || InReleaseZone (Input.mousePosition)) {
					aiming = false;
					HidePath ();
					return;
				}
				rb.isKinematic = false;
				col.enabled = true;
				shoot = true;
				aiming = false;
				rb.AddForce (GetForce (Input.mousePosition));
				GameController.instance.DecrementBalls ();
				HidePath ();
			}
		}
	}

	private Vector2 GetForce (Vector3 mouse) {
		return (new Vector2 (startPos.x, startPos.y) - new Vector2 (mouse.x, mouse.y)) * power;
	}

	private bool InDeadZone (Vector2 mouse) {
		return (Vector3.Distance (startPos, mouse) <= deadSense);
	}

	private bool InReleaseZone (Vector2 mouse) {
		return (mouse.x <= 70);
	}

	private void CalculatePath () {
		Vector2 vel = GetForce (Input.mousePosition) * Time.fixedDeltaTime / rb.mass;
		for (int i = 0; i < projectilePath.Count; i++) {
			projectilePath [i].GetComponent<Renderer> ().enabled = true;
			float t = i / 30f;
			Vector3 point = PathPoint (transform.position, vel, t);
			point.z = 1;
			projectilePath [i].transform.position = point;
		}
	}

	private Vector2 PathPoint (Vector2 startPos, Vector2 startVel, float t) {
		return startPos + startVel * t + 0.5f * Physics2D.gravity * t * t;
	}

	private void HidePath () {
		for (int i = 0; i < projectilePath.Count; i++) {
			projectilePath [i].GetComponent<Renderer>().enabled = false;
		}
	}

	private void ShowPath () {
		for (int i = 0; i < projectilePath.Count; i++) {
			projectilePath [i].GetComponent<Renderer>().enabled = true;
		}
	}
}
