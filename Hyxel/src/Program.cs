using System;
using System.Threading.Tasks;

using Hyxel.Maths;
using Hyxel.Maths.Shapes;
using Hyxel.SDL;

using static System.MathF;
using static Hyxel.Maths.MathHelper;

namespace Hyxel
{
  class Program
  {
    static void Main(string[] args)
    {
      var window = new SdlWindow(800, 500);
      
      var cameraPos   = Vector4.Zero;
      var cameraRot   = Matrix4.Identity;
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
        if (window.MouseRelativeMode) {
          var yaw   =  Deg2Rad(window.MouseMotion.X);
          var pitch = -Deg2Rad(window.MouseMotion.Y);
          
          var yawRot = new Matrix4(
            1 ,      0   ,       0   , 0 ,
            0 , Cos(yaw) , -Sin(yaw) , 0 ,
            0 , Sin(yaw) ,  Cos(yaw) , 0 ,
            0 ,      0   ,       0   , 1 );
          
          var pitchRot = new Matrix4(
            1 , 0 ,       0    ,        0    ,
            0 , 1 ,       0    ,        0    ,
            0 , 0 , Cos(pitch) , -Sin(pitch) ,
            0 , 0 , Sin(pitch) ,  Cos(pitch) );
          
          cameraRot = cameraRot * pitchRot * yawRot;
        }
      };
      
      window.OnRender += () => {
        var forward   = cameraRot * Vector4.Forward;
        var stepRight = cameraRot * Vector4.Right;
        var stepDown  = cameraRot * Vector4.Down;
        Parallel.For(0, window.Width, x => {
          var ray = new Ray(cameraPos,
            forward * focalLength
              - stepRight * (window.Width  / 2 - x)
              - stepDown  * (window.Height / 2    ));
          
          for (var y = 0; y < window.Height; y++) {
            Vector4.Add(ref ray.Direction, stepDown);
            
            float? tMin    = null;
            int foundIndex = -1;
            for (var i = 0; i < circles.Length; i++) {
              if ((circles[i].Intersect(ray) is float tCur) && !(tCur > tMin)) {
                tMin       = tCur;
                foundIndex = i;
              }
            }
            
            if (tMin is float t) {
              var hit    = ray.Origin + ray.Direction * t;
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
