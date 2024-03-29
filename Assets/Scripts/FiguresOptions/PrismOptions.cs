﻿using Assets.Scripts.Interfaces;

namespace Assets.Scripts.FiguresOptions
{
    class PrismOptions : IOption
    {
        public float height; 
        public int faces; 
        public float radius; 

        public override string ToString() => $"h: {height}, f: {faces}, r: {radius}";
    }
}
