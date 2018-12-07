using System;

namespace Hyxel.Maths
{
  public struct Size : IEquatable<Size>
  {
    public static readonly Size Zero = new Size(0, 0);
    
    
    public int Width, Height;
    
    public Size(int width, int height)
      { Width = width; Height = height; }
    
    
    public static Size operator *(Size s, int factor)
      => new Size(s.Width * factor, s.Height * factor);
    public static Size operator /(Size s, int factor)
      => new Size(s.Width / factor, s.Height / factor);
    
    public static bool operator ==(Size left, Size right) =>  left.Equals(right);
    public static bool operator !=(Size left, Size right) => !left.Equals(right);
    
    public static explicit operator Size(Point p) => new Size(p.X, p.Y);
    public static explicit operator Point(Size s) => new Point(s.Width, s.Height);
    
    
    public bool Equals(Size other)
      => (Width == other.Width) && (Height == other.Height);
    public override bool Equals(object obj)
      => (obj is Size other) ? Equals(other) : false;
    
    public override int GetHashCode()
      => Tuple.Create(Width, Height).GetHashCode(); // TODO: Optimize this?
    
    public override string ToString()
      => $"Size(width={Width},height={Height})";
  }
}