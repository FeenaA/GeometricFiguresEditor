using Assets.Scripts.FiguresOptions;
using Assets.Scripts.Interfaces;
using System;
using UnityEngine;

public class Sphere : IFigure
{
	private readonly SphereOptions _options;
	private float gridSize;
	private float diameter; 

	private Vector3[] vertices;
	private Vector3[] normals;

	int[] trianglesZ;
	int[] trianglesX;
	int[] trianglesY;

	/// <summary>
	/// constructor
	/// </summary>
	/// <param name="option">initial conditions of a sphere</param>
	public Sphere(IOption option)
	{
		if (option is SphereOptions opt)
			_options = opt;

		else
			throw new ArgumentException("Options is invalid");
	}

	/// <summary>
	/// calculate diameter
	/// </summary>
	private void GetDiameter()
    {
		diameter = _options.radius * 2f;
	}

	/// <summary>
	/// gridSize calculation
	/// </summary>
	private void GetGridSize()
	{
		gridSize = diameter / _options.sectors;
	}

	private Vector3[] GenerateVertices()
    {
		// amount of sectors
		int sectorCounter = _options.sectors;
		int cornerVertices = 8;
		int edgeVertices = (sectorCounter * 2 + sectorCounter - 3) * 4;
		int faceVertices = (sectorCounter - 1) * (sectorCounter - 1) * 6;

		// result
		vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
		normals = new Vector3[vertices.Length];

		int currentVertic = 0;
		// walls
		for (int y = 0; y <= sectorCounter; y++)
		{
			float currentHeight = y * gridSize;

			for (int x = 0; x <= sectorCounter; x++)
			{
				SetVertex(currentVertic++, x, currentHeight, 0);
			}
			for (int z = 1; z <= sectorCounter; z++)
			{
				SetVertex(currentVertic++, sectorCounter, currentHeight, z);
			}
			for (int x = sectorCounter - 1; x >= 0; x--)
			{
				SetVertex(currentVertic++, x, currentHeight, sectorCounter);
			}
			for (int z = sectorCounter - 1; z > 0; z--)
			{
				SetVertex(currentVertic++, 0, currentHeight, z);
			}
		}
		// roof
		for (int z = 1; z < sectorCounter; z++)
		{
			for (int x = 1; x < sectorCounter; x++)
			{
				SetVertex(currentVertic++, x, sectorCounter, z);
			}
		}
		// floor
		for (int z = 1; z < sectorCounter; z++)
		{
			for (int x = 1; x < sectorCounter; x++)
			{
				SetVertex(currentVertic++, x, 0, z);
			}
		}

		return vertices;
	}

	private void GenerateIndexes() //Triangles
	{
		int sectorCounter = _options.sectors;

		trianglesZ = new int[(sectorCounter * sectorCounter) * 12];
		trianglesX = new int[(sectorCounter * sectorCounter) * 12];
		trianglesY = new int[(sectorCounter * sectorCounter) * 12];
		int ring = (sectorCounter + sectorCounter) * 2;
		int tZ = 0, tX = 0, tY = 0, v = 0;

		for (int y = 0; y < sectorCounter; y++, v++)
		{
			for (int q = 0; q < sectorCounter; q++, v++)
			{
				tZ = SetQuad(ref trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
			}
			for (int q = 0; q < sectorCounter; q++, v++)
			{
				tX = SetQuad(ref trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
			}
			for (int q = 0; q < sectorCounter; q++, v++)
			{
				tZ = SetQuad(ref trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
			}
			for (int q = 0; q < sectorCounter - 1; q++, v++)
			{
				tX = SetQuad(ref trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
			}
			tX = SetQuad(ref trianglesX, tX, v, v - ring + 1, v + ring, v + 1);
		}

		tY = CreateTopFace(ref trianglesY, tY, ring);
		GenerateFloorIndexes(ref trianglesY, tY, ring);
	}

	private int CreateTopFace(ref int[] trianglesY, int tY, int ring)
    {

		return tY;
    }

	private int GenerateFloorIndexes(ref int[] trianglesY, int t, int ring)
	{
		int sectorsCount = _options.sectors;

		// first line
		int v = 1;
		int vMid = vertices.Length - (sectorsCount - 1) * (sectorsCount - 1);
		t = SetQuad(ref trianglesY, t, ring - 1, vMid, 0, 1);
		for (int x = 1; x < sectorsCount - 1; x++, v++, vMid++)
		{
			t = SetQuad(ref trianglesY, t, vMid, vMid + 1, v, v + 1);
		}
		t = SetQuad(ref trianglesY, t, vMid, v + 2, v, v + 1);

		int vMin = ring - 2;
		vMid -= sectorsCount - 2;
		int vMax = v + 2;

		// middle part
		for (int z = 1; z < sectorsCount - 1; z++, vMin--, vMid++, vMax++)
		{
			t = SetQuad(ref trianglesY, t, vMin, vMid + sectorsCount - 1, vMin + 1, vMid);
			for (int x = 1; x < sectorsCount - 1; x++, vMid++)
			{
				t = SetQuad(ref trianglesY, t, vMid + sectorsCount - 1, vMid + sectorsCount, vMid, vMid + 1);
			}
			t = SetQuad(ref trianglesY, t, vMid + sectorsCount - 1, vMax + 1, vMid, vMax);
		}

		// last line
		int vTop = vMin - 1;
		t = SetQuad(ref trianglesY, t, vTop + 1, vTop, vTop + 2, vMid);
		for (int x = 1; x < sectorsCount - 1; x++, vTop--, vMid++)
		{
			t = SetQuad(ref trianglesY, t, vTop, vTop - 1, vMid, vMid + 1);
		}
		t = SetQuad(ref trianglesY, t, vTop, vTop - 1, vMid, vTop - 2);

		return t;
	}

	private int SetQuad(ref int[] indexes, int i, int v00, int v10, int v01, int v11)
	{
		indexes[i] = v00;
		indexes[i + 1] = indexes[i + 4] = v01;
		indexes[i + 2] = indexes[i + 3] = v10;
		indexes[i + 5] = v11;
		return i + 6;
	}

	private Vector3[] GenerateNormals()
    {
		return normals;
	}

	private void SetVertex(int currentVertic, float x, float y, float z)
	{
		Vector3 v = new Vector3(x, y, z) * 2f / gridSize - Vector3.one;
		normals[currentVertic] = v.normalized;
		vertices[currentVertic] = normals[currentVertic] * _options.radius;
	}

	/// <summary>
	/// mesh generation 
	/// </summary>
	/// <param name="option">Parametres of Sphere</param>
	/// <returns></returns>
	public Mesh GetMesh()
	{
		// calculate diameter
		GetDiameter();
		// calculate size of one step
		GetGridSize();

		Mesh mesh = new Mesh
		{
			vertices = GenerateVertices(),
			normals = GenerateNormals(),
		};

		GenerateIndexes();
		mesh.subMeshCount = 3;
		mesh.SetTriangles(trianglesZ, 0);
		mesh.SetTriangles(trianglesX, 1);
		mesh.SetTriangles(trianglesY, 2);

		mesh.RecalculateNormals();
		return mesh;
	}
}