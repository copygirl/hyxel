using System;

namespace Hyxel.Maths
{
  public struct Size
  {
    public int Width, Height;
    
    public Size(int width, int height)
      { Width = width; Height = height; }
    
    public override string ToString()
      => $"Size(width={Width},height={Height})";
  }
}