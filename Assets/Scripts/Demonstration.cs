using Assets.Scripts.FiguresOptions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Demonstration : MonoBehaviour
{
    public Material material;

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

        // --- избавиться от уточнения фигуры
        ParallelepipedOptions parallelepipedOptions = new ParallelepipedOptions() { depth = 5f, height = 5f, width = 5f };
        Parallelepiped parallelepiped = new Parallelepiped(parallelepipedOptions);

        // create and fill mesh
        Mesh mesh = parallelepiped.GetFigure();

        //mesh.Optimize();
        mesh.RecalculateNormals();

        // use mesh
        meshFilter.sharedMesh = mesh;

        // MeshRenderer
        var meshRenderer = myFigure.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
    }
}
