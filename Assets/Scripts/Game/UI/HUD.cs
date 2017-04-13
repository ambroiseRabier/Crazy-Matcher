using UnityEngine;
using UnityEngine.UI;

public class HUD : CanvasScreen<HUD>
{
    [SerializeField] private UIPlayerScore m_p1ScoreComp;
    [SerializeField] private UIPlayerScore m_p2ScoreComp;
    [SerializeField] private Slider m_waterTankSlider;

    public UIPlayerScore P1ScoreComp { get { return m_p1ScoreComp; } }
    public UIPlayerScore P2ScoreComp { get { return m_p2ScoreComp; } }
    public Slider WaterTankSlider { get { return m_waterTankSlider; } }
}
