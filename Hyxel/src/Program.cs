using System;
using System.Runtime.InteropServices;
using static SDL2.SDL;

namespace Hyxel
{
  class Program
  {
    const int WINDOW_WIDTH  = 1200;
    const int WINDOW_HEIGHT = 750;
    
    const uint BACKGROUND_COLOR = 0xFF182848;
    
    static void Main(string[] args)
    {
      if (SDL_Init(SDL_INIT_VIDEO) < 0) {
        Console.WriteLine("Could not initialize SDL: {0}", SDL_GetError());
        return;
      }
      
      var window = SDL_CreateWindow("Hyxel",
        SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED,
        WINDOW_WIDTH, WINDOW_HEIGHT, SDL_WindowFlags.SDL_WINDOW_SHOWN);
      if (window == IntPtr.Zero) {
        Console.WriteLine("Couldn't create window: {0}", SDL_GetError());
        return;
      }
      
      var surfacePtr = SDL_GetWindowSurface(window);
      var surface    = Marshal.PtrToStructure<SDL_Surface>(surfacePtr);
      
      var r = (byte)((BACKGROUND_COLOR >> 16) & 0xFF);
      var g = (byte)((BACKGROUND_COLOR >>  8) & 0xFF);
      var b = (byte)((BACKGROUND_COLOR >>  0) & 0xFF);
      SDL_FillRect(surfacePtr, IntPtr.Zero, SDL_MapRGB(surface.format, r, g, b));
      
      SDL_UpdateWindowSurface(window);
      
      SDL_Delay(2000);
      
      SDL_DestroyWindow(window);
      
      SDL_Quit();
    }
  }
}
