using Assets.Scripts.FiguresOptions;
using Assets.Scripts.Interfaces;
using System;
using UnityEngine;

public class Prism : IFigure
{
    private readonly PrismOptions _options;

    private readonly int count = 5;
    private readonly int minCount = 3;

    private int StepHeightCount;
    private int StepWidthCount;
    private int StepRadiusCount;

    private Vector3[] vertices;
    private int[] indexes;
    private float stepSizeHeight;
    private float stepSizeRadius;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="option"></param>
    public Prism(IOption option)
    {
        if (option is PrismOptions opt)
            _options = opt;

        else
            throw new ArgumentException("Options is invalid");
    }

    /// <summary>
    /// calculate stepSizeHeight, StepHeightCount, 
    /// </summary>
    private void GetStepSize()
    {
        stepSizeHeight = _options.height / count;
        StepHeightCount = count + 1;

        float width = _options.radius * Mathf.Sin(Mathf.PI / _options.faces);
        StepWidthCount = Mathf.RoundToInt(width / stepSizeHeight);//x
        if (StepWidthCount < minCount)
        {
            StepWidthCount = minCount;
        }

        StepRadiusCount = Mathf.RoundToInt(_options.radius / stepSizeHeight);
        if (StepRadiusCount < 1)
        {
            StepRadiusCount = 1;
        }
        stepSizeRadius = _options.radius / StepRadiusCount;
    }

    /// <summary>
    /// Generation vertices
    /// </summary>
    /// <returns>vertices</returns>
    private Vector3[] GenerateVertices()
    {
        int faces = _options.faces;
        int horisontalVertices = 0;
        for (int i = 0; i < StepRadiusCount; i++)
        {
            horisontalVertices += faces * i;
        }
        vertices = new Vector3[faces * StepWidthCount * StepHeightCount + horisontalVertices];

        int numCurrentVertic = 0;

        GenerateVerticesWalls(ref numCurrentVertic);
        GenerateVerticesFloor(ref numCurrentVertic);
        GenerateVerticesRoof(ref numCurrentVertic);

        return vertices;
    }

    private void GenerateVerticesFloor(ref int numCurrentVertic)
    {
        GenerateVerticesHorizontalPlane(ref numCurrentVertic, 0f);
    }

    private void GenerateVerticesRoof(ref int numCurrentVertic)
    {
        GenerateVerticesHorizontalPlane(ref numCurrentVertic, _options.height);
    }

    private void GenerateVerticesHorizontalPlane(ref int numCurrentVertic, float height)
    {
     /*   vertices[numCurrentVertic++] = new Vector3(0f, height, 0f); // centre

        float multiplier = 2f * Mathf.PI / _options.faces;
        float radius = stepSizeRadius;
        float faces = _options.faces;

        // loop by radius
        for (int r = 1; r < StepRadiusCount; r++, radius += stepSizeRadius)
        {
            // loop by faces
            for (int i = 0; i < faces; i++)
            {
                float currentWidth = radius * Mathf.Sin(i * multiplier);
                float currentDepth = radius * Mathf.Cos(i * multiplier);

                float nextWidth = radius * Mathf.Sin((i + 1) * multiplier);
                float nextDepth = radius * Mathf.Cos((i + 1) * multiplier);

                float stepSizeWidth = (nextWidth - currentWidth) / StepWidthCount;
                float stepSizeDepth = (nextDepth - currentDepth) / StepWidthCount;

                // loop by width 
                for (int w = 0; w < r * faces; w++)
                {
                    vertices[numCurrentVertic++] = new Vector3(currentWidth, height, currentDepth);
                    currentWidth += stepSizeWidth;
                    currentDepth += stepSizeDepth;
                }
            }
        }*/
    }

    private void GenerateVerticesWalls(ref int numCurrentVertic)
    {
        float currentHeight = 0.0f;
        float multiplier = 2f * Mathf.PI / _options.faces;
        float faces = _options.faces;
        float radius = _options.radius;


        // loop by layers
        for (int y = 0; y < StepHeightCount; y++, currentHeight += stepSizeHeight)
        {
            // loop by faces
            for (int i = 0; i < faces; i++)
            {
                float currentWidth = radius * Mathf.Sin(i * multiplier);
                float currentDepth = radius * Mathf.Cos(i * multiplier);

                float nextWidth = radius * Mathf.Sin((i + 1) * multiplier);
                float nextDepth = radius * Mathf.Cos((i + 1) * multiplier);

                float stepSizeWidth = (nextWidth - currentWidth) / StepWidthCount;
                float stepSizeDepth = (nextDepth - currentDepth) / StepWidthCount;

                // loop by width 
                for (int w = 0; w < StepWidthCount; w++)
                {
                    vertices[numCurrentVertic++] = new Vector3(currentWidth, currentHeight, currentDepth);
                    currentWidth += stepSizeWidth;
                    currentDepth += stepSizeDepth;
                }
            }
        }
    }

    private int[] GenerateIndexes()
    {
        int quads = _options.faces * StepHeightCount * (StepWidthCount + 1);
        //---
        int triangles = 0;
        indexes = new int[triangles * 3 + quads * 6];
        int currentIndex = 0;

        GenerateIndexesWalls(ref currentIndex);
        //GenerateIndexesFloor(ref currentIndex);
        //GenerateIndexesRoof(ref currentIndex);

        return indexes;
    }

    private void GenerateIndexesWalls(ref int currentIndex)
    {
        int faces = _options.faces;
        // amount of vertices on a ring 
        int ring = StepWidthCount * faces;
        // left low corner
        int v = 0;

        // loop by layers
        for (int y = 0; y < StepHeightCount - 1; y++, v++)
        {
            for (int q = 0; q < ring - 1; q++, v++)
            {
                currentIndex = SetQuad(currentIndex, v, v + 1, v + ring, v + ring + 1);
            }
            currentIndex = SetQuad(currentIndex, v, v - ring + 1, v + ring, v + 1);
        }
    }


    /*
    private Vector3[] GenerateVertices()
    {

        int faces = _options.faces;
        vertices = new Vector3[faces * 2 + 2];

        int numCurrentVertic = 0;

        GenerateVerticesFloor(ref numCurrentVertic);
        GenerateVerticesRoof(ref numCurrentVertic);

        return vertices;
    }

    private void GenerateVerticesFloor(ref int numCurrentVertic)
    {
        GenerateVerticesHorizontalPlane(ref numCurrentVertic, 0f);
    }

    private void GenerateVerticesRoof(ref int numCurrentVertic)
    {
        GenerateVerticesHorizontalPlane(ref numCurrentVertic, _options.height);
    }

    private void GenerateVerticesHorizontalPlane(ref int numCurrentVertic, float height)
    {
        vertices[numCurrentVertic++] = new Vector3(0f, height, 0f); // centre

        float multiplier = 2f * Mathf.PI / _options.faces;
        float radius = _options.radius;

        for (int i = 0; i < _options.faces; i++)
        {
            vertices[numCurrentVertic++] = new Vector3(radius * Mathf.Sin(i * multiplier), height, radius * Mathf.Cos(i * multiplier));
        }
    }

    private int[] GenerateIndexes()
    {
        indexes = new int[_options.faces * 12];
        int currentIndex = 0;

        GenerateIndexesWalls(ref currentIndex);
        GenerateIndexesFloor(ref currentIndex); 
        GenerateIndexesRoof(ref currentIndex); 

        return indexes;
    }

    private void GenerateIndexesWalls(ref int currentIndex)
    {
        int faces = _options.faces;
        int v = 1;
        for (int i = 0; i < faces - 1; i++, v++)
        {
            currentIndex = SetQuad(currentIndex, v, v + 1, v + faces + 1, v + faces + 2);
        }
        currentIndex = SetQuad(currentIndex, v, v - faces + 1, v + faces + 1, v + 2);
    }

    private void GenerateIndexesFloor(ref int currentIndex)
    {
        GenerateIndexesHorizontalPlane(ref currentIndex, 0); 
    }
    private void GenerateIndexesRoof(ref int currentIndex)
    {
        GenerateIndexesHorizontalPlane(ref currentIndex, _options.faces + 1);
    }

    private void GenerateIndexesHorizontalPlane(ref int currentIndex, int heightIndex)
    {
        int faces = _options.faces;
        for (int i = 1; i < faces; i++)
        {
            indexes[currentIndex++] = heightIndex;
            indexes[currentIndex++] = heightIndex + i;
            indexes[currentIndex++] = heightIndex + i + 1;
        }
        indexes[currentIndex++] = heightIndex;
        indexes[currentIndex++] = heightIndex + faces;
        indexes[currentIndex++] = heightIndex + 1;
    }*/

    private int SetQuad(int i, int v00, int v10, int v01, int v11)
    {
        indexes[i] = indexes[i + 3] = v00;
        indexes[i + 1] = v10;
        indexes[i + 2] = indexes[i + 4] = v11;
        indexes[i + 5] = v01;
        return i + 6;
    }


    /// <summary>
    /// mesh generation 
    /// </summary>
    /// <param name="option">Parametres of prism</param>
    /// <returns></returns>
    public Mesh GetMesh()
    {
        GetStepSize();

        Mesh mesh = new Mesh
        {
            vertices = GenerateVertices(),
            triangles = GenerateIndexes()
        };

        mesh.RecalculateNormals();
        return mesh;
    }
}
