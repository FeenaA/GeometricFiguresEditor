using Assets.Scripts.FiguresOptions;
using Assets.Scripts.Interfaces;
using Figures.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    public Button buttonParallelepiped;
    public Button buttonSphere;
    public Button buttonPrism;
    public Button buttonCapsule;

    private Button currentButton;

    public ParalellepipedPanel paralellepipedPanel;
    public SpherePanel spherePanel;
    public PrismPanel prismPanel;
    public CapsulePanel capsulePanel;

    public event UnityAction<IPanel> PanelChanged;

    private IPanel _currentPanel;



    private void Start()
    {
        ParallelepipedClicked();
    }

    public void ParallelepipedClicked()
    {
        OnClick(paralellepipedPanel, buttonParallelepiped);
    }

    public void SphereClicked()
    {
        OnClick(spherePanel, buttonSphere);
    }

    public void PrismClicked()
    {
        OnClick(prismPanel, buttonPrism);
    }

    public void CapsuleClicked()
    {
        OnClick(capsulePanel, buttonCapsule);
    }

    private void OnClick(IPanel panel, Button button)
    {
        _currentPanel?.currentGO.SetActive(false);
        if (currentButton != null)
        {
            currentButton.interactable = true;
        }    

        _currentPanel = panel;
        _currentPanel.currentGO.SetActive(true);

        currentButton = button;
        currentButton.interactable = false;

        PanelChanged?.Invoke(_currentPanel);
    }
}
