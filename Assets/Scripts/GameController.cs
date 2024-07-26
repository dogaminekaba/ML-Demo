using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public TMP_Text _winCount;
	public TMP_Text _loseCount;
	public Image cooldownBarImg;
	public Button simulateButton;
	public Button playButton;

	private int winCount = 0;
	private int loseCount = 0;
	private bool isSimulating = false;

	// Start is called before the first frame update
	void Start()
	{
		_winCount.text = "Zombie Score: " + winCount;
		_loseCount.text = "Player Score: " + loseCount;

		SwitchToSimulate();
	}

	// Update is called once per frame
	void Update()
	{
		_winCount.text = "Zombie Score: " + winCount;
		_loseCount.text = "Player Score: " + loseCount;
	}

	public void IncreaseWinCount()
	{
		++winCount;
	}

	public void IncreaseLoseCount()
	{
		++loseCount;
	}

	public void UpdateCooldownUI(float fillAmount)
	{
		if (cooldownBarImg != null)
		{
			cooldownBarImg.fillAmount = fillAmount;
		}
	}

	public bool IsSimulating()
	{
		return isSimulating;
	}

	public void SwitchToSimulate()
	{
		simulateButton.interactable = false;
		playButton.interactable = true;
		isSimulating = true;
	}

	public void SwitchToPlay()
	{
		simulateButton.interactable = true;
		playButton.interactable = false;
		isSimulating = false;
	}

}
