using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Hyxel.Input;
using Hyxel.Maths;
using Hyxel.Maths.Shapes;
using Hyxel.SDL;

using static System.MathF;
using static Hyxel.Maths.MathHelper;

namespace Hyxel
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var surfaceSize  = new Size(640, 400);
      var pixelScaling = 3;
      var window    = new SdlWindow(surfaceSize * pixelScaling);
      var controls  = new Controls(window);
      
      var cameraPos   = Vector4.Zero;
      var cameraRot   = Matrix4.Identity;
      var focalLength = window.Height / 2.0f / pixelScaling;
      
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
      
      window.Update += (delta) => {
        var mouseSensitivity = 0.2f;
        var moveSpeed = 5.0f * (float)delta.TotalSeconds;
        
        if (controls.FourDimensionalMove) {
          var roll = Deg2Rad(controls.MouseMotion.X) * mouseSensitivity;
          var w    = Deg2Rad(controls.MouseMotion.Y) * mouseSensitivity;
          
          var rollRot = new Matrix4(
            1 ,       0   , 0 ,        0   ,
            0 , Cos(roll) , 0 , -Sin(roll) ,
            0 ,       0   , 1 ,        0   ,
            0 , Sin(roll) , 0 ,  Cos(roll) );
          
          var wRot = new Matrix4(
            Cos(w) , 0 , -Sin(w) , 0 ,
                0  , 1 ,      0  , 0 ,
            Sin(w) , 0 ,  Cos(w) , 0 ,
                0  , 0 ,      0  , 1 );
          
          cameraRot = cameraRot * rollRot * wRot;
          
          if (controls.Up  ) cameraPos += cameraRot *  Vector4.UnitW * moveSpeed;
          if (controls.Down) cameraPos += cameraRot * -Vector4.UnitW * moveSpeed;
        }
        
        if (controls.TraditionalMove) {
          var yaw   =  Deg2Rad(controls.MouseMotion.X) * mouseSensitivity;
          var pitch = -Deg2Rad(controls.MouseMotion.Y) * mouseSensitivity;
          
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
          
          if (controls.Forward) cameraPos += cameraRot * Vector4.Forward * moveSpeed;
          if (controls.Back   ) cameraPos += cameraRot * Vector4.Back    * moveSpeed;
          if (controls.Right  ) cameraPos += cameraRot * Vector4.Right   * moveSpeed;
          if (controls.Left   ) cameraPos += cameraRot * Vector4.Left    * moveSpeed;
          if (controls.Up     ) cameraPos += cameraRot * Vector4.Up      * moveSpeed;
          if (controls.Down   ) cameraPos += cameraRot * Vector4.Down    * moveSpeed;
        }
        
        controls.ResetMouseMotion();
        return Task.CompletedTask;
      };
      
      window.Render += async (delta) => {
        var forward   = cameraRot * Vector4.Forward;
        var stepRight = cameraRot * Vector4.Right;
        var stepDown  = cameraRot * Vector4.Down;
        
        var width  = window.Width  / pixelScaling;
        var height = window.Height / pixelScaling;
        
        IEnumerable<Rectangle> SplitScreen(int size)
        {
          for (var x = 0; x < width; x += size)
            for (var y = 0; y < height; y += size)
              yield return new Rectangle(x, y, Math.Min(width, x + size), Math.Min(height, y + size));
        }
        
        await Task.WhenAll(SplitScreen(32).Select(rect => Task.Run(() => {
          var rayTmp = default(Ray);
          var ray = new Ray(cameraPos,
            forward * focalLength
              - stepRight * (width  / 2 - rect.Left)
              - stepDown  * (height / 2 - rect.Top ));
          for (var x = rect.Left; x < rect.Right; x++) {
            Vector4.Add(ref ray.Direction, stepRight);
            rayTmp = ray;
            for (var y = rect.Top; y < rect.Bottom; y++) {
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
                var color  = new Color(Math.Abs(normal.X), Math.Abs(normal.Y), Math.Abs(normal.Z));
                
                for (var xx = 0; xx < pixelScaling; xx++)
                  for (var yy = 0; yy < pixelScaling; yy++)
                    window.Surface[x*pixelScaling + xx, y*pixelScaling + yy] = color;
              }
            }
            ray = rayTmp;
          }
        })));
      };
      
      await window.Run();
    }
  }
}
