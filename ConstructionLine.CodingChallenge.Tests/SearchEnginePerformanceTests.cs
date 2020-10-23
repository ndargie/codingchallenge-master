using System;
using System.Collections.Generic;
using System.Diagnostics;
using ConstructionLine.CodingChallenge.Tests.SampleData;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
  [TestFixture]
  public class SearchEnginePerformanceTests : SearchEngineTestsBase
  {
    private List<Shirt> _shirts;
    private SearchEngine _searchEngine;

    [SetUp]
    public void Setup()
    {

      var dataBuilder = new SampleDataBuilder(50000);

      _shirts = dataBuilder.CreateShirts();

      _searchEngine = new SearchEngine(_shirts);
    }


    [Test]
    //Have made assumption that we are just considering the search results. So the total for color should only include results from the search, 
    //if the color isn't in the search then the value should be 0
    public void PerformanceTest()
    {
      var sw = new Stopwatch();
      sw.Start();

      var options = new SearchOptions
      {
        Colors = new List<Color> { Color.Red }       
      };

      var results = _searchEngine.Search(options);

      sw.Stop();
      Console.WriteLine($"Test fixture finished in {sw.ElapsedMilliseconds} milliseconds");

      AssertResults(results.Shirts, options);
      AssertSizeCounts(_shirts, options, results.SizeCounts);
      AssertColorCounts(_shirts, options, results.ColorCounts);
    }

    [Test]
    public void PerformanceTestFixed()
    {
      var sw = new Stopwatch();
      sw.Start();
      var options = new SearchOptions
      {
        Colors = new List<Color> { Color.Red }
      };

      var results = _searchEngine.Search(options);
      sw.Stop();
      Console.WriteLine($"Test fixture finished in {sw.ElapsedMilliseconds} milliseconds");
      Assert.IsTrue(sw.ElapsedMilliseconds < 100, "Performance for search fell bellow required margin");
      AssertResults(results.Shirts, options);
      AssertSizeCounts(_shirts, options, results.SizeCounts);
      AssertColorShouldBeInSearchCounts(_shirts, options, results.ColorCounts);
    }
  }
}
