using System;
using System.Runtime.InteropServices;
using Hyxel.SDL;

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
    
    const uint BACKGROUND_COLOR = 0xFF182848;
    
    const float FOCAL_LENGTH = WINDOW_HEIGHT / 2.0f;
    
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
      
      var r = (byte)((BACKGROUND_COLOR >> 16) & 0xFF);
      var g = (byte)((BACKGROUND_COLOR >>  8) & 0xFF);
      var b = (byte)((BACKGROUND_COLOR >>  0) & 0xFF);
      
      var cameraPos = new Vector4(0, 0, 0, 0);
      
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
        
        SDL_FillRect(surface, IntPtr.Zero, SDL_MapRGB(surface.Format, r, g, b));
        
        var ray = new Ray {
          Origin    = cameraPos,
          Direction = new Vector4(0, 0, 0, FOCAL_LENGTH),
        };
        
        var circlePos    = new Vector4(0, 0, 0, 20);
        var circleRadius = 5.0f;
        
        surface.Lock();
        for (var x = 0; x < WINDOW_WIDTH; x++) {
          for (var y = 0; y < WINDOW_HEIGHT; y++) {
            ray.Direction.X = x - WINDOW_WIDTH  / 2;
            ray.Direction.Y = y - WINDOW_HEIGHT / 2;
            
            if (IntersectHypersphere(ray, circlePos, circleRadius, out var _))
              surface.PutPixel(x, y, Color.Black);
          }
        }
        surface.Unlock();
        
        SDL_UpdateWindowSurface(window);
        SDL_Delay(30);
      }
      
      SDL_DestroyWindow(window);
      
      SDL_Quit();
    }
    
    bool IntersectHypersphere(in Ray ray, in Vector4 center, in float radius,
                              out Vector4 hit/*, out Vector4 normal*/)
    {
      hit = Vector4.Zero;
      
      var l = ray.Origin - center;
      var a = ray.Direction.Dot(ray.Direction);
      var b = 2 * ray.Direction.Dot(l);
      var c = l.Dot(l) - radius * radius;
      
      if (!SolveQuadratic(a, b, c, out var t, out var _)) return false;
      hit = ray.Origin + ray.Direction.Normalize() * t;
      
      return true;
    }
    
    bool SolveQuadratic(in float a, in float b, in float c, out float x0, out float x1)
    {
      x0 = x1 = 0;
      var discr = b * b - 4 * a * c;
      if (discr < 0) return false;
      else if (discr == 0) x0 = x1 = b / a / -2;
      else {
        var q = (b > 0)
          ? (b + MathF.Sqrt(discr)) / -2
          : (b - MathF.Sqrt(discr)) / -2;
        x0 = q / a;
        x1 = c / q;
        if (x0 > x1) // Swap x0 and x1.
          (x0, x1) = (x1, x0);
      }
      return true;
    }
  }
}
