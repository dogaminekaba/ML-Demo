using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TMP_Text _winCount;
    public TMP_Text _loseCount;

    private int winCount = 0;
    private int loseCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        _winCount.text = "Win: " + winCount;
        _loseCount.text = "Lose: " + loseCount;
    }

    // Update is called once per frame
    void Update()
    {
        _winCount.text = "Win: " + winCount;
        _loseCount.text = "Lose: " + loseCount;
    }

    public void IncreaseWinCount()
    {
        ++winCount;
    }

    public void IncreaseLoseCount()
    {
        ++loseCount;
    }
}
