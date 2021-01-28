using Assets.Scripts.FiguresOptions;
using Figures.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PrismPanel : MonoBehaviour, IPanel
{
    public InputField inputFieldHeight;
    public InputField inputFieldFaces; 
    public InputField inputFieldRadius;

    private readonly float defaultHeight = 20f;
    private readonly int defaultFaces = 6;
    private readonly float defaultRadius = 10f;

    private enum PrismOptionsFields
    {
        Height,
        Faces,
        Radius,  
    }

    private PrismOptions options;

    public GameObject currentGO => gameObject;

    private void Awake()
    {
        options = new PrismOptions();

        inputFieldHeight.text = defaultHeight.ToString();
        inputFieldFaces.text = defaultFaces.ToString();
        inputFieldRadius.text = defaultRadius.ToString();
    }

    public void OnHeightChanged(string value) => ChangeOptionsValue(value, PrismOptionsFields.Height);

    public void OnFacesChanged(string value) => ChangeOptionsValue(value, PrismOptionsFields.Faces);

    public void OnRadiusChanged(string value) => ChangeOptionsValue(value, PrismOptionsFields.Radius);
     
    private void ChangeOptionsValue(string value, PrismOptionsFields fieldName)
    {
        if (Int32.TryParse(value, out int res))
        {
            switch (fieldName)
            {
                case PrismOptionsFields.Height:
                    options.height = res;
                    break;
                case PrismOptionsFields.Faces:
                    options.faces = res;
                    break;
                case PrismOptionsFields.Radius:
                    options.radius = res;
                    break;
            }
        }
        else
        {
            switch (fieldName)
            {
                case PrismOptionsFields.Height:
                    {
                        options.height = defaultHeight;
                        inputFieldHeight.text = defaultHeight.ToString();
                        break;
                    }
                case PrismOptionsFields.Faces:
                    {
                        inputFieldFaces.text = defaultFaces.ToString();
                        options.faces = defaultFaces;
                        break;
                    }
                case PrismOptionsFields.Radius:
                    {
                        inputFieldRadius.text = defaultRadius.ToString();
                        options.radius = defaultRadius; 
                        break;
                    }
            }
        }
    }

    public IFigure GetFigure() => new Prism(options); 

}