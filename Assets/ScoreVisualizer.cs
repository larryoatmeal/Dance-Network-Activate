using UnityEngine;
using System.Collections.Generic;



public class ScoreVisualizer : MonoBehaviour {

	public GameObject goodShit;
	public GameObject perfect;
	public GameObject great;
	public GameObject okay;
	public GameObject coffin;
	public GameObject miss;

	public List<GameObject> prefabs;
	public Dictionary<ScoreLevels, GameObject> scoreToPrefab;

	// Use this for initialization
	void Start () {
		Messenger<ScoreLevels>.AddListener (MessengerKeys.EVENT_SCORE, onScore);

		prefabs = new List<GameObject> () {
			goodShit,
			perfect,
			great,
			okay,
			coffin,
			miss
		};

		scoreToPrefab = new Dictionary<ScoreLevels, GameObject>(){
			{ScoreLevels.GoodShit, goodShit},
			{ScoreLevels.Perfect, perfect},
			{ScoreLevels.Great, great},
			{ScoreLevels.Good, okay},
			{ScoreLevels.Bad, coffin},
			{ScoreLevels.Miss, miss}
		};

		foreach (GameObject prefab in prefabs) {
			if (prefab == null) {
				Debug.LogWarning ("A scoreText prefab is not set");
			} else {
				prefab.SetActive (false);
			}
		}

		if (DEBUG) {
			InvokeRepeating ("test", 2.0f, 4.0f);
		}


	}

	void test(){
		Messenger<ScoreLevels>.Invoke (MessengerKeys.EVENT_SCORE, ScoreLevels.Perfect);
	}

	void Dispose (){
		Messenger<ScoreLevels>.RemoveListener (MessengerKeys.EVENT_SCORE, onScore);
	}
		
	private GameObject activePrefab;
	void hideActivePrefab(){
		activePrefab.SetActive (false);
	}

	public float scaleFactor = 1.5f;
	public float zoomTime = 0.2f;
	public float animationDelay = 0.2f;
	public float liveTime = 2f;
	public bool DEBUG = true;



	int count = 0;

	void onScore(ScoreLevels score){
		count = (count + 0);

		Debug.LogFormat ("Score received {0}", score);

		GameObject scorePrefab = scoreToPrefab [score];
		if (scorePrefab != null) {
			Debug.Log ("Found prefab");

			var scaleFactor = 1.5f;
			var time = 0.2f;



			//cancel outstanding hides
			CancelInvoke ("hideActivePrefab");
			if (activePrefab != null && activePrefab != scorePrefab) {
				activePrefab.SetActive (false);
			}


			activePrefab = scorePrefab;
			scorePrefab.SetActive (true);



			scorePrefab.transform.localScale = new Vector3 (0, 0, 1);

			LeanTween.scaleX (scorePrefab, 1f, 0.05f);
			LeanTween.scaleY (scorePrefab, 1f, 0.05f);


//			LeanTween.scaleX (scorePrefab, scaleFactor, time).setDelay (animationDelay)
//				.setOnComplete (() => LeanTween.scaleX (scorePrefab, 1.0f, time));
//			LeanTween.scaleY (scorePrefab, scaleFactor, time).setDelay (animationDelay)
//				.setOnComplete (() => LeanTween.scaleY (scorePrefab, 1.0f, time));
//			
			Invoke ("hideActivePrefab", liveTime);


//			LeanTween.scaleY (scorePrefab, scaleFactor, 0.1f);





//			LeanTween.move(scorePrefab, 
//				new Vector2(1, 1),
//				1.0f
//				);
		} else {
			Debug.LogWarningFormat ("Score {0} has no associated prefab", score);
		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}
