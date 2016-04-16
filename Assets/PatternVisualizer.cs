using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PatternVisualizer : MonoBehaviour {

	public Pads pads;
	float padPosition;
	float bottomPosition;
	Vector3 bottomLeftCorner;
	Vector3 topRightCorner;
	public PatternMaster patternMaster;
	public TimeMaster timeMaster;

	float velocity = 0.2f;
	private float scale;
	public int ttl = 100;
	Pad[] padList;
//	List<MusicEvent> musicEvents = new List<MusicEvent> ();

//	List<MusicEvent> musicEventsToRemove = new List<MusicEvent>();

	Dictionary<MusicEvent, GameObject> musicEvents = new Dictionary<MusicEvent, GameObject> ();

	private int latencyAdjustment;
	List<GameObject> destoryQueue = new List<GameObject>();
	// Use this for initialization
	void Start () {
		Messenger<MusicEvent>.AddListener (MessengerKeys.EVENT_VANISH, removeMusicEvent);
		Messenger<MusicEvent>.AddListener (MessengerKeys.EVENT_NO_LONGER_ACTIVE, disableEvent);


		bottomLeftCorner = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, Camera.main.nearClipPlane));
		topRightCorner = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, Camera.main.nearClipPlane));

		bottomPosition = bottomLeftCorner.y;
		padPosition = pads.transform.position.y;


		padList = pads.GetComponentsInChildren<Pad> ();
		Debug.Log (padList.Length);
		scale = GameManager.Instance.lookAhead;
		latencyAdjustment = PlayerPrefs.GetInt (PlayerPrefKeys.AudioLatencyOffset);
//		Debug.Log (bottomPosition);
	}

	private void removeMusicEvent(MusicEvent e){
		if (musicEvents.ContainsKey (e)) {
			AnimateThenDestory (musicEvents [e]);
			musicEvents.Remove (e);
		}
	}

	// Update is called once per frame
	void Update () {
		if (patternMaster.isPlaying()) {
			List<MusicEvent> noLongerScorable = new List<MusicEvent> ();

			foreach(KeyValuePair<MusicEvent, GameObject> pair in musicEvents)
			{
				MusicEvent e = pair.Key;
				long delta = e.startTime - patternMaster.currentSongTime();

				delta += latencyAdjustment;

				GameObject pokeball = pair.Value;
				//			Debug.Log (delta);


				//if objects are past pad point, no longer take scoring into account
				if (delta < -ttl) {
					noLongerScorable.Add (e);
				}

				//if objects are visible
				if (delta < scale) {
					float travelHeight = padPosition - bottomLeftCorner.y;
					float y = padPosition - delta / scale * travelHeight;
					pokeball.transform.position = new Vector3 (pokeball.transform.position.x, y, pokeball.transform.position.z);
				}



				//			float y = padPosition - velocity * delta / 1000f;
				// do something with entry.Value or entry.Key
			}

			foreach (MusicEvent e in noLongerScorable) {
//				Destroy (musicEvents [e]);

//				removeMusicEvent (e);
//				GameObject musicEventObj = musicEvents [e];
				removeMusicEvent (e);

				Messenger<MusicEvent>.Invoke (MessengerKeys.EVENT_OUT_OF_RANGE, e);
			}

			//clean up
			foreach (GameObject obj in destoryQueue) {
				Destroy (obj);
			}
			destoryQueue.Clear ();


			//		foreach (var e in musicEvents) {			
			//			long delta = e.startTime + patternMaster.startTime - timeMaster.GetTime ();
			//
		}
//		}
	}
		
	void AnimateThenDestory(GameObject eventObject){
		LeanTween
			.scaleX (eventObject, 1.1f, 0.1f);
		LeanTween
			.scaleY (eventObject, 1.1f, 0.1f)
			.setOnComplete(() => destoryQueue.Add(eventObject));
	}

	void OnDisable(){
		Messenger<MusicEvent>.RemoveListener (MessengerKeys.EVENT_VANISH, removeMusicEvent);
		Messenger<MusicEvent>.RemoveListener (MessengerKeys.EVENT_NO_LONGER_ACTIVE, disableEvent);

	}

	void disableEvent(MusicEvent e){
		if (musicEvents.ContainsKey (e)) {
			GameObject obj = musicEvents [e];
			SpriteRenderer renderer = obj.GetComponent<SpriteRenderer> ();
			renderer.color = new Color (1f, 1f, 1f, 0.5f);
		}
	}

	public void addEvent(MusicEvent e){
//		Debug.Log (Instantiate(Resources.Load("Pokeball1")));
		GameObject ball = generateBall (e);
		musicEvents.Add (e, ball);
	}

	public void reset(){

		foreach(KeyValuePair<MusicEvent, GameObject> pair in musicEvents)
		{
			Destroy (pair.Value);
			//			float y = padPosition - velocity * delta / 1000f;
			// do something with entry.Value or entry.Key
		}



		musicEvents.Clear ();
	}

	private GameObject generateBall (MusicEvent e){
//		GameObject ball = Instantiate(Resources.Load("Pokeball1")) as GameObject;
		GameObject ball = prefabForEvent (e);


		Pad pad = padForEvent (e);
		ball.transform.position = new Vector3(pad.transform.position.x, -10, 0);

		return ball;
	}



	private GameObject prefabForEvent(MusicEvent e){
		string prefabNum;
		switch (e.eventType) 
		{
		case MusicEventTypes.Down:
			prefabNum = "Pokeball1";
			break;
		case MusicEventTypes.Up:
			prefabNum = "Pokeball2";
			break;
		case MusicEventTypes.Left:
			prefabNum = "Pokeball3";
			break;
		case MusicEventTypes.Right:
			prefabNum = "Pokeball4";
			break;
		default:
			prefabNum = "Pokeball1";
			Debug.LogError ("No ball this event");
			break;

		}
		return Instantiate(Resources.Load(prefabNum)) as GameObject;;
	}


	//not safe
	private Pad padForEvent(MusicEvent e){
		int padNum = 0;
		switch (e.eventType) 
		{
		case MusicEventTypes.Down:
			padNum = 1;
			break;
		case MusicEventTypes.Up:
			padNum = 2;
			break;
		case MusicEventTypes.Left:
			padNum = 0;
			break;
		case MusicEventTypes.Right:
			padNum = 3;
			break;
		default:
			break;

		}
		return padList [padNum];
	}




}
