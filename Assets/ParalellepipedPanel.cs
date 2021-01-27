using Assets.Scripts.FiguresOptions;
using Figures.Interfaces;
using System;
using UnityEngine;

public class ParalellepipedPanel : MonoBehaviour, IPanel
{
    private enum ParalellepipedOptionsFields
    {
        Height,
        Width,
        Depth,
    }

    private ParallelepipedOptions options;

    public GameObject currentGO => gameObject;

    private void Awake()
    {
        options = new ParallelepipedOptions();

        //heightInput = 5d;
    }

    public void OnHeightChanged(string value) => ChangeOptionsValue(value, ParalellepipedOptionsFields.Height);

    public void OnWidthChanged(string value) => ChangeOptionsValue(value, ParalellepipedOptionsFields.Width);

    public void OnDepthChanged(string value) => ChangeOptionsValue(value, ParalellepipedOptionsFields.Depth);

    private void ChangeOptionsValue(string value, ParalellepipedOptionsFields fieldName)
    {
        if (Int32.TryParse(value, out int res))
        {
            switch (fieldName)
            {
                case ParalellepipedOptionsFields.Height:
                    options.height = res;
                    break;
                case ParalellepipedOptionsFields.Width:
                    options.width = res;
                    break;
                case ParalellepipedOptionsFields.Depth:
                    options.depth = res;
                    break;
            }
        }

        else
        {
            //todo: вернуть в поле для ввода корректное значение по умолчанию
        }
    }

    public IFigure GetFigure() => new Parallelepiped(options);
}
