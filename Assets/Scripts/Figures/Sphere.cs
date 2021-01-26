using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.FiguresOptions;
using Assets.Scripts.Interfaces;
using System;
using UnityEngine;

public class Sphere : IFigure
{

	private readonly SphereOptions _options;

	public Sphere(IOption option)
	{
		if (option is SphereOptions opt)
			_options = opt;

		else
			throw new ArgumentException("Options is invalid");
	}

	/// <summary>
	/// mesh generation 
	/// </summary>
	/// <param name="option">Parametres of Sphere</param>
	/// <returns></returns>
	public Mesh GetFigure()
	{
		// calculate amount of triangulas and sizes of one step
		//GetStepSizes();

		Mesh mesh = new Mesh
		{
			/*vertices = GenerateVertices(),
			triangles = GenerateIndexes()*/
		};

		mesh.RecalculateNormals();
		return mesh;
	}
}
