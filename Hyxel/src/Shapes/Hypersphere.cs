using System;

namespace Hyxel.Shapes
{
  public struct Hypersphere
  {
    public Vector4 Center;
    public float Radius;
    
    
    public Hypersphere(Vector4 center, float radius)
      { Center = center; Radius = radius; }
    
    
    public float? Intersect(in Ray ray)
    {
      var l = ray.Origin - Center;
      var a = ray.Direction.Dot(ray.Direction);
      var b = 2 * ray.Direction.Dot(l);
      var c = l.Dot(l) - Radius * Radius;
      
      return (SolveQuadratic(a, b, c, out var t0, out var t1) && ((t0 >= 0) || (t1 >= 0)))
        ? (t0 >= 0) ? t0 : t1
        : (float?)null;
    }
    
    public Vector4 CalculateNormal(in Vector4 hit)
      => (hit - Center).Normalize();
    
    
    bool SolveQuadratic(in float a, in float b, in float c, out float x0, out float x1)
    {
      x0 = x1 = 0;
      var discr = b * b - 4 * a * c;
      if (discr < 0) return false;
      else if (discr == 0) x0 = x1 = b / a / -2;
      else {
        var q = (b > 0)
          ? (b + MathF.Sqrt(discr)) / -2
          : (b - MathF.Sqrt(discr)) / -2;
        x0 = q / a;
        x1 = c / q;
        if (x0 > x1) // Swap x0 and x1.
          (x0, x1) = (x1, x0);
      }
      return true;
    }
  }
}
