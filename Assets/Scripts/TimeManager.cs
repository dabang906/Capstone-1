using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    float playTime;
    public Text playTimeText;
    void Update()
    {
        playTime += Time.deltaTime;

        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600)/60);
        int second = (int)(playTime % 60);

        playTimeText.text = string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);

        if(min == 1)
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
