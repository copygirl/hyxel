using System;
using System.Threading.Tasks;

using Hyxel.Maths;
using Hyxel.Maths.Shapes;
using Hyxel.SDL;

namespace Hyxel
{
  class Program
  {
    static void Main(string[] args)
    {
      var window = new SdlWindow(800, 500);
      
      var cameraPos   = new Vector4(0, 0, 0, 0);
      var focalLength = window.Height / 2.0f;
      
      var circles = new Hypersphere[8];
      var rnd     = new Random(1);
      for (int i = 0; i < circles.Length; i++) {
        var w = ((float)rnd.NextDouble() - 0.5f) * 10;
        var x = ((float)rnd.NextDouble() - 0.5f) * 24;
        var y = ((float)rnd.NextDouble() - 0.5f) * 24;
        var z = ((float)rnd.NextDouble() - 0.5f) * 24;
        var r = 4 + (float)rnd.NextDouble() * 2;
        circles[i] = new Hypersphere(Vector4.Forward * 16 + new Vector4(w, x, y, z), r);
      }
      
      window.OnUpdate += () => {
        if (window.MouseRelativeMode)
          cameraPos += Vector4.Right * window.MouseMotion.X / 50.0f
                    +  Vector4.Down  * window.MouseMotion.Y / 50.0f;
      };
      
      window.OnRender += () => {
        Parallel.For(0, window.Width, x => {
          var ray = new Ray(cameraPos, Vector4.Forward * focalLength);
          ray.Direction.X = x - window.Width / 2;
          ray.Direction.Z = window.Height / 2;
          
          for (var y = 0; y < window.Height; y++) {
            ray.Direction.Z--;
            
            float? tMin    = null;
            int foundIndex = -1;
            for (var i = 0; i < circles.Length; i++) {
              if ((circles[i].Intersect(ray) is float tCur) && !(tCur > tMin)) {
                tMin       = tCur;
                foundIndex = i;
              }
            }
            
            if (tMin is float t) {
              var hit    = ray.Direction * t;
              var normal = circles[foundIndex].CalculateNormal(hit);
              window.Surface[x, y] = new Color(Math.Abs(normal.X), Math.Abs(normal.Y), Math.Abs(normal.Z));
            }
          }
        });
      };
      
      window.Run();
    }
  }
}
