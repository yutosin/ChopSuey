using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
	public void OnStartButtonClick()
	{
		SceneManager.LoadScene("_Scene_1");
	}

	public void OnInstructionsButtonClick()
	{
		
	}
	
	public void OnQuitButtonClick()
	{
		Application.Quit();
	}
}
