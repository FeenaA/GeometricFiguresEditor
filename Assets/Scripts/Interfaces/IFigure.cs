using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFigure
{
    Mesh GetFigure();

    //public Vector3[] GenerateVertices();
    //public int[] GenerateIndexes();
}
