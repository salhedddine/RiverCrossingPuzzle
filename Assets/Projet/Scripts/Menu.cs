using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	public static bool soundIsOn;
	public static bool visuelIsOn;
	public static bool vibrationIsOn;

	public void Start()
	{
		soundIsOn = false;
		visuelIsOn = false;
		vibrationIsOn = false;
	}

	public void PlayGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void QuitGame()
	{
		Application.Quit();
		Debug.Log("JE QUITTE MERCI");
	}

	public void ChangeValueSound()
	{
		if (soundIsOn)
		{
			soundIsOn = !soundIsOn;
		}
	}

	public void ChangeValueVisuel()
	{
		if (visuelIsOn)
		{
			visuelIsOn = !visuelIsOn;
		}
	}

	public void ChangeValueVibration()
	{
		if (vibrationIsOn)
		{
			vibrationIsOn = !vibrationIsOn;
		}
	}


}
