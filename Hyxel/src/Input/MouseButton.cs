using System;

using static SDL2.SDL;

namespace Hyxel.Input
{
  public enum MouseButton
  {
    Left   = (int)SDL_BUTTON_LEFT,
    Middle = (int)SDL_BUTTON_MIDDLE,
    Right  = (int)SDL_BUTTON_RIGHT,
    X1     = (int)SDL_BUTTON_X1,
    X2     = (int)SDL_BUTTON_X2,
  }
}
