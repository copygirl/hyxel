using System;

using Hyxel.Maths;

using static SDL2.SDL;
using static SDL2.SDL.SDL_WindowFlags;
using static SDL2.SDL.SDL_EventType;
using static SDL2.SDL.SDL_Keycode;

namespace Hyxel.SDL
{
  public class SdlWindow
  {
    readonly IntPtr _ptr;
    
    
    public Size Size { get; }
    public int Width  => Size.Width;
    public int Height => Size.Height;
    
    public SdlSurface Surface { get; }
    
    
    public bool Running { get; set; } = true;
    
    public Color BackgroundColor { get; set; } = Color.FromARGB(0xFF182848);
    
    
    public event Action OnUpdate;
    
    public event Action OnRender;
    
    
    public SdlWindow(int width, int height)
    {
      if (SDL_Init(SDL_INIT_VIDEO) < 0) throw new SdlException(
        $"Could not initialize SDL: { SDL_GetError() }");
      
      _ptr = SDL_CreateWindow("Hyxel",
        SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED,
        width, height, SDL_WINDOW_SHOWN);
      
      if (_ptr == IntPtr.Zero) throw new SdlException(
        $"Couldn't create window: { SDL_GetError() }");
      
      Size    = new Size(width, height);
      Surface = new SdlSurface(SDL_GetWindowSurface(_ptr));
    }
    
    
    public void Run()
    {
      while (Running) {
        Update();
        Render();
        SDL_Delay(30);
      }
      SDL_DestroyWindow(_ptr);
      SDL_Quit();
    }
    
    
    void Update()
    {
      SDL_Event ev;
      while (SDL_PollEvent(out ev) != 0) {
        switch (ev.type) {
          case SDL_QUIT:
            // Quit when clicking the close window button.
            Running = false;
            break;
          case SDL_KEYDOWN:
            // Quit when pressing the `Escape` key.
            if (ev.key.keysym.sym == SDLK_ESCAPE)
              Running = false;
            break;
        }
      }
      
      OnUpdate?.Invoke();
    }
    
    void Render()
    {
      SDL_FillRect(Surface, IntPtr.Zero, Surface.MapColor(BackgroundColor));
      
      Surface.Lock();
      OnRender?.Invoke();
      Surface.Unlock();
      
      SDL_UpdateWindowSurface(_ptr);
    }
  }
}
