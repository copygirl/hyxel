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
    
    public uint   Flags     { get; }
    public IntPtr FormatPtr { get; }
    public int    Width     { get; }
    public int    Height    { get; }
    public int    Pitch     { get; }
    public IntPtr Pixels    { get; }
    // public IntPtr userdata; // void*
    // public int locked;
    // public IntPtr lock_data; // void*
    // public SDL_Rect clip_rect;
    // public IntPtr map; // SDL_BlitMap*
    // public int refcount;
    
    public bool MustLock => SDL_MUSTLOCK(this);
    
    public SDL_PixelFormat Format { get; }
    
    
    public Color this[int x, int y] {
      get { unsafe {
        if (!TryGetPointer(x, y, out var p)) return Color.Transparent;
        return MapColor(*p & (~0u << Format.BitsPerPixel));
      } }
      set { unsafe {
        if (!TryGetPointer(x, y, out var p)) return;
        *p = *p & ~(~0u << Format.BitsPerPixel) | MapColor(value);
      } }
    }
    
    
    public SdlSurface(IntPtr ptr)
    {
      _ptr = ptr;
      
      Flags     = unchecked((uint)Marshal.ReadInt32(ptr, FLAGS_OFFSET));
      FormatPtr = Marshal.ReadIntPtr(ptr, FORMAT_OFFSET);
      Width     = Marshal.ReadInt32(ptr, WIDTH_OFFSET);
      Height    = Marshal.ReadInt32(ptr, HEIGHT_OFFSET);
      Pitch     = Marshal.ReadInt32(ptr, PITCH_OFFSET);
      Pixels    = Marshal.ReadIntPtr(ptr, PIXELS_OFFSET);
      
      Format = Marshal.PtrToStructure<SDL_PixelFormat>(FormatPtr);
    }
    
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
    
    public uint MapColor(Color color)
    {
      color.ToARGB(out var a, out var r, out var g, out var b);
      return SDL_MapRGBA(FormatPtr, r, g, b, a);
    }
    
    public Color MapColor(uint pixel)
      => Color.FromARGB(
        (Format.Amask != 0) ? (byte)((pixel & Format.Amask) >> Format.Ashift << Format.Aloss) : byte.MaxValue,
        (byte)((pixel & Format.Rmask) >> Format.Rshift << Format.Rloss),
        (byte)((pixel & Format.Gmask) >> Format.Gshift << Format.Gloss),
        (byte)((pixel & Format.Bmask) >> Format.Bshift << Format.Bloss));
  }
}
