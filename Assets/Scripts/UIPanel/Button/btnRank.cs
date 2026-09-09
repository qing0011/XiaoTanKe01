using TMPro;
using UnityEngine;

public class btnRank : MonoBehaviour
{
    public TextMeshProUGUI textRank;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textBestLevel;

    public void SetRankData(int rank, string name, int score, int bestLevel)
    {
        if (textRank != null)
            textRank.text = rank.ToString();
        
        if (textName != null)
            textName.text = name;
        
        if (textScore != null)
            textScore.text = score.ToString();
        
        if (textBestLevel != null)
            textBestLevel.text =  bestLevel.ToString();
    }
}
