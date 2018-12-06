using System;

namespace Hyxel.Maths
{
  public struct Ray
  {
    public Vector4 Origin, Direction;
    
    public Ray(Vector4 origin, Vector4 direction)
      { Origin = origin; Direction = direction; }
    
    public override string ToString()
      => $"Ray(origin={Origin},direction={Direction})";
  }
}
