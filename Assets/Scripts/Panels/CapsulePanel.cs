using Assets.Scripts.Figures;
using Assets.Scripts.FiguresOptions;
using Figures.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CapsulePanel : MonoBehaviour, IPanel
{
    public InputField inputFieldHeight;
    public InputField inputFieldFaces;
    public InputField inputFieldRadius;

    private readonly float defaultHeight = 20f;
    private readonly int defaultFaces = 6;
    private readonly float defaultRadius = 10f;

    private enum CapsuleOptionsFields
    {
        Height, 
        Faces,
        Radius,
    }

    private CapsuleOptions options;

    public GameObject currentGO => gameObject;

    private void Awake()
    {
        options = new CapsuleOptions();

        inputFieldHeight.text = defaultHeight.ToString();
        inputFieldFaces.text = defaultFaces.ToString();
        inputFieldRadius.text = defaultRadius.ToString();
    }

    public void OnHeightChanged(string value) => ChangeOptionsValue(value, CapsuleOptionsFields.Height);

    public void OnFacesChanged(string value) => ChangeOptionsValue(value, CapsuleOptionsFields.Faces);

    public void OnRadiusChanged(string value) => ChangeOptionsValue(value, CapsuleOptionsFields.Radius);

    private void ChangeOptionsValue(string value, CapsuleOptionsFields fieldName)
    {
        if (Int32.TryParse(value, out int res))
        {
            switch (fieldName)
            {
                case CapsuleOptionsFields.Height:
                    options.height = res;
                    break;
                case CapsuleOptionsFields.Faces:
                    options.faces = res;
                    break;
                case CapsuleOptionsFields.Radius:
                    options.radius = res;
                    break;
            }
        }
        else
        {
            switch (fieldName)
            {
                case CapsuleOptionsFields.Height:
                    {
                        options.height = defaultHeight;
                        inputFieldHeight.text = defaultHeight.ToString();
                        break;
                    }
                case CapsuleOptionsFields.Faces:
                    {
                        inputFieldFaces.text = defaultFaces.ToString();
                        options.faces = defaultFaces;
                        break;
                    }
                case CapsuleOptionsFields.Radius:
                    {
                        inputFieldRadius.text = defaultRadius.ToString();
                        options.radius = defaultRadius;
                        break;
                    }
            }
        }
    }

    /*public IFigure GetFigure()
    {
        throw new System.NotImplementedException();
    }*/

    public IFigure GetFigure() => new Capsule(options);
}