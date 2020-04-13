using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public CanvasGroup victoryGroup;
    public CanvasGroup LoseGroup;

    public GameObject button;
    public void SpyWin()
    {
        victoryGroup.alpha = 1;
        victoryGroup.blocksRaycasts = true;
        victoryGroup.interactable = true;

        button.SetActive(true);
    }

    public void SpyLoss()
    {
        LoseGroup.alpha = 1;
        LoseGroup.blocksRaycasts = true;
        LoseGroup.interactable = true;

        button.SetActive(true);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(0);
    }
}
