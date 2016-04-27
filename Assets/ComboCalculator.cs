using UnityEngine;
using System.Collections.Generic;

public class ComboCalculator : MonoBehaviour {

	public Dictionary<ScoreLevels, int> scoreMaping;

	int combo = 0;
	float score = 0;


 	public int badScore = -200;
	public int missScore = -400;
	public int okayScore = 0;
	public int greatScore = 300;
	public int perfectScore = 500;
//	public int goodshitScore = 500; 


	// Use this for initialization
	void Start () {
		Messenger<ScoreLevels>.AddListener (MessengerKeys.EVENT_SCORE, onScore);
		scoreMaping = new Dictionary<ScoreLevels, int> () {
			{ ScoreLevels.Bad, badScore },
			{ ScoreLevels.Miss, missScore },
			{ ScoreLevels.Okay, okayScore },
			{ ScoreLevels.Great, greatScore },
			{ ScoreLevels.Perfect, perfectScore },
			{ ScoreLevels.GoodShit, perfectScore }
		};
	}

	void onScore(ScoreLevels scoreLevel){

		if (scoreLevel == ScoreLevels.Perfect || scoreLevel == ScoreLevels.GoodShit || scoreLevel == ScoreLevels.Great) {
			combo += 1;
		} else {
			combo = 0;
		}
		if (combo == 0) {
			score += scoreMaping [scoreLevel];
		} else {
			score += scoreMaping [scoreLevel] * Mathf.Log10 (combo);
		}

		if (score < 0) {
			score = 0;
		}

		DebugPanel.Instance.log ("Score Numeric", score);
	}

	void OnDestroy(){
		Messenger<ScoreLevels>.RemoveListener (MessengerKeys.EVENT_SCORE, onScore);
	}



}
