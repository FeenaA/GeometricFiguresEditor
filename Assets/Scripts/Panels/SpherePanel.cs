using Assets.Scripts.FiguresOptions;
using Figures.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SpherePanel : MonoBehaviour, IPanel
{
    public InputField inputFieldRadius;
    public InputField inputFieldSectors; 

    private readonly float defaultRadius = 15f;
    private readonly int defaultSectors = 20;

    private enum SphereOptionsFields
    {
        Radius, 
        Sectors,
    }

    private SphereOptions options;
    public GameObject currentGO => gameObject;

    private void Awake()
    {
        options = new SphereOptions();

        inputFieldRadius.text = defaultRadius.ToString();
        inputFieldSectors.text = defaultSectors.ToString();
    }

    public void OnRadiusChanged(string value) => ChangeOptionsValue(value, SphereOptionsFields.Radius);

    public void OnSectorsChanged(string value) => ChangeOptionsValue(value, SphereOptionsFields.Sectors);

    private void ChangeOptionsValue(string value, SphereOptionsFields fieldName)
    {
        if (Int32.TryParse(value, out int res))
        {
            switch (fieldName)
            {
                case SphereOptionsFields.Radius:
                    options.radius = res;
                    break;
                case SphereOptionsFields.Sectors:
                    options.sectors = res;
                    break;
            }
        }
        else
        {
            switch (fieldName)
            {
                case SphereOptionsFields.Radius:
                    {
                        inputFieldRadius.text = defaultRadius.ToString();
                        options.radius = defaultRadius;
                        break;
                    }
                case SphereOptionsFields.Sectors:
                    {
                        inputFieldSectors.text = defaultSectors.ToString();
                        options.sectors = defaultSectors;
                        break;
                    }
            }
        }
    }

    public IFigure GetFigure() => new Sphere(options);
}