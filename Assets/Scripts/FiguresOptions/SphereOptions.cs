using Assets.Scripts.Interfaces;

namespace Assets.Scripts.FiguresOptions
{
    public class SphereOptions : IOption
    {
        public float radius;
        public int sectors;

        public override string ToString() => $"r: {radius}, s: {sectors}";
    } 
}