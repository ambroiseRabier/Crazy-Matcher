using UnityEngine;
using UnityEngine.UI;

public class VSIntroductionScreen : CanvasScreen<VSIntroductionScreen> {
    [SerializeField] private Sprite m_mamySprite;
    [SerializeField] private Sprite m_matchesSprite;

    [SerializeField] private Image m_p1Image;
    [SerializeField] private Image m_p2Image;

    public void SetP1Team(GameManager.Team p1Team)
    {
        SetTeamSpriteOnImage(p1Team, m_p1Image);
    }

    public void SetP2Team(GameManager.Team p2Team)
    {
        SetTeamSpriteOnImage(p2Team, m_p2Image);
    }

    private void SetTeamSpriteOnImage(GameManager.Team team, Image image)
    {
        image.sprite = (team == GameManager.Team.MATCHES) ? m_matchesSprite : m_mamySprite;
    }
}
