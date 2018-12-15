using System;

namespace Hyxel.Maths
{
  public static class RandomExtensions
  {
    public static int Next(this Random rnd, int min, int maxExclusive)
      => min + rnd.Next(maxExclusive - min);
    public static int NextIncl(this Random rnd, int min, int maxInclusive)
      => min + rnd.Next(maxInclusive + 1 - min);
    
    public static float NextFloat(this Random rnd)
      => (float)rnd.NextDouble();
    public static float Next(this Random rnd, float min, float max)
      => min + rnd.NextFloat() * (max - min);
    public static double Next(this Random rnd, double min, double max)
      => min + rnd.NextDouble() * (max - min);
    
    public static T Choose<T>(this Random rnd, params T[] elements)
      => elements[rnd.Next(elements.Length)];
  }
}
