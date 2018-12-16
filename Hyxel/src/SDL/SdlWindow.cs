using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Hyxel.Maths;

using static SDL2.SDL;
using static SDL2.SDL.SDL_WindowFlags;
using static SDL2.SDL.SDL_EventType;
using static SDL2.SDL.SDL_Keycode;
using static SDL2.SDL.SDL_bool;
using static SDL2.SDL.SDL_WindowEventID;

namespace Hyxel.SDL
{
  public class SdlWindow
  {
    readonly IntPtr _ptr;
    
    Point _mousePosition;
    
    
    public Size Size { get; }
    public int Width    => Size.Width;
    public int Height   => Size.Height;
    public Point Center => (Point)(Size / 2);
    
    public SdlSurface Surface { get; }
    
    
    public bool Running { get; set; } = true;
    
    public Color BackgroundColor { get; set; } = Color.FromARGB(0xFF182848);
    
    
    public Point MousePosition {
      get => _mousePosition;
      set => WarpMouse(value);
    }
    
    public bool MouseRelativeMode {
      get => (SDL_GetRelativeMouseMode() == SDL_TRUE);
      set => SDL_SetRelativeMouseMode(value ? SDL_TRUE : SDL_FALSE);
    }
    
    public Point MouseMotion { get; private set; }
    
    
    public event Func<TimeSpan, Task> OnUpdate;
    
    public event Func<TimeSpan, Task> OnRender;
    
    
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
    
    
    public async Task Run()
    {
      await Task.WhenAll(
        Loop(TimeSpan.FromSeconds(1.0 / 90), Update) ,
        Loop(TimeSpan.FromSeconds(1.0 / 30), Render) );
      SDL_DestroyWindow(_ptr);
      SDL_Quit();
    }
    
    async Task Loop(TimeSpan frequency, Func<TimeSpan, Task> action)
    {
      var delta = TimeSpan.Zero;
      var watch = new Stopwatch();
      watch.Start();
      while (Running) {
        await action(delta);
        
        var delay = (frequency - watch.Elapsed);
        if (delay > TimeSpan.Zero)
          await Task.Delay(delay);
        
        delta = watch.Elapsed;
        watch.Reset();
      }
    }
    
    
    async Task Update(TimeSpan delta)
    {
      MouseMotion = Point.Origin;
      
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
          
          case SDL_MOUSEBUTTONDOWN:
            if (ev.button.button == SDL_BUTTON_RIGHT)
              MouseRelativeMode = true;
            break;
          case SDL_MOUSEBUTTONUP:
            if (ev.button.button == SDL_BUTTON_RIGHT) {
              MouseRelativeMode = false;
              WarpMouse(_mousePosition);
            }
            break;
          case SDL_MOUSEMOTION:
            var pos = new Point(ev.motion.x, ev.motion.y);
            if (!MouseRelativeMode) _mousePosition = pos;
            MouseMotion += new Point(ev.motion.xrel, ev.motion.yrel);
            break;
          
          case SDL_WINDOWEVENT:
            switch (ev.window.windowEvent) {
              case SDL_WINDOWEVENT_FOCUS_LOST:
                MouseRelativeMode = false;
                break;
            }
            break;
        }
      }
      
      await InvokeAll(OnUpdate, delta);
    }
    
    async Task Render(TimeSpan delta)
    {
      SDL_FillRect(Surface, IntPtr.Zero, Surface.MapColor(BackgroundColor));
      
      Surface.Lock();
      await InvokeAll(OnRender, delta);
      Surface.Unlock();
      
      SDL_UpdateWindowSurface(_ptr);
    }
    
    
    void WarpMouse(Point position)
      => SDL_WarpMouseInWindow(_ptr, position.X, position.Y);
    
    Task InvokeAll<T>(Func<T, Task> ev, T arg)
      => (ev != null)
        ? Task.WhenAll(ev.GetInvocationList()
          .Cast<Func<T, Task>>()
          .Select(d => d.Invoke(arg)))
        : Task.CompletedTask;
  }
}
