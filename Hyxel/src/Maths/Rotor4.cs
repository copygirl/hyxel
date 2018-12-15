using System;

namespace Hyxel.Maths
{
  // ┌               ┐ ┌               ┐
  // │ a  -b  -c  -d │ │ p  -q  -r  -s │
  // │ b   a  -d   c │ │ q   p   s  -r │
  // │ c   d   a  -b │ │ r  -s   p   q │
  // │ d  -c   b   a │ │ s   r  -q   p │
  // └               ┘ └               ┘
  public struct Rotor4
  {
    // Scalar part
    float s;
    // BiVector part
    float wx, wy, wz;
    float     xy, xz;
    float         yz;
    // 4-vector part
    float v4;
    
    
    public Rotor4(float s, float wx, float wy, float wz, float xy, float xz, float yz, float v4)
    {
      this.s  = s;
      this.wx = wx; this.wy = wy; this.wz = wz;
                    this.xy = xy; this.xz = xz;
                                  this.yz = yz;
      this.v4 = v4;
    }
    
    // public Rotor4(in Vector4 from, in Vector4 to)
    // {
      
    // }
    
    // public Rotor4(float angleRadian, in BiVector4 plane)
    // {
      
    // }
  }
  
  public struct BiVector4
  {
    // ┌                 ┐
    // │   0  wx  wy  wz │ ???
    // │ -wx   0  xy  xz │
    // │ -wy -xy   0  yz │
    // │ -wz -xz -yz   0 │
    // └                 ┘
    
    public float wx, wy, wz;
    public float     xy, xz;
    public float         yz;
    
    public BiVector4(float wx, float wy, float wz, float xy, float xz, float yz)
    {
      this.wx = wx; this.wy = wy; this.wz = wz;
                    this.xy = xy; this.xz = xz;
                                  this.yz = yz;
    }
  }
  
  public static class BiVector4Extensions
  {
    public static BiVector4 Wedge(this in Vector4 a, in Vector4 b)
      => new BiVector4(a.W*b.X - b.W*a.X, a.W*b.Y - b.W*a.Y, a.W*b.Z - b.W*a.Z,
                                          a.X*b.Y - b.X*a.Y, a.X*b.Z - b.X*a.Z,
                                                             a.Y*b.Z - b.Y*a.Z);
  }
}
