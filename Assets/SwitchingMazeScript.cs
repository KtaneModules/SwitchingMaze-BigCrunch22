using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class SwitchingMazeScript : MonoBehaviour
{
    public KMAudio Audio;
    public KMBombInfo Bomb;
    public KMBombModule Module;

    public AudioClip[] SFX;
	public AudioSource Digger;
	public Material[] BorderColor;
	public MeshRenderer Border;

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
	
	string[] ColorsOfMaze = {"Red", "Green", "Blue", "Magenta", "Cyan", "Yellow", "Black", "White", "Gray", "Orange", "Pink", "Brown"};
	
	private string[][][] Mazes = new string[12][][]{
		new string[6][]{
			new string[6] {"E", "M", "E", "F", "K", "F"},
			new string[6] {"C", "J", "G", "H", "F", "I"},
			new string[6] {"C", "J", "F", "L", "C", "G"},
			new string[6] {"I", "L", "I", "H", "B", "M"},
			new string[6] {"I", "I", "C", "J", "J", "F"},
			new string[6] {"H", "G", "N", "K", "J", "G"}
		},
		new string[6][]{
			new string[6] {"E", "D", "F", "E", "F", "L"},
			new string[6] {"I", "I", "H", "G", "H", "G"},
			new string[6] {"I", "H", "F", "E", "J", "F"},
			new string[6] {"I", "L", "H", "A", "E", "A"},
			new string[6] {"I", "H", "F", "N", "I", "I"},
			new string[6] {"H", "J", "G", "K", "G", "N"}
		},
		new string[6][]{
			new string[6] {"E", "F", "E", "F", "E", "F"},
			new string[6] {"I", "H", "G", "C", "G", "N"},
			new string[6] {"C", "J", "F", "H", "D", "F"},
			new string[6] {"C", "F", "I", "K", "G", "I"},
			new string[6] {"I", "I", "H", "J", "F", "I"},
			new string[6] {"N", "H", "M", "K", "G", "N"}
		},
		new string[6][]{
			new string[6] {"L", "E", "F", "E", "J", "F"},
			new string[6] {"I", "I", "N", "H", "F", "I"},
			new string[6] {"H", "O", "J", "J", "A", "I"},
			new string[6] {"K", "G", "K", "D", "G", "I"},
			new string[6] {"E", "J", "J", "G", "E", "G"},
			new string[6] {"H", "J", "J", "M", "H", "M"}
		},
		new string[6][]{
			new string[6] {"E", "J", "J", "F", "K", "F"},
			new string[6] {"H", "J", "F", "H", "F", "I"},
			new string[6] {"E", "J", "G", "L", "I", "I"},
			new string[6] {"I", "E", "F", "I", "H", "A"},
			new string[6] {"C", "G", "I", "H", "J", "A"},
			new string[6] {"H", "M", "H", "M", "K", "G"}
		},
		new string[6][]{
			new string[6] {"E", "J", "J", "D", "J", "F"},
			new string[6] {"H", "J", "F", "H", "F", "N"},
			new string[6] {"E", "M", "H", "M", "H", "F"},
			new string[6] {"H", "D", "J", "J", "J", "G"},
			new string[6] {"E", "A", "E", "J", "D", "F"},
			new string[6] {"N", "H", "G", "K", "G", "N"}
		},
		new string[6][]{
			new string[6] {"L", "E", "D", "J", "J", "M"},
			new string[6] {"H", "G", "H", "J", "J", "F"},
			new string[6] {"E", "D", "J", "J", "J", "A"},
			new string[6] {"I", "H", "F", "E", "M", "I"},
			new string[6] {"I", "L", "I", "H", "F", "I"},
			new string[6] {"H", "G", "H", "M", "H", "G"}
		},
		new string[6][]{
			new string[6] {"E", "F", "E", "J", "J", "F"},
			new string[6] {"I", "I", "N", "E", "F", "I"},
			new string[6] {"I", "I", "E", "G", "C", "G"},
			new string[6] {"I", "H", "A", "E", "B", "F"},
			new string[6] {"I", "E", "G", "N", "E", "G"},
			new string[6] {"N", "H", "J", "M", "H", "M"}
		},
		new string[6][]{
			new string[6] {"E", "F", "E", "J", "J", "F"},
			new string[6] {"N", "I", "I", "E", "M", "I"},
			new string[6] {"E", "B", "G", "H", "J", "A"},
			new string[6] {"H", "F", "K", "J", "J", "G"},
			new string[6] {"E", "G", "E", "J", "J", "F"},
			new string[6] {"H", "J", "G", "K", "J", "G"}
		},
		new string[6][]{
			new string[6] {"L", "E", "M", "E", "D", "F"},
			new string[6] {"I", "I", "E", "A", "I", "N"},
			new string[6] {"I", "I", "N", "I", "H", "F"},
			new string[6] {"H", "O", "F", "H", "F", "N"},
			new string[6] {"E", "G", "H", "F", "H", "F"},
			new string[6] {"H", "J", "M", "H", "J", "G"}
		},
		new string[6][]{
			new string[6] {"K", "F", "K", "F", "K", "F"},
			new string[6] {"E", "G", "L", "H", "F", "I"},
			new string[6] {"C", "D", "G", "E", "G", "I"},
			new string[6] {"I", "H", "J", "B", "J", "G"},
			new string[6] {"H", "J", "D", "F", "E", "F"},
			new string[6] {"K", "J", "G", "H", "G", "N"}
		},
		new string[6][]{
			new string[6] {"E", "M", "K", "J", "F", "L"},
			new string[6] {"C", "J", "J", "F", "H", "A"},
			new string[6] {"H", "D", "F", "I", "E", "G"},
			new string[6] {"F", "I", "I", "I", "C", "M"},
			new string[6] {"C", "G", "I", "N", "C", "F"},
			new string[6] {"H", "M", "H", "J", "G", "N"}
		}
	};
	int NumberBasis;

    private bool MovingAgain = false;

    string Kelp;

    // Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool ModuleSolved;

    void Awake()
    {
        moduleId = moduleIdCounter++;
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

        SendIt.OnInteract += delegate () { Sender(); return false; };
    }

    void Start()
    {
        Coordinance();
		for (int q = 0; q < 7; q++)
		{
			Steppers[q].SetActive(false);
		}
		Module.OnActivate += Player;
    }
	
	void Player()
	{
		StartCoroutine(FirstColor());
	}

    void Selected(int Pressing)
    {
        Stepping[Pressing].SetActive(true);
    }

    void Deselected(int Depressing)
    {
        Stepping[Depressing].SetActive(false);
    }
	
	IEnumerator FirstColor()
	{
		for (int q = 0; q < 7; q++)
		{
			Steppers[q].SetActive(true);
		}
		int[] ColorPair = {0, 0};
		int Numberbret = UnityEngine.Random.Range(0,12);
		NumberBasis = Numberbret;
		while (Numberbret == NumberBasis)
		{
			NumberBasis = UnityEngine.Random.Range(0,12);
		}
		ColorPair[0] = Numberbret;
		ColorPair[1] = NumberBasis;
		Audio.PlaySoundAtTransform(SFX[8].name, transform);
		for (int x = 0; x < 6; x++)
		{
			Border.material = BorderColor[ColorPair[1]];
			yield return new WaitForSecondsRealtime(0.05f);
			Border.material = BorderColor[ColorPair[0]];
			yield return new WaitForSecondsRealtime(0.05f);
		}
		Border.material = BorderColor[NumberBasis];
		Debug.LogFormat("[Switching Maze #{0}] Your starting maze color: {1}", moduleId, ColorsOfMaze[NumberBasis]);
		ColorLog();
	}

    IEnumerator ColorSwitch()
    {
		int[] ColorPair = {0, 0};
		ColorPair[0] = NumberBasis;
		int Judge = UnityEngine.Random.Range(0,3);
		if (Judge == 0)
		{
			int Numberbret = NumberBasis;
			while (Numberbret == NumberBasis)
			{
				NumberBasis = UnityEngine.Random.Range(0,12);
			}
			ColorPair[1] = NumberBasis;
			Audio.PlaySoundAtTransform(SFX[8].name, transform);
			for (int x = 0; x < 6; x++)
			{
				Border.material = BorderColor[ColorPair[1]];
				yield return new WaitForSecondsRealtime(0.05f);
				Border.material = BorderColor[ColorPair[0]];
				yield return new WaitForSecondsRealtime(0.05f);
			}
			Border.material = BorderColor[NumberBasis];
			Debug.LogFormat("[Switching Maze #{0}] The maze switch its color to: {1}", moduleId, ColorsOfMaze[NumberBasis]);
		}
		ColorLog();
    }
	
	void ColorLog()
	{
		Kelp = Mazes[NumberBasis][Copper[0][0]][Copper[0][1]];
		if (Kelp == "A")
		{
			Debug.LogFormat("[Switching Maze #{0}] Current wall on your cell: East", moduleId);
		}
		
		else if (Kelp == "B")
		{
			Debug.LogFormat("[Switching Maze #{0}] Current wall on your cell: South", moduleId);
		}
		
		else if (Kelp == "C")
		{
			Debug.LogFormat("[Switching Maze #{0}] Current wall on your cell: West", moduleId);
		}
		
		else if (Kelp == "D")
		{
			Debug.LogFormat("[Switching Maze #{0}] Current wall on your cell: North", moduleId);
		}
		
		else if (Kelp == "E")
		{
			Debug.LogFormat("[Switching Maze #{0}] Current wall on your cell: North/West", moduleId);
		}
		
		else if (Kelp == "F")
		{
			Debug.LogFormat("[Switching Maze #{0}] Current wall on your cell: North/East", moduleId);
		}
		
		else if (Kelp == "G")
		{
			Debug.LogFormat("[Switching Maze #{0}] Current wall on your cell: South/East", moduleId);
		}
		
		else if (Kelp == "H")
		{
			Debug.LogFormat("[Switching Maze #{0}] Current wall on your cell: South/West", moduleId);
		}
		
		else if (Kelp == "I")
		{
			Debug.LogFormat("[Switching Maze #{0}] Current wall on your cell: East/West", moduleId);
		}
		
		else if (Kelp == "J")
		{
			Debug.LogFormat("[Switching Maze #{0}] Current wall on your cell: North/South", moduleId);
		}
		
		else if (Kelp == "K")
		{
			Debug.LogFormat("[Switching Maze #{0}] Current wall on your cell: South/West/North", moduleId);
		}
		
		else if (Kelp == "L")
		{
			Debug.LogFormat("[Switching Maze #{0}] Current wall on your cell: West/North/East", moduleId);
		}
		
		else if (Kelp == "M")
		{
			Debug.LogFormat("[Switching Maze #{0}] Current wall on your cell: North/East/South", moduleId);
		}
		
		else if (Kelp == "N")
		{
			Debug.LogFormat("[Switching Maze #{0}] Current wall on your cell: East/South/West", moduleId);
		}
		
		else if (Kelp == "O")
		{
			Debug.LogFormat("[Switching Maze #{0}] Current wall on your cell: -", moduleId);
		}
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
				Debug.LogFormat("[Switching Maze #{0}] You moved north.", moduleId);
                if (Copper[0][0] == 0 || Kelp == "D" || Kelp == "E" || Kelp == "F" || Kelp == "J" || Kelp == "K" || Kelp == "L" || Kelp == "M")
                {
                    StartCoroutine(Incorrect());
                }

                else
                {
                    Copper[0][0] -= 1;
                    for (int q = 0; q < 7; q++)
                    {
                        Steppers[q].SetActive(false);
                    }
                    StartCoroutine(Stepyard());
					Debug.LogFormat("[Switching Maze #{2}] Your are currently on: {0}, {1}", Copper[0][0].ToString(), Copper[0][1].ToString(), moduleId);
					StartCoroutine(ColorSwitch());
                }
            }

            if (Movement == 1)
            {
				Debug.LogFormat("[Switching Maze #{0}] You moved south.", moduleId);
                if (Copper[0][0] == 5 || Kelp == "B" || Kelp == "G" || Kelp == "H" || Kelp == "J" || Kelp == "K" || Kelp == "M" || Kelp == "N")
                {
                    StartCoroutine(Incorrect());
                }

                else
                {
                    Copper[0][0] += 1;
                    for (int q = 0; q < 7; q++)
                    {
                        Steppers[q].SetActive(false);
                    }
					Debug.LogFormat("[Switching Maze #{2}] Your are currently on: {0}, {1}", Copper[0][0].ToString(), Copper[0][1].ToString(), moduleId);
                    StartCoroutine(Stepyard());
					StartCoroutine(ColorSwitch());
                }
            }

            if (Movement == 2)
            {
				Debug.LogFormat("[Switching Maze #{0}] You moved east.", moduleId);
                if (Copper[0][1] == 5 || Kelp == "A" || Kelp == "F" || Kelp == "G" || Kelp == "I" || Kelp == "L" || Kelp == "M" || Kelp == "N")
                {
                    StartCoroutine(Incorrect());
                }

                else
                {
                    Copper[0][1] += 1;
                    for (int q = 0; q < 7; q++)
                    {
                        Steppers[q].SetActive(false);
                    }
					Debug.LogFormat("[Switching Maze #{2}] Your are currently on: {0}, {1}", Copper[0][0].ToString(), Copper[0][1].ToString(), moduleId);
                    StartCoroutine(Stepyard());
					StartCoroutine(ColorSwitch());
                }
            }

            if (Movement == 3)
            {
				Debug.LogFormat("[Switching Maze #{0}] You moved west.", moduleId);
                if (Copper[0][1] == 0 || Kelp == "C" || Kelp == "E" || Kelp == "H" || Kelp == "I" || Kelp == "K" || Kelp == "L" || Kelp == "N")
                {
                    StartCoroutine(Incorrect());
                }

                else
                {
                    Copper[0][1] -= 1;
                    for (int q = 0; q < 7; q++)
                    {
                        Steppers[q].SetActive(false);
                    }
                    StartCoroutine(Stepyard());
					Debug.LogFormat("[Switching Maze #{2}] Your are currently on: {0}, {1}", Copper[0][0].ToString(), Copper[0][1].ToString(), moduleId);
					StartCoroutine(ColorSwitch());
                }
            }
        }
    }

    IEnumerator Stepyard()
    {
        MovingAgain = true;
		TakingAStep = true;
        Audio.PlaySoundAtTransform(SFX[0].name, transform);
        yield return new WaitForSecondsRealtime(0.65f);
        for (int q = 0; q < 7; q++)
        {
            Steppers[q].SetActive(true);
        }
		yield return new WaitForSecondsRealtime(0.35f);
        MovingAgain = false;
		TakingAStep = false;
    }

    string[] Alphabreak = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "+", "/" };
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
                Copper[a][b] = ((Copper[2][0] * 64) + (Copper[2][1])) % 6;
            }
        }
		Debug.LogFormat("[Switching Maze #{0}] {1}", moduleId, Seedling.text);
        Debug.LogFormat("[Switching Maze #{2}] Your starting coordinance is: {0}, {1}", Copper[0][0].ToString(), Copper[0][1].ToString(), moduleId);
        Debug.LogFormat("[Switching Maze #{2}] Your destination is: {0}, {1}", Copper[1][0].ToString(), Copper[1][1].ToString(), moduleId);
    }

    IEnumerator Incorrect()
    {
		Debug.LogFormat("[Switching Maze #{0}] You slammed on a wall. The mazing is now moving.", moduleId);
        MovingAgain = true;
		MazeMoving = true;
        for (int q = 0; q < 7; q++)
        {
            Steppers[q].SetActive(false);
        }
        Audio.PlaySoundAtTransform(SFX[3].name, transform);
        yield return new WaitForSecondsRealtime(1.8f);
		Digger.clip = SFX[4];
		Digger.Play();
        while (Digger.isPlaying)
		{
            int ColorMemorem = UnityEngine.Random.Range(0,12);
			Border.material = BorderColor[ColorMemorem];
            yield return new WaitForSecondsRealtime(0.04f);
        }
        Audio.PlaySoundAtTransform(SFX[5].name, transform);
        Module.HandleStrike();
		Debug.LogFormat("[Switching Maze #{0}] A strike was given to you.", moduleId);
        Coordinance();
        StartCoroutine(FirstColor());
        MovingAgain = false;
		MazeMoving = false;
    }

    IEnumerator ActualStep()
    {
		Debug.LogFormat("[Switching Maze #{0}] You activated your current platform. The maze is now moving.", moduleId);
        MovingAgain = true;
		MazeMoving = true;
        for (int q = 0; q < 7; q++)
        {
            Steppers[q].SetActive(false);
        }
        Audio.PlaySoundAtTransform(SFX[6].name, transform);
        yield return new WaitForSecondsRealtime(.75f);
		Digger.clip = SFX[4];
		Digger.Play();
        while (Digger.isPlaying)
		{
			int ColorMemorem = UnityEngine.Random.Range(0,12);
			Border.material = BorderColor[ColorMemorem];
            yield return new WaitForSecondsRealtime(0.04f);
        }
        if (Copper[0][0] == Copper[1][0] && Copper[0][1] == Copper[1][1])
        {
            Audio.PlaySoundAtTransform(SFX[5].name, transform);
            Module.HandlePass();
            ModuleSolved = true;
			Border.material = BorderColor[11];
            yield return new WaitForSecondsRealtime(1f);
            Audio.PlaySoundAtTransform(SFX[7].name, transform);
			Debug.LogFormat("[Switching Maze #{0}] You stepped the correct platform. Module solved.", moduleId);
        }
        else
        {
            Audio.PlaySoundAtTransform(SFX[5].name, transform);
            Module.HandleStrike();
			Debug.LogFormat("[Switching Maze #{0}] You stepped on an incorrect platform. A strike was given.", moduleId);
            Coordinance();
            StartCoroutine(FirstColor());
            for (int q = 0; q < 7; q++)
            {
                Steppers[q].SetActive(true);
            }
            MovingAgain = false;
        }
		MazeMoving = false;
    }
	
	//twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"To move in the maze, use the command !{0} north/south/east/west or up/down/left/right | To activate your current platform, use the command !{0} set";
    #pragma warning restore 414
	
	bool TakingAStep = false;
	bool MazeMoving = false;
	
	IEnumerator ProcessTwitchCommand(string command)
	{
		string[] parameters = command.Split(' ');
		
		if (Regex.IsMatch(command, @"^\s*up\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*north\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*u\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*n\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;
			if (TakingAStep == true)
			{
				yield return "sendtochaterror You are currently moving in the maze. The command was not processed.";
				yield break;
			}
			
			else if (MazeMoving == true)
			{
				yield return "sendtochaterror The maze is currently moving. The command was not processed.";
				yield break;
			}
			yield return "strike";
			Steps[0].OnInteract();
		}
		
		if (Regex.IsMatch(command, @"^\s*down\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*south\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*d\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*s\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;			
			if (TakingAStep == true)
			{
				yield return "sendtochaterror You are currently moving in the maze. The command was not processed.";
				yield break;
			}
			
			else if (MazeMoving == true)
			{
				yield return "sendtochaterror The maze is currently moving. The command was not processed.";
				yield break;
			}
			yield return "strike";
			Steps[1].OnInteract();
		}
		
		if (Regex.IsMatch(command, @"^\s*right\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*east\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*r\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*e\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;		
			if (TakingAStep == true)
			{
				yield return "sendtochaterror You are currently moving in the maze. The command was not processed.";
				yield break;
			}
			
			else if (MazeMoving == true)
			{
				yield return "sendtochaterror The maze is currently moving. The command was not processed.";
				yield break;
			}
			yield return "strike";
			Steps[2].OnInteract();
		}
		
		if (Regex.IsMatch(command, @"^\s*left\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*west\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*l\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*w\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;
			if (TakingAStep == true)
			{
				yield return "sendtochaterror You are currently moving in the maze. The command was not processed.";
				yield break;
			}
			
			else if (MazeMoving == true)
			{
				yield return "sendtochaterror The maze is currently moving. The command was not processed.";
				yield break;
			}
			yield return "strike";
			Steps[3].OnInteract();
		}
		
		if (Regex.IsMatch(command, @"^\s*set\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;
			if (TakingAStep == true)
			{
				yield return "sendtochaterror You are currently moving in the maze. The command was not processed.";
				yield break;
			}
			
			else if (MazeMoving == true)
			{
				yield return "sendtochaterror The maze is currently moving. The command was not processed.";
				yield break;
			}
			yield return "solve";
			yield return "strike";
			SendIt.OnInteract();
		}
	}
}
