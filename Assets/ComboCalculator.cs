using UnityEngine;
using System.Collections.Generic;

public class ComboCalculator : MonoBehaviour {
//	public PatternMaster patternMaster;
	public Dictionary<ScoreLevels, int> scoreMaping;

	public ComboText comboText;
	public Healthbar healthBar;
	int combo = 0;
	float score = 0;
	int maxScore;

	public int comboLogBase = 10;

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

		DebugPanel.Instance.log ("MaxScore", maxScore);

	}

	public void setMaxScore(int totalScorable){
		maxScore = calculateMaxScore (totalScorable);
		Debug.Log (maxScore);
	}

	int calculateMaxScore(int totalScorable){
		//max score is all perfects and all combos
		//essentially perfects * sum(log(n))
		//we know integral of log(x) is (xln(x) -x)/ln10 (from 1 to x)
		//we want a lower bound

		float x = totalScorable - 1;//ensurs that integral is bounded below sum
		float integral = (x * Mathf.Log (x) - x) / Mathf.Log (comboLogBase);

		DebugPanel.Instance.log ("Total score", integral.ToString());
		return perfectScore * (int)integral;
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
			score += scoreMaping [scoreLevel] * Mathf.Log (combo, comboLogBase);
		}

		if (score < 0) {
			score = 0;
		}

		DebugPanel.Instance.log ("Score Numeric", score);

		comboText.SetCombo (combo);
		healthBar.setFraction (score / maxScore);
	}

	void OnDestroy(){
		Messenger<ScoreLevels>.RemoveListener (MessengerKeys.EVENT_SCORE, onScore);
	}



}
