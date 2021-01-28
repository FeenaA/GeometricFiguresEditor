using Assets.Scripts.Interfaces;

internal class CapsuleOptions : IOption
{
    public float height;
    public int faces;
    public float radius;

    public override string ToString() => $"h: {height}, f: {faces}, r: {radius}";
}