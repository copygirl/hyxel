using System;

namespace Hyxel
{
  public struct Color
  {
    public static readonly Color Transparent = Color.FromARGB(0x00000000);
    public static readonly Color White       = Color.FromARGB(0xFFFFFFFF);
    public static readonly Color Black       = Color.FromARGB(0xFF000000);
    
    
    public float Alpha, Red, Green, Blue;
    
    
    public Color(float alpha, float red, float green, float blue)
    {
      Alpha = alpha;
      Red   = red;
      Green = green;
      Blue  = blue;
    }
    public Color(float red, float green, float blue)
      : this(1.0f, red, green, blue) {  }
    
    
    public static Color FromARGB(byte a, byte r, byte g, byte b)
      => new Color(a / 255.0f, r / 255.0f, g / 255.0f, b / 255.0f);
    
    public static Color FromARGB(uint argb)
      => FromARGB((byte)((argb >> 24) & 0xFF),
                  (byte)((argb >> 16) & 0xFF),
                  (byte)((argb >>  8) & 0xFF),
                  (byte)((argb >>  0) & 0xFF));
    
    public uint ToARGB() => (uint)(Math.Clamp(Alpha, 0.0, 1.0) * 0xFF) << 24
                          | (uint)(Math.Clamp(Red  , 0.0, 1.0) * 0xFF) << 16
                          | (uint)(Math.Clamp(Green, 0.0, 1.0) * 0xFF) <<  8
                          | (uint)(Math.Clamp(Blue , 0.0, 1.0) * 0xFF);
    
    public void ToARGB(out byte a, out byte r, out byte g, out byte b)
    {
      a = (byte)(Math.Clamp(Alpha, 0.0, 1.0) * 0xFF);
      r = (byte)(Math.Clamp(Red  , 0.0, 1.0) * 0xFF);
      g = (byte)(Math.Clamp(Green, 0.0, 1.0) * 0xFF);
      b = (byte)(Math.Clamp(Blue , 0.0, 1.0) * 0xFF);
    }
    
    
    public override string ToString()
      => $"Color {{a={Alpha}, r={Red}, g={Green}, b={Blue}}}";
  }
}
