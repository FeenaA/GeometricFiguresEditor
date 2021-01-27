using UnityEngine;

namespace Figures.Interfaces
{
    public interface IPanel
    {
        GameObject currentGO { get; }

        IFigure GetFigure();
    }
}