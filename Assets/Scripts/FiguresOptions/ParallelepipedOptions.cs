﻿using Assets.Scripts.Interfaces;

namespace Assets.Scripts.FiguresOptions
{
    public class ParallelepipedOptions : IOption
    {
        public float width;
        public float height;
        public float depth;

        public override string ToString() => $"w: {width}, h: {height}, d: {depth}";
    }
}
