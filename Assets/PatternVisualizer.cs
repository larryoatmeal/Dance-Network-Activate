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
	public float scale = 5000f;
	public int ttl = 100;
	Pad[] padList;
//	List<MusicEvent> musicEvents = new List<MusicEvent> ();

//	List<MusicEvent> musicEventsToRemove = new List<MusicEvent>();

	Dictionary<MusicEvent, GameObject> musicEvents = new Dictionary<MusicEvent, GameObject> ();

	// Use this for initialization
	void Start () {
		Messenger<MusicEvent>.AddListener (MessengerKeys.EVENT_VANISH, removeMusicEvent);


		bottomLeftCorner = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, Camera.main.nearClipPlane));
		topRightCorner = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, Camera.main.nearClipPlane));

		bottomPosition = bottomLeftCorner.y;
		padPosition = pads.transform.position.y;


		padList = pads.GetComponentsInChildren<Pad> ();
		Debug.Log (padList.Length);
//		Debug.Log (bottomPosition);
	}

	private void removeMusicEvent(MusicEvent e){
		if (musicEvents.ContainsKey (e)) {
			Destroy (musicEvents [e]);
			musicEvents.Remove (e);
		}
	}

	// Update is called once per frame
	void Update () {
		if (patternMaster.started) {
			List<MusicEvent> events = new List<MusicEvent> ();

			foreach(KeyValuePair<MusicEvent, GameObject> pair in musicEvents)
			{
				MusicEvent e = pair.Key;
				long delta = e.startTime + patternMaster.startTime - timeMaster.GetTime ();

				GameObject pokeball = pair.Value;
				//			Debug.Log (delta);


				if (delta < -ttl) {
					events.Add (e);
				}

				if (delta < scale) {
					float travelHeight = padPosition - bottomLeftCorner.y;
					float y = padPosition - delta / scale * travelHeight;
					pokeball.transform.position = new Vector3 (pokeball.transform.position.x, y, pokeball.transform.position.z);
				}


				//			float y = padPosition - velocity * delta / 1000f;
				// do something with entry.Value or entry.Key
			}

			foreach (MusicEvent e in events) {
				Destroy (musicEvents [e]);
				musicEvents.Remove (e);
				Messenger<MusicEvent>.Invoke (MessengerKeys.EVENT_OUT_OF_RANGE, e);
			}
			//		foreach (var e in musicEvents) {			
			//			long delta = e.startTime + patternMaster.startTime - timeMaster.GetTime ();
			//
		}
//		}
	}
		
	void OnDisable(){
		Messenger<MusicEvent>.RemoveListener (MessengerKeys.EVENT_VANISH, removeMusicEvent);
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
			padNum = 0;
			break;
		case MusicEventTypes.Up:
			padNum = 1;
			break;
		case MusicEventTypes.Left:
			padNum = 2;
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
