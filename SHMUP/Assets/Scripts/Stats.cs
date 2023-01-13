using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    // ----- | Variables | -----
    public Text text;
    public Scrollbar bar;
    public Collision refrence;
    public string format;

    public float score;
    public float maxScore;

    // ----- | Properties | -----
    public float Score
    {
        get { return score; }
        set { score = value; }
    }

    public float MaxScore
    {
        get { return maxScore; }
        set { maxScore = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        format = string.Format($"Stats:" +
                               $"\r\n- Bullets: ({refrence.PlayerGun.playerBullets.Count})10" +
                               $"\r\n- Clock: ({Mathf.Floor(refrence.clock)}){refrence.MaxClock}" +
                               $"\r\n- Score: ({Mathf.Floor(score)})");

        score = score + .2f * Time.deltaTime; 
        bar.size = refrence.Player.Health / 100;

        text.text = format;
    }
}
