using UnityEngine;

public class WinScreen : CanvasScreen<WinScreen> {
    [SerializeField] private GameObject m_mamyPanel;
    [SerializeField] private GameObject m_matchesPanel;

    public void SetWinnerTeam(GameManager.Team team)
    {
        m_mamyPanel.SetActive(team == GameManager.Team.FIRE_FIGHTER);
        m_matchesPanel.SetActive(team == GameManager.Team.MATCHES);
    }
}
