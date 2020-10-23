using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ConstructionLine.CodingChallenge
{
  public class SizeColor : IComparer<SizeColor>
  {
    public Guid ColorId { get; }    
    public Guid SizeId { get; }  
    
    public SizeColor(Color color, Size size)
    {
      ColorId = color.Id;
      SizeId = size.Id;
    }

    public int Compare([AllowNull] SizeColor x, [AllowNull] SizeColor y)
    {
      if(x.ColorId == y.ColorId && x.SizeId == y.SizeId)
      {
        return 0;
      }
      return -1;
    }
  }
}
