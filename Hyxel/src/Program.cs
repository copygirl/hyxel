using System;
using System.Runtime.InteropServices;

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
      
      var surfacePtr = SDL_GetWindowSurface(window);
      var surface    = Marshal.PtrToStructure<SDL_Surface>(surfacePtr);
      
      var r = (byte)((BACKGROUND_COLOR >> 16) & 0xFF);
      var g = (byte)((BACKGROUND_COLOR >>  8) & 0xFF);
      var b = (byte)((BACKGROUND_COLOR >>  0) & 0xFF);
      SDL_FillRect(surfacePtr, IntPtr.Zero, SDL_MapRGB(surface.format, r, g, b));
      
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
        
        SDL_UpdateWindowSurface(window);
        SDL_Delay(30);
      }
      
      SDL_DestroyWindow(window);
      
      SDL_Quit();
    }
  }
}
