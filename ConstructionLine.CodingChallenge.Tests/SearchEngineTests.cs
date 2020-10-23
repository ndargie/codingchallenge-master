using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyModel;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
  [TestFixture]
  public class SearchEngineTests : SearchEngineTestsBase
  {

    [Test]
    public void SimpleSearchMatch()
    {
      var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

      var searchEngine = new SearchEngine(shirts);

      var searchOptions = new SearchOptions
      {
        Colors = new List<Color> { Color.Red },
        Sizes = new List<Size> { Size.Small }
      };

      var results = searchEngine.Search(searchOptions);

      Assert.AreEqual(1, results.Shirts.Count);
      Assert.AreEqual(results.Shirts.Single(x => x.Color.Name == Color.Red.Name), results.Shirts.First());
    }

    [Test]
    public void TestNoSearchProvided()
    {
      var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };
      var searchEngine = new SearchEngine(shirts);
      SearchOptions searchOptions = new SearchOptions();
      var results = searchEngine.Search(searchOptions);
      Assert.AreEqual(3, results.Shirts.Count);
      Assert.AreEqual(1, results.ColorCounts.Where(x => x.Color.Name == "Red").Count());
    }

    [Test]
    public void TestMultipleOfSizeSoFilteredByColor()
    {
      var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "White - Small", Size.Small, Color.White),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

      var searchOptions = new SearchOptions
      {
        Colors = new List<Color> { Color.Red }
      };
      var searchEngine = new SearchEngine(shirts);
      var results = searchEngine.Search(searchOptions);

      AssertResults(results.Shirts, searchOptions);
      AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
      AssertColorShouldBeInSearchCounts(shirts, searchOptions, results.ColorCounts);
    }

    [Test]
    public void TestMultipleColorSoFilteredBySize()
    {
      var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red),
                new Shirt(Guid.NewGuid(), "White - Small", Size.Small, Color.White),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

      var searchOptions = new SearchOptions
      {
        Colors = new List<Color> { Color.Red },
        Sizes = new List<Size> { Size.Small }
      };
      var searchEngine = new SearchEngine(shirts);
      var results = searchEngine.Search(searchOptions);

      AssertResults(results.Shirts, searchOptions);
      AssertSizeCountsShouldBeInSearch(shirts, searchOptions, results.SizeCounts);
      AssertColorShouldBeInSearchCounts(shirts, searchOptions, results.ColorCounts);
    }

    [Test]
    public void TestMultipleColorSearch()
    {
      var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };
      var searchOptions = new SearchOptions
      {
        Colors = new List<Color> { Color.Red, Color.Black },
        Sizes = new List<Size> { Size.Small }
      };
      var searchEngine = new SearchEngine(shirts);
      var results = searchEngine.Search(searchOptions);
      AssertResults(results.Shirts, searchOptions);
      AssertSizeCountsShouldBeInSearch(shirts, searchOptions, results.SizeCounts);
      AssertColorShouldBeInSearchCounts(shirts, searchOptions, results.ColorCounts);
    }

    [Test]
    public void TestColorNotFound()
    {
      var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };
      var searchOptions = new SearchOptions
      {
        Colors = new List<Color> { Color.White },
        Sizes = new List<Size> { Size.Small }
      };
      var searchEngine = new SearchEngine(shirts);
      var results = searchEngine.Search(searchOptions);
      AssertResults(results.Shirts, searchOptions);
      AssertSizeCountsShouldBeInSearch(shirts, searchOptions, results.SizeCounts);
      AssertColorShouldBeInSearchCounts(shirts, searchOptions, results.ColorCounts);
    }



    [Test]
    public void TestRedSearch()
    {
      var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

      var searchEngine = new SearchEngine(shirts);

      var searchOptions = new SearchOptions
      {
        Colors = new List<Color> { Color.Red }       
      };

      var results = searchEngine.Search(searchOptions);

      AssertResults(results.Shirts, searchOptions);
      AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
      AssertColorShouldBeInSearchCounts(shirts, searchOptions, results.ColorCounts);
    }

    [Test]
    public void Test()
    {
      var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

      var searchEngine = new SearchEngine(shirts);

      var searchOptions = new SearchOptions
      {
        Colors = new List<Color> { Color.Red },
        Sizes = new List<Size> { Size.Small }
      };

      var results = searchEngine.Search(searchOptions);

      AssertResults(results.Shirts, searchOptions);
      AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
      AssertColorCounts(shirts, searchOptions, results.ColorCounts);
    }

    [Test]
    public void Testme()
    {
      Console.WriteLine("Test run");
    }


  }
}
