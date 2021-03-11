using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreens : MonoBehaviour
{
	public void RestartGame()
	{
		SceneManager.LoadScene(1);
		Time.timeScale = 1;
	}
	
	public void QuitGame()
	{
		Debug.Log("JE QUITTE MERCI");
		Application.Quit();
		
	}

}
