using Assets.Scripts.FiguresOptions;
using Figures.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ParalellepipedPanel : MonoBehaviour, IPanel
{
    public InputField inputFieldHeight;
    public InputField inputFieldWidth;
    public InputField inputFieldDepth; 

    private readonly float defaultHeight = 15f;
    private readonly float defaultWidth = 15f;
    private readonly float defaultDepth = 15f; 

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

        inputFieldHeight.text = defaultHeight.ToString();
        inputFieldWidth.text = defaultWidth.ToString();
        inputFieldDepth.text = defaultDepth.ToString();
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
            switch (fieldName)
            {
                case ParalellepipedOptionsFields.Height:
                    {
                        options.height = defaultHeight;
                        inputFieldHeight.text = defaultHeight.ToString();
                        break;
                    }
                case ParalellepipedOptionsFields.Width:
                    {
                        inputFieldWidth.text = defaultWidth.ToString();
                        options.width = defaultWidth;
                        break;
                    }
                case ParalellepipedOptionsFields.Depth:
                    {
                        inputFieldDepth.text = defaultDepth.ToString();
                        options.depth = defaultDepth;
                        break;
                    }
            }
        }
    }

    public IFigure GetFigure() => new Parallelepiped(options);
}
