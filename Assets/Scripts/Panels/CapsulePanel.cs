using Figures.Interfaces;
using UnityEngine;

public class CapsulePanel : MonoBehaviour, IPanel
{
    public GameObject currentGO => gameObject;

    public IFigure GetFigure()
    {
        throw new System.NotImplementedException();
    }
}