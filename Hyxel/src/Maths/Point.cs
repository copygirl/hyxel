using System;

namespace Hyxel.Maths
{
  public struct Point : IEquatable<Point>
  {
    public static readonly Point Origin = new Point(0, 0);
    
    
    public int X, Y;
    
    public Point(int x, int y)
      { X = x; Y = y; }
    
    
    public static Point operator -(Point p)
      => new Point(-p.X, -p.Y);
    
    public static Point operator *(Point p, int factor)
      => new Point(p.X * factor, p.Y * factor);
    public static Point operator /(Point p, int factor)
      => new Point(p.X / factor, p.Y / factor);
    
    public static Point operator +(Point left, Point right)
      => new Point(left.X + right.X, left.Y + right.Y);
    public static Point operator -(Point left, Point right)
      => new Point(left.X - right.X, left.Y - right.Y);
    
    public static bool operator ==(Point left, Point right) =>  left.Equals(right);
    public static bool operator !=(Point left, Point right) => !left.Equals(right);
    
    
    public bool Equals(Point other)
      => (X == other.X) && (Y == other.Y);
    public override bool Equals(object obj)
      => (obj is Point other) ? Equals(other) : false;
    
    public override int GetHashCode()
      => Tuple.Create(X, Y).GetHashCode(); // TODO: Optimize this?
    
    public override string ToString()
      => $"Point(x={X},y={Y})";
  }
}