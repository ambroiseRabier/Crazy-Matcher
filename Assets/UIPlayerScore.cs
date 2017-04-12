using UnityEngine;
using UnityEngine.UI;

public class UIPlayerScore : MonoBehaviour {
    [SerializeField] private Text m_nMatchesWinText;
    [SerializeField] private Text m_nFireFighterWinText;

    public void SetNMatchesWin(int nWin)
    {
        m_nMatchesWinText.text = nWin.ToString();
    }

    public void SetNFireFighterWin(int nWin)
    {
        m_nFireFighterWinText.text = nWin.ToString();
    }
}
