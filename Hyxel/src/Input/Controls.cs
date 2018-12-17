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
    
    public bool TraditionalMove { get; private set; }
    public bool FourDimensionalMove { get; private set; }
    
    public bool Forward { get; private set; }
    public bool Back    { get; private set; }
    public bool Right   { get; private set; }
    public bool Left    { get; private set; }
    public bool Up   { get; private set; }
    public bool Down { get; private set; }
    
    
    public Controls(SdlWindow window)
    {
      Window = window;
      
      window.MouseButtonDown += OnMouseButtonDown;
      window.MouseButtonUp   += OnMouseButtonUp;
      window.MouseMotion += OnMouseMotion;
      
      window.KeyDown += OnKeyDown;
      window.KeyUp   += OnKeyUp;
      
      window.FocusLost += OnFocusLost;
    }
    

    void OnMouseButtonDown(MouseButton button, Point p)
    {
      switch (button) {
        case MouseButton.Right:
          TraditionalMove = true;
          break;
        case MouseButton.Left:
          FourDimensionalMove = true;
          break;
      }
      if (TraditionalMove || FourDimensionalMove)
        Window.MouseRelativeMode = true;
    }
    
    void OnMouseButtonUp(MouseButton button, Point p)
    {
      switch (button) {
        case MouseButton.Right:
          TraditionalMove = false;
          break;
        case MouseButton.Left:
          FourDimensionalMove = false;
          break;
      }
      if (!TraditionalMove && !FourDimensionalMove) {
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
      switch (keysym.sym) {
        // Quit when pressing the `Escape` key.
        case SDLK_ESCAPE: Window.Running = false; break;
        
        case SDLK_w: Forward = true; break;
        case SDLK_s: Back    = true; break;
        case SDLK_d: Right   = true; break;
        case SDLK_a: Left    = true; break;
        case SDLK_SPACE:  Up   = true; break;
        case SDLK_LSHIFT: Down = true; break;
      }
    }
    
    void OnKeyUp(SDL_Keysym keysym)
    {
      switch (keysym.sym) {
        case SDLK_w: Forward = false; break;
        case SDLK_s: Back    = false; break;
        case SDLK_d: Right   = false; break;
        case SDLK_a: Left    = false; break;
        case SDLK_SPACE:  Up   = false; break;
        case SDLK_LSHIFT: Down = false; break;
      }
    }
    
    void OnFocusLost()
      => Forward = Back = Right = Left = Up = Down = false;
  }
}
