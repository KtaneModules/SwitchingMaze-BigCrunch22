using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Video;
using KModkit;

public class ShiftingMazeScript : MonoBehaviour
{
	public KMAudio Audio;
    public KMBombInfo Bomb;
    public KMBombModule Module;
	
	public AudioClip[] SFX;
	
	public KMSelectable[] Steps;
	public GameObject[] Stepping;
	public GameObject[] Steppers;
	public TextMesh Seedling;
	public KMSelectable SendIt;
	
	private int[][] Copper = new int[3][]{
		new int[2] {0, 0},
		new int[2] {0, 0},
		new int[2] {0, 0}
	};
	
	private bool MovingAgain = false;
	
	string Kelp;
	
	//Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool ModuleSolved;
	
	void Awake()
	{	
		for (int k = 0; k < 5; ++k)
		{
			int Movement = k;
			Steps[Movement].OnInteract += delegate
			{
				Moving(Movement);
				return false;
			};
		}
		
		for (int a = 0; a < 5; ++a)
		{
			int Pressing = a;
			Steps[Pressing].OnHighlight += delegate
			{
				Selected(Pressing);
			};
		}
	
		for (int b = 0; b < 5; ++b)
		{
			int Depressing = b;
			Steps[Depressing].OnHighlightEnded += delegate
			{
				Deselected(Depressing);
			};
		}
		
		SendIt.OnInteract += delegate () {Sender(); return false; };
	}

	void Start()
	{
		Coordinance();
		Random();
	}
	
	string[] Alfa = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N"}; //Center
	string[] Bravo = {"E", "K", "L"}; //Top Left
	string[] Charlie = {"F", "L", "M"}; //Top Right
	string[] Delta = {"H", "K", "N"}; //Bottom Left
	string[] Echo = {"G", "M", "N"}; //Bottom Right
	string[] Foxtrot = {"E", "F", "J", "K", "L", "M"}; //Top
	string[] Golf = {"G", "H", "J", "K", "M", "N"}; //Bottom
	string[] Hotel = {"F", "G", "I", "L", "M", "N"}; //Right
	string[] India = {"E", "H", "I", "K", "L", "N"}; //Left
	
	void Selected(int Pressing)
	{
		Stepping[Pressing].SetActive(true);
	}
	
	void Deselected(int Depressing)
	{
		Stepping[Depressing].SetActive(false);
	}
	
	void Random()
	{
		if (Copper[0][0] == 0 && Copper[0][1] == 0)
		{
			Kelp = Bravo[UnityEngine.Random.Range(0, Bravo.Count())];
		}
		
		else if (Copper[0][0] == 0 && Copper[0][1] == 9)
		{
			Kelp = Charlie[UnityEngine.Random.Range(0, Charlie.Count())];
		}
		
		else if (Copper[0][0] == 9 && Copper[0][1] == 0)
		{
			Kelp = Delta[UnityEngine.Random.Range(0, Delta.Count())];
		}
		
		else if (Copper[0][0] == 9 && Copper[0][1] == 9)
		{
			Kelp = Echo[UnityEngine.Random.Range(0, Echo.Count())];
		}
		
		else if (Copper[0][0] == 0)
		{
			Kelp = Foxtrot[UnityEngine.Random.Range(0, Foxtrot.Count())];
		}
		
		else if (Copper[0][0] == 9)
		{
			Kelp = Golf[UnityEngine.Random.Range(0, Golf.Count())];
		}
		
		else if (Copper[0][1] == 9)
		{
			Kelp = Hotel[UnityEngine.Random.Range(0, Hotel.Count())];
		}
		
		else if (Copper[0][1] == 0)
		{
			Kelp = India[UnityEngine.Random.Range(0, India.Count())];
		}
		
		else
		{
			Kelp = Alfa[UnityEngine.Random.Range(0, Alfa.Count())];
		}
		
		Debug.LogFormat(Kelp);
	}
	
	void Sender()
	{
		if (MovingAgain == false)
		{
			StartCoroutine(ActualStep());
		}
	}
	
	void Moving(int Movement)
	{
		if (MovingAgain == false)
		{
			if (Movement == 0)
			{
				if (Copper[0][0] == 0 || Kelp == "D" || Kelp == "E" || Kelp == "F" || Kelp == "J" || Kelp == "K" || Kelp == "L" || Kelp == "M")
				{
					StartCoroutine(Incorrect());
				}
				
				else
				{
					Copper[0][0] -= 1;
					Random();
					for (int q = 0; q < 6; q++)
					{
						Steppers[q].SetActive(false);
					}
					StartCoroutine(Stepyard());
				}
			}
			
			if (Movement == 1)
			{
				if (Copper[0][0] == 9 || Kelp == "B" || Kelp == "G" || Kelp == "H" || Kelp == "J" || Kelp == "K" || Kelp == "M" || Kelp == "N")
				{
					StartCoroutine(Incorrect());
				}
				
				else
				{
					Copper[0][0] += 1;
					Random();
					for (int q = 0; q < 6; q++)
					{
						Steppers[q].SetActive(false);
					}
					StartCoroutine(Stepyard());
				}
			}
			
			if (Movement == 2)
			{
				if (Copper[0][1] == 9 || Kelp == "A" || Kelp == "F" || Kelp == "G" || Kelp == "I" || Kelp == "L" || Kelp == "M" || Kelp == "N")
				{
					StartCoroutine(Incorrect());
				}
				
				else
				{
					Copper[0][1] += 1;
					Random();
					for (int q = 0; q < 6; q++)
					{
						Steppers[q].SetActive(false);
					}
					StartCoroutine(Stepyard());
				}
			}
			
			if (Movement == 3)
			{
				if (Copper[0][1] == 0 || Kelp == "C" || Kelp == "E" || Kelp == "H" || Kelp == "I" || Kelp == "K" || Kelp == "L" || Kelp == "N")
				{
					StartCoroutine(Incorrect());
				}
				
				else
				{
					Copper[0][1] -= 1;
					Random();
					for (int q = 0; q < 6; q++)
					{
						Steppers[q].SetActive(false);
					}
					StartCoroutine(Stepyard());
				}
			}
			
			if (Movement == 4)
			{
				StartCoroutine(Dinger());
			}
		}
	}
	
	IEnumerator Dinger()
	{
		MovingAgain = true;
		for (int q = 0; q < 6; q++)
		{
			Steppers[q].SetActive(false);
		}
		if (Kelp == "A")
		{
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[0].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[2].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[1].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[3].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
		}
		
		else if (Kelp == "B")
		{
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[0].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[2].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[1].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[3].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
		}
		
		else if (Kelp == "C")
		{
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[0].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[2].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[1].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[3].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
		}
		
		else if (Kelp == "D")
		{
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[0].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[2].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[1].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[3].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
		}
		
		else if (Kelp == "E")
		{
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[0].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[2].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[1].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[3].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
		}
		
		else if (Kelp == "F")
		{
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[0].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[2].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[1].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[3].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
		}
		
		else if (Kelp == "G")
		{
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[0].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[2].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[1].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[3].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
		}
		
		else if (Kelp == "H")
		{
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[0].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[2].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[1].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[3].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
		}
		
		else if (Kelp == "I")
		{
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[0].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[2].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[1].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[3].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
		}
		
		else if (Kelp == "J")
		{
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[0].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[2].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[1].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[3].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
		}
		
		else if (Kelp == "K")
		{
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[0].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[2].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[1].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[3].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
		}
		
		else if (Kelp == "L")
		{
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[0].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[2].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[1].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[3].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
		}
		
		else if (Kelp == "M")
		{
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[0].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[2].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[1].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Stepping[3].SetActive(false);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			yield return new WaitForSecondsRealtime(0.4f);
		}
		
		else if (Kelp == "N")
		{
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[1].name, transform);
			Stepping[0].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[2].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[1].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
			Audio.PlaySoundAtTransform(SFX[2].name, transform);
			Stepping[3].SetActive(false);
			yield return new WaitForSecondsRealtime(0.4f);
		}
		for (int q = 0; q < 4; q++)
		{
			Steppers[q].SetActive(true);
		}
		Steppers[5].SetActive(true);
		MovingAgain = false;
	}
	
	IEnumerator Stepyard()
	{
		MovingAgain = true;
		Audio.PlaySoundAtTransform(SFX[0].name, transform);
		yield return new WaitForSecondsRealtime(0.65f);
		for (int q = 0; q < 6; q++)
		{
			Steppers[q].SetActive(true);
		}
		MovingAgain = false;
	}
	
	string[] Alphabreak = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "+", "/"};
	void Coordinance()
	{
		Seedling.text = "SEED: ";
		for (int a = 0; a < 2; a++)
		{
			for (int b = 0; b < 2; b++)
			{
				for (int c = 0; c < 2; c++)
				{
					Copper[2][c] = UnityEngine.Random.Range(0, Alphabreak.Count());
					Seedling.text += Alphabreak[Copper[2][c]];
				}
				Copper[a][b] = ((Copper[2][0] * 64) + (Copper[2][1])) % 10;
			}
		}
		Debug.LogFormat("Your starting coordinance is: {0}, {1}", Copper[0][0].ToString(), Copper[0][1].ToString());
		Debug.LogFormat("Your destination is: {0}, {1}", Copper[1][0].ToString(), Copper[1][1].ToString());
	}
	
	IEnumerator Incorrect()
	{
		MovingAgain = true;
		for (int q = 0; q < 6; q++)
		{
			Steppers[q].SetActive(false);
		}
		Audio.PlaySoundAtTransform(SFX[3].name, transform);
		yield return new WaitForSecondsRealtime(1.8f);
		Audio.PlaySoundAtTransform(SFX[4].name, transform);
		for (int c = 0; c < 630; c++)
		{
			Seedling.text = "SEED: ";
			for (int f = 0; f < 8; f++)
			{
				int Seedlings = UnityEngine.Random.Range(0, Alphabreak.Count());
				Seedling.text += Alphabreak[Seedlings];
			}
			yield return new WaitForSecondsRealtime(0.01f);
		}
		Audio.PlaySoundAtTransform(SFX[5].name, transform);
		Module.HandleStrike();
		Coordinance();
		Random();
		for (int q = 0; q < 6; q++)
		{
			Steppers[q].SetActive(true);
		}
		MovingAgain = false;
	}
	
	IEnumerator ActualStep()
	{
		MovingAgain = true;
		for (int q = 0; q < 6; q++)
		{
			Steppers[q].SetActive(false);
		}
		Audio.PlaySoundAtTransform(SFX[6].name, transform);
		yield return new WaitForSecondsRealtime(.75f);
		Audio.PlaySoundAtTransform(SFX[4].name, transform);
		for (int c = 0; c < 600; c++)
		{
			Seedling.text = "SEED: ";
			for (int f = 0; f < 8; f++)
			{
				int Seedlings = UnityEngine.Random.Range(0, Alphabreak.Count());
				Seedling.text += Alphabreak[Seedlings];
			}
			yield return new WaitForSecondsRealtime(0.01f);
		}
		if (Copper[0][0] == Copper[1][0] && Copper[0][1] == Copper[1][1])
		{
			Audio.PlaySoundAtTransform(SFX[5].name, transform);
			Module.HandlePass();
			Seedling.text = "";
			yield return new WaitForSecondsRealtime(1f);
			Audio.PlaySoundAtTransform(SFX[7].name, transform);
		}
		else
		{
			Audio.PlaySoundAtTransform(SFX[5].name, transform);
			Module.HandleStrike();
			Coordinance();
			Random();
			for (int q = 0; q < 6; q++)
			{
				Steppers[q].SetActive(true);
			}
			MovingAgain = false;
		}
	}
}