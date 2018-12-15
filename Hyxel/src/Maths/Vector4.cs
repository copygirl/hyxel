using System;
using System.Runtime.CompilerServices;

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
      { var result = default(Vector4); Normalize(ref result, this); return result; }
    
    
    public static Vector4 operator -(in Vector4 value)
      => new Vector4(-value.W, -value.X, -value.Y, -value.Z);
    
    public static Vector4 operator *(in Vector4 value, float factor)
      => new Vector4(value.W * factor, value.X * factor, value.Y * factor, value.Z * factor);
    public static Vector4 operator /(in Vector4 a, float factor)
      => new Vector4(a.W / factor, a.X / factor, a.Y / factor, a.Z / factor);
    
    public static Vector4 operator +(in Vector4 left, in Vector4 right)
      => new Vector4(left.W + right.W, left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    public static Vector4 operator -(in Vector4 left, in Vector4 right)
      => new Vector4(left.W - right.W, left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    
    
    public static void Add(ref Vector4 result, in Vector4 left, in Vector4 right)
    {
      result.W = left.W + right.W;
      result.X = left.X + right.X;
      result.Y = left.Y + right.Y;
      result.Z = left.Z + right.Z;
    }
    public static void Add(ref Vector4 left, in Vector4 right)
      => Add(ref left, left, right);
    
    
    public static void Sub(ref Vector4 result, in Vector4 left, in Vector4 right)
    {
      result.W = left.W - right.W;
      result.X = left.X - right.X;
      result.Y = left.Y - right.Y;
      result.Z = left.Z - right.Z;
    }
    public static void Sub(ref Vector4 left, in Vector4 right)
      => Sub(ref left, left, right);
    
    
    public static void Normalize(ref Vector4 result, in Vector4 value)
    {
      var length = value.Length;
      if (length > 0) {
        result.W = value.W / length;
        result.X = value.X / length;
        result.Y = value.Y / length;
        result.Z = value.Z / length;
      } else
        result.W = result.X = result.Y = result.Z = 0;
    }
    public static void Normalize(ref Vector4 value)
      => Normalize(ref value, value);
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(in Vector4 left, in Vector4 right)
      => Dot(left.W, left.X, left.Y, left.Z, right.W, right.X, right.Y, right.Z);
      
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(float l0, float l1, float l2, float l3, in Vector4 right)
      => Dot(l0, l1, l2, l3, right.W, right.X, right.Y, right.Z);
      
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(in Vector4 left, float r0, float r1, float r2, float r3)
      => Dot(left.W, left.X, left.Y, left.Z, r0, r1, r2, r3);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(float l0, float l1, float l2, float l3,
                            float r0, float r1, float r2, float r3)
      => (l0 * r0) + (l1 * r1) + (l2 * r2) + (l3 * r3);
    
    
    public override string ToString()
      => $"Vector4(w={W},x={X},y={Y},z={Z})";
  }
}
