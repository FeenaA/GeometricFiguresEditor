using Figures.Interfaces;
using System;
using UnityEngine;

public class Demonstration : MonoBehaviour
{
    public Material material;

    public MainPanel mainPanel;

    private IPanel _panel;



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
        var myFigure = new GameObject("MyFigure");

        // dealing with MeshFilter 
        var meshFilter = myFigure.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = new Mesh();

        var figure = _panel.GetFigure();
        Mesh mesh = figure.GetMesh();

        //ParallelepipedOptions parallelepipedOptions = new ParallelepipedOptions() { depth = 5f, height = 5f, width = 5f };
        //Parallelepiped parallelepiped = new Parallelepiped(parallelepipedOptions);
        // create and fill mesh for parallelepiped
        //Mesh mesh = parallelepiped.GetFigure();

        //mesh.Optimize();
        mesh.RecalculateNormals();

        // use mesh
        meshFilter.sharedMesh = mesh;

        // MeshRenderer
        var meshRenderer = myFigure.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
    }
}
