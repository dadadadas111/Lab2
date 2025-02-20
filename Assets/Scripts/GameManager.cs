using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI killCountText;
    private int killCount = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void IncreaseKillCount()
    {
        killCount++;
        killCountText.text = "Kills: " + killCount;
    }

    public void ResetKillCount()
    {
        killCount = 0;
        killCountText.text = "Kills: " + killCount;
    }

    public int GetKillCount()
    {
        return killCount;
    }
}
