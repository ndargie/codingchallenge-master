using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;

namespace ConstructionLine.CodingChallenge
{
  public class SearchEngine
  {
    private readonly List<Shirt> _shirts;
    private readonly ILookup<string, Shirt> _lookup;

    public SearchEngine(List<Shirt> shirts)
    {
      _shirts = shirts;
      _lookup = _shirts.ToLookup(s => $"{s.Size.Name}-{s.Color.Name}");


      // TODO: data preparation and initialisation of additional data structures to improve performance goes here.
    }

    public SearchResults Search(SearchOptions options)
    {
      var sizeComparisons = options.Sizes.Any() ? options.Sizes : Size.All;
      var colorComparison = options.Colors.Any() ? options.Colors : Color.All;
      List<ColorCount> colorCounts = Color.All.Select(x => new ColorCount()
      {
        Color = x,
        Count = 0
      }).ToList();
      List<SizeCount> sizeCounts = Size.All.Select(x => new SizeCount()
      {
        Size = x,
        Count = 0
      }).ToList();
      List<Shirt> shirtResults = new List<Shirt>();
      sizeComparisons.ForEach(s =>
      {
        colorComparison.ForEach(c =>
        {
          var key = $"{s.Name}-{c.Name}";
          var shirts = _lookup[key];
          colorCounts.Single(x => x.Color == c).Count += shirts.Count();
          sizeCounts.Single(x => x.Size == s).Count += shirts.Count();
          shirtResults.AddRange(shirts);
        });
      });
      return new SearchResults
      {
        Shirts = shirtResults,
        ColorCounts = colorCounts,
        SizeCounts = sizeCounts
      };

    }
  }
}