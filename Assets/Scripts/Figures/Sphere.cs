using Assets.Scripts.FiguresOptions;
using Assets.Scripts.Interfaces;
using System;
using UnityEngine;

public class Sphere : IFigure
{
	private readonly SphereOptions _options;

	private readonly int layers = 25;

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
	/// Vertics generation
	/// </summary>
	/// <returns>vertices</returns>
	private Vector3[] GenerateVertics() 
	{
		int sectors = _options.sectors;

		Vector3[] vertices = new Vector3[(layers + 1) * (sectors + 1)];

		// "плоские" вершины
		for (int i = 0; i < vertices.Length; i++)
		{
			float x = i % (sectors + 1);
			float y = i / (sectors + 1);
			float x_pos = x / sectors;
			float y_pos = y / layers;
			vertices[i] = new Vector3(x_pos, y_pos, 0);
		}

		// выт€гивание по формуле поверхности сферы
		float radius = _options.radius;
		float pi = Mathf.PI;
		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 v;
			v.x = radius * Mathf.Cos(vertices[i].x * 2 * pi) * Mathf.Cos(vertices[i].y * pi - pi / 2);
			v.y = radius * Mathf.Sin(vertices[i].x * 2 * pi) * Mathf.Cos(vertices[i].y * pi - pi / 2);
			v.z = radius * Mathf.Sin(vertices[i].y * pi - pi / 2);

			vertices[i] = v;
		}

		return vertices;
	}

	/// <summary>
	/// triangles generation
	/// </summary>
	/// <returns>triangles</returns>
	private int[] GenerateTriangles()
    {
		int[] triangles;
		int sectors = _options.sectors;
		triangles = new int[6 * layers * sectors];// 6 = 3 (количество углов треугольника) * 2 (количество треугольников в квадрате)

		for (int i = 0; i < 2 * layers * sectors; i++)
		{
			int[] triIndex = new int[3];
			if (i % 2 == 0) //ч
			{
				triIndex[0] = i / 2 + i / (2 * sectors);
				triIndex[1] = triIndex[0] + 1;
				triIndex[2] = triIndex[0] + (sectors + 1);
			}
			else //н
			{
				triIndex[0] = (i + 1) / 2 + i / (2 * sectors);
				triIndex[1] = triIndex[0] + (sectors + 1);
				triIndex[2] = triIndex[1] - 1;

			}
			triangles[i * 3] = triIndex[0];
			triangles[i * 3 + 1] = triIndex[1];
			triangles[i * 3 + 2] = triIndex[2];
		}
		return triangles;
	}

	/// <summary>
	/// mesh generation 
	/// </summary>
	/// <param name="option">Parametres of Sphere</param>
	/// <returns></returns>
	public Mesh GetMesh()
	{
		Mesh mesh = new Mesh { };

		mesh.vertices = GenerateVertics();
		mesh.triangles = GenerateTriangles();
		mesh.RecalculateNormals();
		return mesh;
	}
}