using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Grade{
	A,
	B,
	C,
	D,
	F
}


public class ScoreSummary{
	public int maxCombo;
	public int totalScore;
	public int score;
	public int numPerfect;
	public int numGreat;
	public int numOkay;
	public int numCoffin;
	public int numMiss;
	public Grade grade;

	public ScoreSummary (int maxCombo, int totalScore, int score, int numPerfect, int numGreat, int numOkay, int numCoffin, int numMiss, Grade grade)
	{
		this.maxCombo = maxCombo;
		this.totalScore = totalScore;
		this.score = score;
		this.numPerfect = numPerfect;
		this.numGreat = numGreat;
		this.numOkay = numOkay;
		this.numCoffin = numCoffin;
		this.numMiss = numMiss;
		this.grade = grade;
	}
}


public class FinalScore : MonoBehaviour {
	public Modal modal;	
	// Use this for initialization
	public void showScore(ScoreSummary scoreSummary){

		modal.gameObject.SetActive (true);
		modal.SetTitle ("Song Cleared!");
		modal.SetCancelEnabled (false);
		modal.SetNoEnabled (false);
		modal.SetOkayText ("Continue");

		modal.SetRows (new List<RowMeta>{
			{new RowMeta("Perfect", scoreSummary.numPerfect)},
			{new RowMeta("Great", scoreSummary.numGreat)},
			{new RowMeta("Okay", scoreSummary.numOkay)},
			{new RowMeta("Coffin", scoreSummary.numCoffin)},
			{new RowMeta("Miss", scoreSummary.numMiss)},
			{new RowMeta("Max Combo", scoreSummary.maxCombo)},
			{new RowMeta("Score", scoreSummary.score + "/" + scoreSummary.totalScore)},
			{new RowMeta("Grade", scoreSummary.grade)},
		});
	}


}
