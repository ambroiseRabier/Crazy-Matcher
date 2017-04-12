using UnityEngine;
using UnityEngine.UI;

public class WinScreen : CanvasScreen {
    [SerializeField] private GameObject m_mamyPanel;
    [SerializeField] private GameObject m_matchesPanel;

    public void SetWinnerTeam(GameManager.Team team)
    {
        m_mamyPanel.SetActive(team == GameManager.Team.FIRE_FIGHTER);
        m_matchesPanel.SetActive(team == GameManager.Team.MATCHES);
    }
}
