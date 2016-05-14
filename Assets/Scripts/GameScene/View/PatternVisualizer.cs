using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PatternVisualizer : MonoBehaviour {

	public Pads pads;
//	public GameObject trails;
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
//	NoteTrail[] noteTrails;


//	List<MusicEvent> musicEvents = new List<MusicEvent> ();

//	List<MusicEvent> musicEventsToRemove = new List<MusicEvent>();

	Dictionary<MusicEvent, GameObject> musicEvents = new Dictionary<MusicEvent, GameObject> ();

	//the bool indicates whether or not the hold is currently active
	Dictionary<MusicEvent, bool> activeHolds = new Dictionary<MusicEvent, bool> ();
	Dictionary<MusicEvent, NoteTrail> noteTrails = new Dictionary<MusicEvent, NoteTrail> ();
	Dictionary<MusicEvent, GameObject> noteTrailEnds = new Dictionary<MusicEvent, GameObject> ();


	public Pool arrowPool;

	private int latencyAdjustment;
	List<GameObject> destoryQueue = new List<GameObject>();
	// Use this for initialization
	void Start () {
		Messenger<MusicEvent>.AddListener (MessengerKeys.EVENT_HIT, eventHit);
		Messenger<MusicEvent>.AddListener (MessengerKeys.EVENT_NO_LONGER_SCORABLE, disableEvent);
		Messenger<MusicEvent>.AddListener (MessengerKeys.EVENT_HELD_RELEASED, releaseEvent);
		Messenger<MusicEvent>.AddListener (MessengerKeys.EVENT_RELEASE_NO_LONGER_SCORABLE, releaseEvent);

		bottomLeftCorner = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, Camera.main.nearClipPlane));
		topRightCorner = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, Camera.main.nearClipPlane));

		bottomPosition = bottomLeftCorner.y;
		padPosition = pads.transform.position.y;


		padList = pads.GetComponentsInChildren<Pad> ();
//		noteTrails = trails.GetComponentsInChildren<NoteTrail> ();

//		if (noteTrails.Length == padList.Length) {
//			for (int i = 0; i < padList.Length; i++) {
//				Pad pad = padList [i];
//				NoteTrail trail = noteTrails [i];
//				trail.setX (pad.transform.position.x);
//				trail.setTopY (1);
//				trail.setBottomY (-1);
//			}
//		} else {
//			Debug.LogWarning ("Not an equal number of pads and trails");
//		}


		scale = GameManager.Instance.lookAhead;
		latencyAdjustment = PlayerPrefs.GetInt (PlayerPrefKeys.AudioLatencyOffset);
//		Debug.Log (bottomPosition);
	}

	void releaseEvent(MusicEvent e){
//		Debug.Log (e);

		if (noteTrails.ContainsKey (e)) {
			activeHolds.Remove (e);
			NoteTrail trail = noteTrails [e];
			Destroy (trail.gameObject);
			noteTrails.Remove (e);
		}
	}

	private void removeMusicEvent(MusicEvent e){
		if (musicEvents.ContainsKey (e)) {
//			AnimateThenDestory (musicEvents [e]);

			destoryQueue.Add (musicEvents [e]);
			musicEvents.Remove (e);
		}
	}

	// Update is called once per frame
	void Update () {
		if (patternMaster.isPlaying()) {
			List<MusicEvent> outOfRange = new List<MusicEvent> ();

			foreach(KeyValuePair<MusicEvent, GameObject> pair in musicEvents)
			{
				MusicEvent e = pair.Key;
				GameObject pokeball = pair.Value;

				float delta = timeToDelta (e.startTime);
				float y = timeToY (e.startTime);

				//if objects are past pad point
				if (delta < -ttl) {
					outOfRange.Add (e);
				}

				//if objects are visible
				if (delta < scale) {
					pokeball.transform.position = new Vector3 (pokeball.transform.position.x, y, pokeball.transform.position.z);
				}
			}

			foreach (MusicEvent e in outOfRange) {
//				Destroy (musicEvents [e]);

//				removeMusicEvent (e);
//				GameObject musicEventObj = musicEvents [e];
				removeMusicEvent (e);
				releaseEvent (e);

				Messenger<MusicEvent>.Invoke (MessengerKeys.EVENT_OUT_OF_RANGE, e);
			}

			foreach (MusicEvent e in activeHolds.Keys) {
				bool active = activeHolds [e];
				NoteTrail trail = noteTrails [e];

				if (active) {
					trail.setTopY (padList[0].transform.position.y);
					trail.setBottomY (timeToY (e.endTime));
				} else {

					trail.setTopY (timeToY (e.startTime));
					trail.setBottomY (timeToY (e.endTime));

//					NoteTrail trail = noteTrails [laneForEvent (e)];
//					trail.setTopY (timeToY(e.startTime));
//					trail.setBottomY(timeToY(e.endTime));
				}
			}



//			foreach (MusicEvent e in activeHolds.K) {
//				NoteTrail trail = noteTrails [laneForEvent (e)];
//				trail.setTopY (timeToY(e.startTime));
//				trail.setBottomY(timeToY(e.endTime));
//			}

			//clean up
			foreach (GameObject obj in destoryQueue) {
//				Destroy (obj);
				arrowPool.Decommision (obj);

			}
			destoryQueue.Clear ();


			//		foreach (var e in musicEvents) {			
			//			long delta = e.startTime + patternMaster.startTime - timeMaster.GetTime ();
			//
		}
//		}
	}

	float timeToDelta(long time){
		return time - patternMaster.currentSongTime () + latencyAdjustment;
	}

	float timeToY(long time){
		float delta = timeToDelta (time);
		float travelHeight = padPosition - bottomLeftCorner.y;
		float y = padPosition - delta / scale * travelHeight;
		return y;
	}
		
	void eventHit(MusicEvent m){
		if (musicEvents.ContainsKey (m)) {
			AnimateThenDestory (musicEvents [m]);
			musicEvents.Remove (m);

			if (activeHolds.ContainsKey (m)) {
				activeHolds [m] = true;
			}
		}
	}

	void AnimateThenDestory(GameObject eventObject){
		LeanTween
			.scaleX (eventObject, 1.1f, 0.1f);
		LeanTween
			.scaleY (eventObject, 1.1f, 0.1f)
			.setOnComplete(() => destoryQueue.Add(eventObject));
	}

	void OnDisable(){
		Messenger<MusicEvent>.RemoveListener (MessengerKeys.EVENT_HIT, removeMusicEvent);
		Messenger<MusicEvent>.RemoveListener (MessengerKeys.EVENT_NO_LONGER_SCORABLE, disableEvent);
		Messenger<MusicEvent>.RemoveListener (MessengerKeys.EVENT_HELD_RELEASED, releaseEvent);

	}

	void disableEvent(MusicEvent e){
//		if (musicEvents.ContainsKey (e)) {
////			GameObject obj = musicEvents [e];
////			SpriteRenderer renderer = obj.GetComponent<SpriteRenderer> ();
////			renderer.color = Color.Lerp (renderer.color, new Color(1f,1f,1f, 0.01f), 0.1f);
////			renderer.color = new Color(1f,1f,1f,0.1f);
//		}

	}

	public void addEvent(MusicEvent e){
//		Debug.Log (Instantiate(Resources.Load("Pokeball1")));
		GameObject ball = generateBall (e);
		musicEvents.Add (e, ball);
		if (e.isHeldEvent ()) {
			activeHolds[e] = false;

			NoteTrail trail = (Instantiate (Resources.Load ("Trail")) as GameObject).GetComponent<NoteTrail>();
			noteTrails [e] = trail;
			trail.setX (xForEvent (e));
//			Debug.Log (trail);

//			GameObject newBall = prefabForEvent(e);
		}
	}

	public void reset(){

		foreach(KeyValuePair<MusicEvent, GameObject> pair in musicEvents)
		{
			Destroy (pair.Value);
			//			float y = padPosition - velocity * delta / 1000f;
			// do something with entry.Value or entry.Key
		}
		foreach(KeyValuePair<MusicEvent, NoteTrail> pair in noteTrails)
		{
			Destroy (pair.Value.gameObject);
			//			float y = padPosition - velocity * delta / 1000f;
			// do something with entry.Value or entry.Key
		}
		foreach(KeyValuePair<MusicEvent, NoteTrail> pair in noteTrails)
		{
			Destroy (pair.Value.gameObject);
		}

		activeHolds.Clear ();
		noteTrails.Clear ();

		musicEvents.Clear ();
	}

	private GameObject generateBall (MusicEvent e){
//		GameObject ball = Instantiate(Resources.Load("Pokeball1")) as GameObject;
		GameObject ball = prefabForEvent (e);

		//so not exactly same z
//		ball.transform.Translate (new Vector3(0, 0, Random.value * 1.0f));

		Pad pad = padForEvent (e);
		ball.transform.position = new Vector3(pad.transform.position.x, -10, ball.transform.position.z);

		return ball;
	}



	private GameObject prefabForEvent(MusicEvent e){
//		string prefabNum;
//		switch (e.eventType) 
//		{
//		case MusicEventTypes.Down:
//			prefabNum = "Pokeball1";
//			break;
//		case MusicEventTypes.Up:
//			prefabNum = "Pokeball2";
//			break;
//		case MusicEventTypes.Left:
//			prefabNum = "Pokeball3";
//			break;
//		case MusicEventTypes.Right:
//			prefabNum = "Pokeball4";
//			break;
//		default:
//			prefabNum = "Pokeball1";
//			Debug.LogError ("No ball this event");
//			break;
//
//		}
//		return Instantiate(Resources.Load(prefabNum)) as GameObject;
		GameObject arrowObj = arrowPool.Commission ();
		Arrow arrow = arrowObj.GetComponent<Arrow> ();
//		arrow.transform.position = new Vector3(pad.transform.position.x, -10	, 0);
		arrow.SetDirection (e.eventType);
		arrow.randomizeZ ();
		return arrowObj;
	}
		
	//not safe
	private Pad padForEvent(MusicEvent e){
		return padList [laneForEvent(e)];
	}
	private int laneForEvent(MusicEvent e){
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
		return padNum;
	}
	private float xForEvent(MusicEvent e){
		return padList [laneForEvent (e)].transform.position.x;
	}




}
