using Assets.Scripts.FiguresOptions;
using Assets.Scripts.Interfaces;
using System;
using UnityEngine;
 
public class Parallelepiped : IFigure
{
	private const int count = 10; 
	private const int minCount = 3;

	private int StepWidthCount, StepHeightCount, StepDepthCount;

	private float stepSizeWidth, stepSizeHeight, stepSizeDepth;
	  
	private Vector3[] vertices;
	private int[] indexes;

	private readonly ParallelepipedOptions _options;


	public Parallelepiped(IOption option) 
	{
		if (option is ParallelepipedOptions opt)
			_options = opt;

		else
			throw new ArgumentException("Options is invalid");
	}


	/// <summary>
	/// generation of Vertices 
	/// </summary>
	/// <returns> Vector3 with Vertices </returns>
	private Vector3[] GenerateVertices() 
	{
		int cornerVertices = 8;
		int edgeVertices = (StepWidthCount + StepHeightCount + StepDepthCount - 3) * 4;
		int faceVertices = (
			(StepWidthCount - 1) * (StepHeightCount - 1) +
			(StepWidthCount - 1) * (StepDepthCount - 1) +
			(StepHeightCount - 1) * (StepDepthCount - 1)) * 2;

		// result
		vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

		int currentVertic = 0;
		GenerateVerticesWall(ref currentVertic);
		GenerateVerticesRoof(ref currentVertic);
		GenerateVerticesFloor(ref currentVertic);

		return vertices;
	}

	/// <summary>
	/// calculation of one triangular's sizes and amount of triangulars on each boarder
	/// </summary>
	private void GetStepSizes()
    {
		StepHeightCount = count;
		stepSizeHeight = _options.height / StepHeightCount;//y
		 
		StepWidthCount = Mathf.RoundToInt(_options.width / stepSizeHeight);//x
		if (StepWidthCount < minCount)
        {
			StepWidthCount = minCount;
		}
		stepSizeWidth = _options.width / StepWidthCount;

		StepDepthCount = Mathf.RoundToInt(_options.depth / stepSizeHeight);//z
		if (StepDepthCount < minCount)
		{
			StepDepthCount = minCount;
		}
		stepSizeDepth = _options.depth / StepDepthCount;
	}

	/// <summary>
	/// filling the set of vertices on walls
	/// </summary>
	/// <param name="numCurrentVertic">current amount of vertices</param>
	private void GenerateVerticesWall(ref int numCurrentVertic)
    {
		float currentHeight = 0.0f;

		// loop by layers
		for (int y = 0; y <= StepHeightCount; y++)
		{
			for (int x = 0; x <= StepWidthCount; x++)
			{
				vertices[numCurrentVertic++] = new Vector3(x * stepSizeWidth, currentHeight, 0);
			}
			for (int z = 1; z <= StepDepthCount; z++)
			{
				vertices[numCurrentVertic++] = new Vector3(_options.width, currentHeight, z * stepSizeDepth);
			}
			for (int x = StepWidthCount - 1; x >= 0; x--)
			{
				vertices[numCurrentVertic++] = new Vector3(x * stepSizeWidth, currentHeight, _options.depth);
			}
			for (int z = StepDepthCount - 1; z > 0; z--)
			{
				vertices[numCurrentVertic++] = new Vector3(0, currentHeight, z * stepSizeDepth);
			}

			currentHeight += stepSizeHeight;
		}
	}

	/// <summary>
	/// filling the set of vertices on roof
	/// </summary>
	/// <param name="v">current amount of vertices</param>
	private void GenerateVerticesRoof(ref int v)
    {
		// roof: y=height
		GenerateVerticesHorizontalPlane(_options.height, ref v); 
	}

	/// <summary>
	/// filling the set of vertices on floor
	/// </summary>
	/// <param name="v">current amount of vertices</param>
	private void GenerateVerticesFloor(ref int v)
	{
		// floor: y=0
		GenerateVerticesHorizontalPlane(0.0f, ref v);
	}

	/// <summary>
	/// filling the set of vertices on floor
	/// </summary>
	/// <param name="y">fixed height</param>
	/// <param name="v">current amount of vertices</param>
	private void GenerateVerticesHorizontalPlane(float y, ref int v)
	{
		for (int z = 1; z < StepDepthCount; z++)
		{
			for (int x = 1; x < StepWidthCount; x++)
			{
				vertices[v++] = new Vector3(x * stepSizeWidth, y, z * stepSizeDepth);
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="i">current amount of indexes</param>
	/// <param name="v00"></param>
	/// <param name="v10"></param>
	/// <param name="v01"></param>
	/// <param name="v11"></param>
	/// <returns></returns>
	private int SetQuad(int i, int v00, int v10, int v01, int v11)
	{
		indexes[i] = v00;
		indexes[i + 1] = indexes[i + 4] = v01;
		indexes[i + 2] = indexes[i + 3] = v10;
		indexes[i + 5] = v11;
		return i + 6;
	}

	/// <summary>
	/// generation of indexes 
	/// </summary>
	/// <returns> Array of Indexes in the correct direction </returns>
	private int[] GenerateIndexes() 
	{
		// one quad contains two triangles ( i.e. 6 indexes )
		int quads = (StepWidthCount * StepHeightCount + StepWidthCount * StepDepthCount + StepHeightCount * StepDepthCount) * 2;
		indexes = new int[quads * 6];

		// amount of vertices on a ring 
		int ring = (StepWidthCount + StepDepthCount) * 2;
		int currentIndex = 0; 
		// left low corner
		int v = 0;

		// loop by layers
		for (int y = 0; y < StepHeightCount; y++, v++)
		{
			for (int q = 0; q < ring - 1; q++, v++)
			{
				currentIndex = SetQuad(currentIndex, v, v + 1, v + ring, v + ring + 1);
			}
			currentIndex = SetQuad(currentIndex, v, v - ring + 1, v + ring, v + 1);
		}

		currentIndex = GenerateRoofIndexes(currentIndex, ring);
		GenerateFloorIndexes(currentIndex, ring);

		return indexes;
	}

	/// <summary>
	/// generation of indexes on roof
	/// </summary>
	/// <param name="t"></param>
	/// <param name="ring"></param>
	/// <returns></returns>
	private int GenerateRoofIndexes(int t, int ring)
	{
		// first line
		int v = ring * StepHeightCount;
		for (int x = 0; x < StepWidthCount - 1; x++, v++)
		{
			t = SetQuad(t, v, v + 1, v + ring - 1, v + ring);
		}
		t = SetQuad(t, v, v + 1, v + ring - 1, v + 2);

		// middle part
		int vMin = ring * (StepHeightCount + 1) - 1;
		int vMid = vMin + 1;
		int vMax = v + 2;

		for (int z = 1; z < StepDepthCount - 1; z++, vMin--, vMid++, vMax++)
		{
			t = SetQuad(t, vMin, vMid, vMin - 1, vMid + StepWidthCount - 1);
			for (int x = 1; x < StepWidthCount - 1; x++, vMid++)
			{
				t = SetQuad( t, vMid, vMid + 1, vMid + StepWidthCount - 1, vMid + StepWidthCount);
			}
			t = SetQuad( t, vMid, vMax, vMid + StepWidthCount - 1, vMax + 1);
		}

		// last line
		int vTop = vMin - 2;
		t = SetQuad(t, vMin, vMid, vMin - 1, vMin - 2);
		for (int x = 1; x < StepWidthCount - 1; x++, vTop--, vMid++)
		{
			t = SetQuad( t, vMid, vMid + 1, vTop, vTop - 1);
		}
		t = SetQuad( t, vMid, vTop - 2, vTop, vTop - 1);

		return t;
	}

	/// <summary>
	/// generation of indexes on floor
	/// </summary>
	/// <param name="t"></param>
	/// <param name="ring"></param>
	/// <returns></returns>
	private int GenerateFloorIndexes(int t, int ring)
    {
		// first line
		int v = 1;
		int vMid = vertices.Length - (StepWidthCount - 1) * (StepDepthCount - 1);
		t = SetQuad(t, ring - 1, vMid, 0, 1);
		for (int x = 1; x < StepWidthCount - 1; x++, v++, vMid++)
		{
			t = SetQuad(t, vMid, vMid + 1, v, v + 1);
		}
		t = SetQuad(t, vMid, v + 2, v, v + 1);

		int vMin = ring - 2;
		vMid -= StepWidthCount - 2;
		int vMax = v + 2;

		// middle part
		for (int z = 1; z < StepWidthCount - 1; z++, vMin--, vMid++, vMax++)
		{
			t = SetQuad(t, vMin, vMid + StepWidthCount - 1, vMin + 1, vMid);
			for (int x = 1; x < StepWidthCount - 1; x++, vMid++)
			{
				t = SetQuad( t, vMid + StepWidthCount - 1, vMid + StepWidthCount, vMid, vMid + 1);
			}
			t = SetQuad( t, vMid + StepWidthCount - 1, vMax + 1, vMid, vMax);
		}

		// last line
		int vTop = vMin - 1;
		t = SetQuad( t, vTop + 1, vTop, vTop + 2, vMid);
		for (int x = 1; x < StepWidthCount - 1; x++, vTop--, vMid++)
		{
			t = SetQuad( t, vTop, vTop - 1, vMid, vMid + 1);
		}
		t = SetQuad( t, vTop, vTop - 1, vMid, vTop - 2);

		return t;
	}

	/// <summary>
	/// mesh generation 
	/// </summary>
	/// <param name="option">Parametres of parallelepiped</param>
	/// <returns></returns>
	public Mesh GetMesh()
    {
		// calculate amount of triangulas and sizes of one step
		GetStepSizes();

		Mesh mesh = new Mesh
        {
            vertices = GenerateVertices(),
            triangles = GenerateIndexes()
        };

        mesh.RecalculateNormals();
		return mesh;
    }
}
