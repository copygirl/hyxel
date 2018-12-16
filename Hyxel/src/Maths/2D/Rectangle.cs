using System;

namespace Hyxel.Maths
{
  public struct Rectangle : IEquatable<Rectangle>
  {
    public int Left, Right;
    public int Top, Bottom;
    
    
    public int Width  => Right  - Left;
    public int Height => Bottom - Top;
    
    public Point TopLeft     => new Point(Left, Top);
    public Point TopRight    => new Point(Right, Top);
    public Point BottomLeft  => new Point(Left, Bottom);
    public Point BottomRight => new Point(Right, Bottom);
    
    public Point Center => new Point((Left + Right) / 2, (Top + Bottom) / 2);
    
    public Size Size => new Size(Width, Height);
    
    
    public Rectangle(int left, int top, int right, int bottom)
      { Left = left; Top = top; Right = right; Bottom = bottom; }
    
    
    public static bool operator ==(Rectangle left, Rectangle right) =>  left.Equals(right);
    public static bool operator !=(Rectangle left, Rectangle right) => !left.Equals(right);
    
    
    public bool Equals(Rectangle other)
      => (Left  == other.Left ) && (Top    == other.Top   )
      && (Right == other.Right) && (Bottom == other.Bottom);
    public override bool Equals(object obj)
      => (obj is Rectangle other) ? Equals(other) : false;
    
    public override int GetHashCode()
      => Tuple.Create(Left, Top, Right, Bottom).GetHashCode(); // TODO: Optimize this?
    
    public override string ToString()
      => $"Rectangle(left={Left},top={Top},right={Right},bottom={Bottom})";
  }
}
