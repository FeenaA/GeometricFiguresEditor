using Figures.Interfaces;
using System;
using UnityEngine;

public class Demonstration : MonoBehaviour
{
    public Material material;

    public MainPanel mainPanel;

    private IPanel _panel;

    private GameObject currentFigure; 

    private void Awake()
    {
        mainPanel.PanelChanged += OnPanelChanged;
    }

    private void OnPanelChanged(IPanel currentPanel)
    {
        _panel = currentPanel;
    }

    public void DrawPressed()
    {
        CreateFigure();
    }

    private void CreateFigure()
    {
        if (this.currentFigure != null)
        {
            Destroy(this.currentFigure);
        }

        currentFigure = new GameObject("MyFigure");
        currentFigure.transform.position = new Vector3(0, 0, 40);

        // dealing with MeshFilter 
        var meshFilter = currentFigure.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = new Mesh();

        var figure = _panel.GetFigure();
        Mesh mesh = figure.GetMesh();

        // use mesh
        meshFilter.sharedMesh = mesh;

        // MeshRenderer
        var meshRenderer = currentFigure.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
    }
}
