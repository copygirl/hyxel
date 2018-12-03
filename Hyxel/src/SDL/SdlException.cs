using System;

namespace Hyxel.SDL
{
  public class SdlException : Exception
  {
    public SdlException(string message)
      : base(message) {  }
  }
}
