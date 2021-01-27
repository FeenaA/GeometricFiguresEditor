using Assets.Scripts.FiguresOptions;
using Figures.Interfaces;
using System;
using UnityEngine;

public class SpherePanel : MonoBehaviour, IPanel
{
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

        //RadiusInput = 5d;
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
            //todo: вернуть в поле для ввода корректное значение по умолчанию
        }
    }

    public IFigure GetFigure() => new Sphere(options);
}