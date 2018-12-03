using System;
using System.Runtime.InteropServices;

using Hyxel.SDL;
using Hyxel.Shapes;

using static SDL2.SDL;
using static SDL2.SDL.SDL_WindowFlags;
using static SDL2.SDL.SDL_EventType;
using static SDL2.SDL.SDL_Keycode;

namespace Hyxel
{
  class Program
  {
    const int WINDOW_WIDTH  = 1200;
    const int WINDOW_HEIGHT = 750;
    
    const float FOCAL_LENGTH = WINDOW_HEIGHT / 2.0f;
    
    static readonly Color BackgroundColor = Color.FromARGB(0xFF182848);
    
    bool _running = true;
    
    static void Main(string[] args)
      => new Program().Run();
    
    void Run()
    {
      if (SDL_Init(SDL_INIT_VIDEO) < 0) {
        Console.Error.WriteLine("Could not initialize SDL: {0}", SDL_GetError());
        return;
      }
      
      var window = SDL_CreateWindow("Hyxel",
        SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED,
        WINDOW_WIDTH, WINDOW_HEIGHT, SDL_WINDOW_SHOWN);
      if (window == IntPtr.Zero) {
        Console.Error.WriteLine("Couldn't create window: {0}", SDL_GetError());
        return;
      }
      
      var surface = new SdlSurface(SDL_GetWindowSurface(window));
      
      var cameraPos = new Vector4(0, 0, 0, 0);
      
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
      
      SDL_Event ev;
      while (_running) {
        
        while (SDL_PollEvent(out ev) != 0) {
          switch (ev.type) {
            case SDL_QUIT:
              // Quit when clicking the close window button.
              _running = false;
              break;
            case SDL_KEYDOWN:
              // Quit when pressing the `Escape` key.
              if (ev.key.keysym.sym == SDLK_ESCAPE)
                _running = false;
              break;
          }
        }
        
        SDL_FillRect(surface, IntPtr.Zero, surface.MapColor(BackgroundColor));
        
        var ray = new Ray {
          Origin    = cameraPos,
          Direction = Vector4.Forward * FOCAL_LENGTH,
        };
        
        surface.Lock();
        for (var x = 0; x < WINDOW_WIDTH; x++) {
          for (var y = 0; y < WINDOW_HEIGHT; y++) {
            ray.Direction.X =   x - WINDOW_WIDTH  / 2;
            ray.Direction.Z = -(y - WINDOW_HEIGHT / 2);
            
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
              surface[x, y] = new Color(Math.Abs(normal.X), Math.Abs(normal.Y), Math.Abs(normal.Z));
            }
          }
        }
        surface.Unlock();
        
        SDL_UpdateWindowSurface(window);
        SDL_Delay(30);
      }
      
      SDL_DestroyWindow(window);
      
      SDL_Quit();
    }
  }
}
