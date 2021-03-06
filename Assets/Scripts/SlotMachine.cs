﻿// RAD-Assignment5, Sergei #200325005, 2-12-2016. 
// This mini game "Slot Machine"
// original code written by Tom
// add basic animation, sounds, text labels and buttons
// Good luck in this gambling machine
// P.S. This machine has cheat button
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class SlotMachine : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		// set basic bet text on start
		this.BetText.text = playerBet.ToString ();

	}

	private int playerMoney = 1000;
	private int winnings = 0;
	private int jackpot = 5000;
	private float turn = 0.0f;
	private int playerBet = 10;
	private float winNumber = 0.0f;
	private float lossNumber = 0.0f;
	private string[] spinResult;	
	//private string fruits = "";
	private float winRatio = 0.0f;
	private float lossRatio = 0.0f;
	private int grapes = 0;
	private int bananas = 0;
	private int oranges = 0;
	private int cherries = 0;
	private int bars = 0;
	private int bells = 0;
	private int sevens = 0;
	private int blanks = 0;

	// custom variables
	public Text BetText, GameResultText, TotalCreditText, WinnerPaidText;
	public Button spinButton;
	public Image[] slots;
	public Sprite[] slotSprites;
	// sounds
	public AudioSource spinSound;

	/* Utility function to show Player Stats */
	private void showPlayerStats()
	{
		winRatio = winNumber / turn;
		lossRatio = lossNumber / turn;
		string stats = "";
		stats += ("Jackpot: " + jackpot + "\n");
		stats += ("Player Money: " + playerMoney + "\n");
		stats += ("Turn: " + turn + "\n");
		stats += ("Wins: " + winNumber + "\n");
		stats += ("Losses: " + lossNumber + "\n");
		stats += ("Win Ratio: " + (winRatio * 100) + "%\n");
		stats += ("Loss Ratio: " + (lossRatio * 100) + "%\n");
		Debug.Log(stats);
	}

	/* Utility function to reset all fruit tallies*/
	private void resetFruitTally()
	{
		grapes = 0;
		bananas = 0;
		oranges = 0;
		cherries = 0;
		bars = 0;
		bells = 0;
		sevens = 0;
		blanks = 0;
	}

	/* Utility function to reset the player stats */
	private void resetAll()
	{
		playerMoney = 1000;
		winnings = 0;
		jackpot = 5000;
		turn = 0;
		playerBet = 0;
		winNumber = 0;
		lossNumber = 0;
		winRatio = 0.0f;

		this.spinButton.interactable = true;
		setGameResultText ("Welcome");
		setWinnerPaidText (0);
		setTotalCreditText(playerMoney);
		SetBet (10);
	}

	/* Check to see if the player won the jackpot */
	private void checkJackPot()
	{
		/* compare two random values */
		var jackPotTry = Random.Range (1, 51);
		var jackPotWin = Random.Range (1, 51);
		if (jackPotTry == jackPotWin)
		{
			playerMoney += jackpot;
			jackpot = 1000;

			// if player won jackpot
			setGameResultText ("JACKPOT!!!");
			setWinnerPaidText (jackpot);
			setTotalCreditText (playerMoney);
		}
	}

	/* Utility function to show a win message and increase player money */
	private void showWinMessage()
	{
		playerMoney += winnings;

		// add text to text fields
		setGameResultText ("WINNER");
		setWinnerPaidText(winnings);
		setTotalCreditText (playerMoney);

		resetFruitTally();
		checkJackPot();
	}

	/* Utility function to show a loss message and reduce player money */
	private void showLossMessage()
	{
		playerMoney -= playerBet;

		// add text to text fields
		setGameResultText ("You lost!");
		setWinnerPaidText (0);
		setTotalCreditText (playerMoney);

		resetFruitTally();
	}

	/* Utility function to check if a value falls within a range of bounds */
	private bool checkRange(int value, int lowerBounds, int upperBounds)
	{
		return (value >= lowerBounds && value <= upperBounds) ? true : false;

	}

	/* When this function is called it determines the betLine results.
    e.g. Bar - Orange - Banana */
	private string[] Reels()
	{
		string[] betLine = { " ", " ", " " };
		int[] outCome = { 0, 0, 0 };

		for (var spin = 0; spin < 3; spin++)
		{
			outCome[spin] = Random.Range(1,65);

			if (checkRange(outCome[spin], 1, 27)) {  // 41.5% probability
				blanks++;
				ChangeImage (spin, 0);
			}
			else if (checkRange(outCome[spin], 28, 37)){ // 15.4% probability
				grapes++;
				ChangeImage (spin, 1);
			}
			else if (checkRange(outCome[spin], 38, 46)){ // 13.8% probability
				bananas++;
				ChangeImage (spin, 2);
			}
			else if (checkRange(outCome[spin], 47, 54)){ // 12.3% probability
				oranges++;
				ChangeImage (spin, 3);
			}
			else if (checkRange(outCome[spin], 55, 59)){ //  7.7% probability
				cherries++;
				ChangeImage (spin, 4);
			}
			else if (checkRange(outCome[spin], 60, 62)){ //  4.6% probability
				bars++;
				ChangeImage (spin, 5);
			}
			else if (checkRange(outCome[spin], 63, 64)){ //  3.1% probability
				bells++;
				ChangeImage (spin, 6);
			}
			else if (checkRange(outCome[spin], 65, 65)){ //  1.5% probability
				sevens++;
				ChangeImage (spin, 7);
			}

		}
		return betLine;
	}

	/* This function calculates the player's winnings, if any */
	private void determineWinnings()
	{
		if (blanks == 0)
		{
			if (grapes == 3)
			{
				winnings = playerBet * 10;
			}
			else if (bananas == 3)
			{
				winnings = playerBet * 20;
			}
			else if (oranges == 3)
			{
				winnings = playerBet * 30;
			}
			else if (cherries == 3)
			{
				winnings = playerBet * 40;
			}
			else if (bars == 3)
			{
				winnings = playerBet * 50;
			}
			else if (bells == 3)
			{
				winnings = playerBet * 75;
			}
			else if (sevens == 3)
			{
				winnings = playerBet * 100;
			}
			else if (grapes == 2)
			{
				winnings = playerBet * 2;
			}
			else if (bananas == 2)
			{
				winnings = playerBet * 2;
			}
			else if (oranges == 2)
			{
				winnings = playerBet * 3;
			}
			else if (cherries == 2)
			{
				winnings = playerBet * 4;
			}
			else if (bars == 2)
			{
				winnings = playerBet * 5;
			}
			else if (bells == 2)
			{
				winnings = playerBet * 10;
			}
			else if (sevens == 2)
			{
				winnings = playerBet * 20;
			}
			else if (sevens == 1)
			{
				winnings = playerBet * 5;
			}
			else
			{
				winnings = playerBet * 1;
			}
			winNumber++;
			showWinMessage();
		}
		else
		{
			lossNumber++;
			showLossMessage();
		}

	}

	public void OnSpinButtonClick()
	{

		if (playerMoney == 0)
		{
			StartCoroutine (gameOverTimer ());
		}
		else if (checkBet())
		{

		}
		else if (!checkBet())
		{
			this.spinButton.interactable = false;
			this.spinSound.Play();
			StartCoroutine(setRandomImg());
			StartCoroutine(setTimer ());
			turn++;
			showPlayerStats();
		}
	}

	// CUSTOM CODE

	// this method check game result and set result pictures to slot machine
	public void ChangeImage(int slotNumber, int spriteNumber) {
		this.slots[slotNumber].sprite = slotSprites[spriteNumber];			
	}

	// function to wait some time and then show result
	public IEnumerator setTimer() {
			yield return new WaitForSeconds (3.0f);
			Reels ();
			determineWinnings();
		    this.spinButton.interactable = true;
	}

	// this function change slot images several times
	public IEnumerator setRandomImg() {
		for (float count = 0; count < 10; count++) {
			this.slots[0].sprite = slotSprites[Random.Range(0,7)];
			this.slots[1].sprite = slotSprites[Random.Range(0,7)];
			this.slots[2].sprite = slotSprites[Random.Range(0,7)];
			yield return new WaitForSeconds(0.3f);
		}
	}

	// show game over for 5 seconds
	public IEnumerator gameOverTimer() {
			setGameResultText ("GAME OVER");
			showPlayerStats();
			this.spinButton.interactable = false;
			yield return new WaitForSeconds (5.0f);
			resetAll();
	}

	// play sound coming soon
	public void SpinSound() {
		
	}

	// set new bet
	public void SetBet(int newBet) {
		playerBet = newBet;
		this.BetText.text = playerBet.ToString ();
		checkBet ();
	}

	private bool checkBet() {
		if (playerBet > playerMoney) {
			this.spinButton.interactable = false;
			setGameResultText ("Decrease Bet");
			return true;
		} else {
			this.spinButton.interactable = true;
			return false;
		}
	}

	// hack machine
	public void hackMachine() {
		playerMoney = 9999;
		setGameResultText ("CHEATER!");
		setTotalCreditText (playerMoney);
	}

	// set text for game result text field
	private void setGameResultText(string gameResult) {
		this.GameResultText.text = gameResult;
	}

	// set text for total credit text field
	private void setTotalCreditText(int totalMoney) {
		this.TotalCreditText.text = totalMoney.ToString ();
	}

	// set text for game result text field
	private void setWinnerPaidText(int paidMoney) {
		this.WinnerPaidText.text = paidMoney.ToString ();
	}

	// reset game score
	public void ResetGame() {
		resetFruitTally ();
		resetAll ();
	}
 
	// terminate game
	public void Exit() {
		setGameResultText ("Exit");
		Application.Quit();
	}
}
