using UnityEngine;
using System.Collections.Generic;

public class ComboCalculator : MonoBehaviour {
//	public PatternMaster patternMaster;
	public Dictionary<ScoreLevels, int> scoreMaping;
	public Dictionary<ScoreLevels, int> scoreCount;

	public FinalScore finalScore;
	public ComboText comboText;
	public Healthbar healthBar;
	int combo = 0;
	float score = 0;
	int maxScore;


	int maxCombo = 0;

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
		Messenger.AddListener (MessengerKeys.EVENT_PATTERN_FINISHED, songFinished);



		scoreMaping = new Dictionary<ScoreLevels, int> () {
			{ ScoreLevels.Bad, badScore },
			{ ScoreLevels.Miss, missScore },
			{ ScoreLevels.Okay, okayScore },
			{ ScoreLevels.Great, greatScore },
			{ ScoreLevels.Perfect, perfectScore },
			{ ScoreLevels.GoodShit, perfectScore }
		};
		scoreCount = new Dictionary<ScoreLevels, int> () {
			{ ScoreLevels.Bad, 0 },
			{ ScoreLevels.Miss, 0 },
			{ ScoreLevels.Okay, 0 },
			{ ScoreLevels.Great, 0 },
			{ ScoreLevels.Perfect, 0 },
			{ ScoreLevels.GoodShit, 0 }
		};
		DebugPanel.Instance.log ("MaxScore", maxScore);
		healthBar.setFraction (0);
	}

	void OnDestroy(){
		Messenger<ScoreLevels>.RemoveListener (MessengerKeys.EVENT_SCORE, onScore);
		Messenger.RemoveListener (MessengerKeys.EVENT_PATTERN_FINISHED, songFinished);
	}

	public void setMaxScore(int totalScorable){
		maxScore = calculateMaxScore (totalScorable);
		Debug.Log (maxScore);
	}

	void songFinished(){
		finalScore.showScore (reportFinalScore ());
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
		scoreCount [scoreLevel] += 1;
		comboText.SetCombo (combo);
		maxCombo = Mathf.Max (combo, maxCombo);

		healthBar.setFraction (healthBarDisplayFraction());
	}

	float healthBarDisplayFraction(){
		//we don't necessarily want maxScore
		float display = score/ maxScore * 1.05f;
		return Mathf.Clamp (display, 0f, 1f);
	}




	public ScoreSummary reportFinalScore(){
		return new ScoreSummary(maxCombo, maxScore, (int) score,
			scoreCount[ScoreLevels.Perfect] + scoreCount[ScoreLevels.GoodShit],
			scoreCount[ScoreLevels.Great],
			scoreCount[ScoreLevels.Okay],
			scoreCount[ScoreLevels.Bad],
			scoreCount[ScoreLevels.Miss],
			getGrade()
		);
	}
		
	public Grade getGrade(){
		float percent = score / maxScore;

		if (percent > 0.9f) {
			return Grade.A;
		} else if (percent > 0.8f) {
			return Grade.B;
		} else if (percent > 0.7f) {
			return Grade.C;
		} else if (percent > 0.6f) {
			return Grade.D;
		} else {
			return Grade.F;
		}

	}



}
