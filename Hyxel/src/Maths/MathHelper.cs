using System;

namespace Hyxel.Maths
{
  public static class MathHelper
  {
    public const float TAU = MathF.PI * 2;
    
    
    public static float Deg2Rad(float degrees) => degrees * TAU / 360.0f;
    
    public static float Rad2Deg(float radians) => radians / TAU * 360.0f;
  }
}
