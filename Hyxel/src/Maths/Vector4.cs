using System;

namespace Hyxel.Maths
{
  public struct Vector4
  {
    public static readonly Vector4 Zero  = new Vector4(0, 0, 0, 0);
    public static readonly Vector4 One   = new Vector4(1, 1, 1, 1);
    
    public static readonly Vector4 UnitW = new Vector4(1, 0, 0, 0);
    public static readonly Vector4 UnitX = new Vector4(0, 1, 0, 0);
    public static readonly Vector4 UnitY = new Vector4(0, 0, 1, 0);
    public static readonly Vector4 UnitZ = new Vector4(0, 0, 0, 1);
    
    public static readonly Vector4 Right   =  UnitX;
    public static readonly Vector4 Left    = -UnitX;
    public static readonly Vector4 Forward =  UnitY;
    public static readonly Vector4 Back    = -UnitY;
    public static readonly Vector4 Up      =  UnitZ;
    public static readonly Vector4 Down    = -UnitZ;
    
    
    public float W, X, Y, Z;
    
    public float LengthSqr => (W * W) + (X * X) + (Y * Y) + (Z * Z);
    public float Length => MathF.Sqrt(LengthSqr);
    
    
    public Vector4(float w, float x, float y, float z)
      { W = w; X = x; Y = y; Z = z; }
    
    
    public Vector4 Normalize()
    {
      var length = Length;
      return (length > 0)
        ? new Vector4(W / length, X / length, Y / length, Z / length)
        : Zero;
    }
    
    public float Dot(in Vector4 other)
      => (W * other.W) + (X * other.X) + (Y * other.Y) + (Z * other.Z);
    
    
    public static Vector4 operator -(in Vector4 v)
      => new Vector4(-v.W, -v.X, -v.Y, -v.Z);
    
    public static Vector4 operator *(in Vector4 a, in float factor)
      => new Vector4(a.W * factor, a.X * factor, a.Y * factor, a.Z * factor);
    public static Vector4 operator /(in Vector4 a, in float factor)
      => new Vector4(a.W / factor, a.X / factor, a.Y / factor, a.Z / factor);
    
    public static Vector4 operator +(in Vector4 a, in Vector4 b)
      => new Vector4(a.W + b.W, a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vector4 operator -(in Vector4 a, in Vector4 b)
      => new Vector4(a.W - b.W, a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    
    
    public override string ToString()
      => $"Vector4(w={W},x={X},y={Y},z={Z})";
  }
}
