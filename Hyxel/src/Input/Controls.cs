using System;

using Hyxel.Maths;
using Hyxel.SDL;

using static SDL2.SDL;
using static SDL2.SDL.SDL_Keycode;

namespace Hyxel.Input
{
  public class Controls
  {
    public SdlWindow Window { get; }
    
    public Point MouseMotion { get; private set; }
    
    
    public Controls(SdlWindow window)
    {
      Window = window;
      
      window.MouseButtonDown += OnMouseButtonDown;
      window.MouseButtonUp   += OnMouseButtonUp;
      window.MouseMotion += OnMouseMotion;
      
      window.KeyDown += OnKeyDown;
      window.KeyUp   += OnKeyUp;
    }
    
    
    void OnMouseButtonDown(MouseButton button, Point p)
    {
      if (button == MouseButton.Right)
        Window.MouseRelativeMode = true;
    }
    
    void OnMouseButtonUp(MouseButton button, Point p)
    {
      if (button == MouseButton.Right) {
        var storedPos = Window.MousePosition;
        Window.MouseRelativeMode = false;
        Window.MousePosition = storedPos;
      }
    }
    
    void OnMouseMotion(Point motion)
      => MouseMotion += motion;
    
    public void ResetMouseMotion()
      => MouseMotion = Point.Origin;
    
    
    void OnKeyDown(SDL_Keysym keysym)
    {
      // Quit when pressing the `Escape` key.
      if (keysym.sym == SDLK_ESCAPE)
        Window.Running = false;
    }
    
    void OnKeyUp(SDL_Keysym keysym)
    {
      
    }
  }
}
