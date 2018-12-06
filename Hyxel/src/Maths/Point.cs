using System;

namespace Hyxel.Maths
{
  public struct Point
  {
    public int X, Y;
    
    public Point(int x, int y)
      { X = x; Y = y; }
    
    public override string ToString()
      => $"Point(x={X},y={Y})";
  }
}