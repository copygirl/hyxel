#define PARALLEL

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
        var w = rnd.Next(- 5.0f,  5.0f);
        var x = rnd.Next(-12.0f, 12.0f);
        var y = rnd.Next(-12.0f, 12.0f);
        var z = rnd.Next(-12.0f, 12.0f);
        var position = Vector4.Forward * 16 + new Vector4(w, x, y, z);
        var radius   = rnd.Next(4.0f, 6.0f);
        circles[i] = new Hypersphere(position, radius);
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
#if PARALLEL
        Parallel.For(0, window.Width, x => {
#else
        for (var x = 0; x < window.Width; x++) {
#endif
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
        }
#if PARALLEL
        );
#endif
      };
      
      window.Run();
    }
  }
}
