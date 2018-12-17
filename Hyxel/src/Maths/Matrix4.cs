using System;

namespace Hyxel.Maths
{
  public struct Matrix4
  {
    public static readonly Matrix4 Identity = new Matrix4( 1 , 0 , 0 , 0 ,
                                                           0 , 1 , 0 , 0 ,
                                                           0 , 0 , 1 , 0 ,
                                                           0 , 0 , 0 , 1 );
    
    
    public float M00, M01, M02, M03;
    public float M10, M11, M12, M13;
    public float M20, M21, M22, M23;
    public float M30, M31, M32, M33;
    
    
    public Matrix4(float m00, float m01, float m02, float m03,
                   float m10, float m11, float m12, float m13,
                   float m20, float m21, float m22, float m23,
                   float m30, float m31, float m32, float m33)
    {
      M00 = m00; M01 = m01; M02 = m02; M03 = m03;
      M10 = m10; M11 = m11; M12 = m12; M13 = m13;
      M20 = m20; M21 = m21; M22 = m22; M23 = m23;
      M30 = m30; M31 = m31; M32 = m32; M33 = m33;
    }
    
    
    public static Matrix4 operator *(in Matrix4 left, in Matrix4 right)
      { var result = default(Matrix4); Mult(ref result, left, right); return result; }
    
    public static Vector4 operator *(in Matrix4 left, in Vector4 right)
      { var result = default(Vector4); Mult(ref result, left, right); return result; }
    
    
    public static void Mult(ref Matrix4 result, in Matrix4 left, in Matrix4 right)
    {
      // result.MXY = Vector4.Dot(left.RowY, right.ColX);
      
      result.M00 = Vector4.Dot( left.M00,left.M01,left.M02,left.M03 , right.M00,right.M10,right.M20,right.M30 );
      result.M01 = Vector4.Dot( left.M10,left.M11,left.M12,left.M13 , right.M00,right.M10,right.M20,right.M30 );
      result.M02 = Vector4.Dot( left.M20,left.M21,left.M22,left.M23 , right.M00,right.M10,right.M20,right.M30 );
      result.M03 = Vector4.Dot( left.M30,left.M31,left.M32,left.M33 , right.M00,right.M10,right.M20,right.M30 );
      
      result.M10 = Vector4.Dot( left.M00,left.M01,left.M02,left.M03 , right.M01,right.M11,right.M21,right.M31 );
      result.M11 = Vector4.Dot( left.M10,left.M11,left.M12,left.M13 , right.M01,right.M11,right.M21,right.M31 );
      result.M12 = Vector4.Dot( left.M20,left.M21,left.M22,left.M23 , right.M01,right.M11,right.M21,right.M31 );
      result.M13 = Vector4.Dot( left.M30,left.M31,left.M32,left.M33 , right.M01,right.M11,right.M21,right.M31 );
      
      result.M20 = Vector4.Dot( left.M00,left.M01,left.M02,left.M03 , right.M02,right.M12,right.M22,right.M32 );
      result.M21 = Vector4.Dot( left.M10,left.M11,left.M12,left.M13 , right.M02,right.M12,right.M22,right.M32 );
      result.M22 = Vector4.Dot( left.M20,left.M21,left.M22,left.M23 , right.M02,right.M12,right.M22,right.M32 );
      result.M23 = Vector4.Dot( left.M30,left.M31,left.M32,left.M33 , right.M02,right.M12,right.M22,right.M32 );
      
      result.M30 = Vector4.Dot( left.M00,left.M01,left.M02,left.M03 , right.M03,right.M13,right.M23,right.M33 );
      result.M31 = Vector4.Dot( left.M10,left.M11,left.M12,left.M13 , right.M03,right.M13,right.M23,right.M33 );
      result.M32 = Vector4.Dot( left.M20,left.M21,left.M22,left.M23 , right.M03,right.M13,right.M23,right.M33 );
      result.M33 = Vector4.Dot( left.M30,left.M31,left.M32,left.M33 , right.M03,right.M13,right.M23,right.M33 );
    }
    
    public static void Mult(ref Vector4 result, in Matrix4 left, in Vector4 right)
    {
      result.W = Vector4.Dot( left.M00,left.M01,left.M02,left.M03 , right );
      result.X = Vector4.Dot( left.M10,left.M11,left.M12,left.M13 , right );
      result.Y = Vector4.Dot( left.M20,left.M21,left.M22,left.M23 , right );
      result.Z = Vector4.Dot( left.M30,left.M31,left.M32,left.M33 , right );
    }
    
    
    public override string ToString()
      => $"Matrix4(({M00},{M01},{M02},{M03});({M10},{M11},{M12},{M13});({M20},{M21},{M22},{M23});({M30},{M31},{M32},{M33}))";
  }
}
