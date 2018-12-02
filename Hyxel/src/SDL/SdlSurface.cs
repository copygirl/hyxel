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
    
    public uint           Flags  => unchecked((uint)Marshal.ReadInt32(this, FLAGS_OFFSET));
    public SdlPixelFormat Format => new SdlPixelFormat(Marshal.ReadIntPtr(this, FORMAT_OFFSET));
    public int            Width  => Marshal.ReadInt32(this, WIDTH_OFFSET);
    public int            Height => Marshal.ReadInt32(this, HEIGHT_OFFSET);
    public int            Pitch  => Marshal.ReadInt32(this, PITCH_OFFSET);
    public IntPtr         Pixels => Marshal.ReadIntPtr(this, PIXELS_OFFSET);
    // public IntPtr userdata; // void*
    // public int locked;
    // public IntPtr lock_data; // void*
    // public SDL_Rect clip_rect;
    // public IntPtr map; // SDL_BlitMap*
    // public int refcount;
    
    public bool MustLock => SDL_MUSTLOCK(this);
    
    
    public Color this[int x, int y] {
      get { unsafe {
        if (!TryGetPointer(x, y, out var p)) return Color.Transparent;
        return Format.Map(*p & (~0u << Format.BitsPerPixel));
      } }
      set { unsafe {
        if (!TryGetPointer(x, y, out var p)) return;
        *p = *p & ~(~0u << Format.BitsPerPixel) | Format.Map(value);
      } }
    }
    
    
    public SdlSurface(IntPtr ptr) => _ptr = ptr;
    
    public static implicit operator IntPtr(in SdlSurface surface) => surface._ptr;
    
    
    public void Lock() { if (MustLock) SDL_LockSurface(this); }
    
    public void Unlock() { if (MustLock) SDL_UnlockSurface(this); }
    
    
    unsafe bool TryGetPointer(int x, int y, out uint* pointer)
    {
      if ((x >= 0) && (x < Width) && (y >= 0) && (y < Height)) {
        // FIXME: This might be problematic for pixel formats that aren't 32 bits?
        pointer = (uint*)Pixels + (y * Pitch / Format.BytesPerPixel) + x;
        return true;
      } else {
        pointer = null;
        return false;
      }
    }
  }
}
