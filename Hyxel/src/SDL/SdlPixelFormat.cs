using System;
using System.Runtime.InteropServices;

using static SDL2.SDL;

namespace Hyxel.SDL
{
  public struct SdlPixelFormat
  {
    static readonly int BITS_PER_PIXEL_OFFSET  = (int)Marshal.OffsetOf<SDL_PixelFormat>("BitsPerPixel");
    static readonly int BYTES_PER_PIXEL_OFFSET = (int)Marshal.OffsetOf<SDL_PixelFormat>("BytesPerPixel");
    static readonly int R_MASK_OFFSET          = (int)Marshal.OffsetOf<SDL_PixelFormat>("Rmask");
    static readonly int G_MASK_OFFSET          = (int)Marshal.OffsetOf<SDL_PixelFormat>("Gmask");
    static readonly int B_MASK_OFFSET          = (int)Marshal.OffsetOf<SDL_PixelFormat>("Bmask");
    static readonly int A_MASK_OFFSET          = (int)Marshal.OffsetOf<SDL_PixelFormat>("Amask");
    static readonly int R_LOSS_OFFSET          = (int)Marshal.OffsetOf<SDL_PixelFormat>("Rloss");
    static readonly int G_LOSS_OFFSET          = (int)Marshal.OffsetOf<SDL_PixelFormat>("Gloss");
    static readonly int B_LOSS_OFFSET          = (int)Marshal.OffsetOf<SDL_PixelFormat>("Bloss");
    static readonly int A_LOSS_OFFSET          = (int)Marshal.OffsetOf<SDL_PixelFormat>("Aloss");
    static readonly int R_SHIFT_OFFSET         = (int)Marshal.OffsetOf<SDL_PixelFormat>("Rshift");
    static readonly int G_SHIFT_OFFSET         = (int)Marshal.OffsetOf<SDL_PixelFormat>("Gshift");
    static readonly int B_SHIFT_OFFSET         = (int)Marshal.OffsetOf<SDL_PixelFormat>("Bshift");
    static readonly int A_SHIFT_OFFSET         = (int)Marshal.OffsetOf<SDL_PixelFormat>("Ashift");
    
    readonly IntPtr _ptr;
    
    // public uint format;
    // public IntPtr palette; // SDL_Palette*
    public byte BitsPerPixel  => Marshal.ReadByte(this, BITS_PER_PIXEL_OFFSET);
    public byte BytesPerPixel => Marshal.ReadByte(this, BYTES_PER_PIXEL_OFFSET);
    public uint Rmask         => unchecked((uint)Marshal.ReadInt32(this, R_MASK_OFFSET));
    public uint Gmask         => unchecked((uint)Marshal.ReadInt32(this, G_MASK_OFFSET));
    public uint Bmask         => unchecked((uint)Marshal.ReadInt32(this, B_MASK_OFFSET));
    public uint Amask         => unchecked((uint)Marshal.ReadInt32(this, A_MASK_OFFSET));
    public byte Rloss         => Marshal.ReadByte(this, R_LOSS_OFFSET);
    public byte Gloss         => Marshal.ReadByte(this, G_LOSS_OFFSET);
    public byte Bloss         => Marshal.ReadByte(this, B_LOSS_OFFSET);
    public byte Aloss         => Marshal.ReadByte(this, A_LOSS_OFFSET);
    public byte Rshift        => Marshal.ReadByte(this, R_SHIFT_OFFSET);
    public byte Gshift        => Marshal.ReadByte(this, G_SHIFT_OFFSET);
    public byte Bshift        => Marshal.ReadByte(this, B_SHIFT_OFFSET);
    public byte Ashift        => Marshal.ReadByte(this, A_SHIFT_OFFSET);
    // public int refcount;
    // public IntPtr next; // SDL_PixelFormat*
    
    
    public SdlPixelFormat(IntPtr ptr) => _ptr = ptr;
    
    public static implicit operator IntPtr(in SdlPixelFormat format) => format._ptr;
    
    
    public uint Map(Color color)
    {
      color.ToARGB(out var a, out var r, out var g, out var b);
      return SDL_MapRGBA(this, r, g, b, a);
    }
    
    public Color Map(uint pixel)
      => Color.FromARGB(
        (Amask != 0) ? (byte)((pixel & Amask) >> Ashift << Aloss) : byte.MaxValue,
        (byte)((pixel & Rmask) >> Rshift << Rloss),
        (byte)((pixel & Gmask) >> Gshift << Gloss),
        (byte)((pixel & Bmask) >> Bshift << Bloss));
  }
}
