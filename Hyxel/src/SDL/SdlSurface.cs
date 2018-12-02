using System;
using System.Runtime.InteropServices;

using static SDL2.SDL;

namespace Hyxel.SDL
{
  public struct SdlSurface
  {
    static readonly int FLAGS_OFFSET  = (int)Marshal.OffsetOf<SDL_Surface>("flags");
    static readonly int FORMAT_OFFSET = (int)Marshal.OffsetOf<SDL_Surface>("format");
    static readonly int WIDTH_OFFSET  = (int)Marshal.OffsetOf<SDL_Surface>("w");
    static readonly int HEIGHT_OFFSET = (int)Marshal.OffsetOf<SDL_Surface>("h");
    static readonly int PITCH_OFFSET  = (int)Marshal.OffsetOf<SDL_Surface>("pitch");
    static readonly int PIXELS_OFFSET = (int)Marshal.OffsetOf<SDL_Surface>("pixels");
    
    readonly IntPtr _ptr;
    
    public uint   Flags  => unchecked((uint)Marshal.ReadInt32(_ptr, FLAGS_OFFSET));
    public IntPtr Format => Marshal.ReadIntPtr(_ptr, FORMAT_OFFSET);
    public int    Width  => Marshal.ReadInt32(_ptr, WIDTH_OFFSET);
    public int    Height => Marshal.ReadInt32(_ptr, HEIGHT_OFFSET);
    public int    Pitch  => Marshal.ReadInt32(_ptr, PITCH_OFFSET);
    public IntPtr Pixels => Marshal.ReadIntPtr(_ptr, PIXELS_OFFSET);
    
    // public IntPtr userdata; // void*
    // public int locked;
    // public IntPtr lock_data; // void*
    // public SDL_Rect clip_rect;
    // public IntPtr map; // SDL_BlitMap*
    // public int refcount;
    
    public bool MustLock => SDL_MUSTLOCK(this);
    
    
    public SdlSurface(IntPtr ptr) => _ptr = ptr;
    
    public static implicit operator IntPtr(in SdlSurface surface) => surface._ptr;
    
    
    public void Lock() { if (MustLock) SDL_LockSurface(this); }
    
    public void Unlock() { if (MustLock) SDL_UnlockSurface(this); }
    
    
    public void PutPixel(int x, int y, Color color)
    {
      color.ToARGB(out var _, out var r, out var g, out var b);
      unsafe
      {
        var p = (uint*)Pixels + (y * Pitch / sizeof(uint)) + x;
        *p = SDL_MapRGB(Format, r, g, b);
      }
    }
  }
}
