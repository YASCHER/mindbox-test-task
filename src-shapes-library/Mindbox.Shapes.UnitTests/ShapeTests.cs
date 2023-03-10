using Mindbox.Shapes.UnitTests.Fixtures;
using Xunit;

namespace Mindbox.Shapes.UnitTests;

public class ShapeTests : IClassFixture<ShapeTestsFixture>
{
    private readonly ShapeTestsFixture _fixture;

    public ShapeTests(ShapeTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "Площадь и периметр без знания типа фигуры в compile-time")]
    public void DifferentShapesTest()
    {
        var shapeInfos = new (Shape Shape, double ExpectedArea, double ExpectedPerimeter)[]
        {
            (new Circle(5), 78.539, 31.415),
            (new Triangle(3, 4, 5), 6, 12),
            (new Rectangle(10, 3), 30, 26)
        };

        foreach (var shapeInfo in shapeInfos)
        {
            var area = shapeInfo.Shape.CalculateArea();

            Assert.True(Math.Abs(area - shapeInfo.ExpectedArea) < _fixture.Tolerance);

            var perimeter = shapeInfo.Shape.CalculatePerimeter();

            Assert.True(Math.Abs(perimeter - shapeInfo.ExpectedPerimeter) < _fixture.Tolerance);
        }
    }

    #region Circle

    [Theory(DisplayName = "Circle: площадь")]
    [InlineData(5, 78.539)]
    [InlineData(3, 28.274)]
    [InlineData(4.11, 53.068)]
    [InlineData(124.98, 49071.678)]
    public void CircleAreaTheory(double radius, double expectedArea)
    {
        var circle = new Circle(radius);

        var area = circle.CalculateArea();

        Assert.True(Math.Abs(area - expectedArea) < _fixture.Tolerance);
    }

    [Theory(DisplayName = "Circle: периметр")]
    [InlineData(7, 43.982)]
    [InlineData(3, 18.849)]
    public void CirclePerimeterTheory(double radius, double expectedPerimeter)
    {
        var circle = new Circle(radius);

        var perimeter = circle.CalculatePerimeter();

        Assert.True(Math.Abs(perimeter - expectedPerimeter) < _fixture.Tolerance);
    }

    [Theory(DisplayName = "Circle: некорректный радиус")]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(int.MinValue)]
    [InlineData(-3.3)]
    public void CircleInvalidRadiusTheory(double radius)
    {
        try
        {
            var circle = new Circle(radius);
        }
        catch (ArgumentException)
        {
            Assert.True(radius <= 0);
        }
    }

    #endregion

    #region Triangle

    [Theory(DisplayName = "Triangle: площадь")]
    [InlineData(3, 4.6, 3, 4.430)]
    [InlineData(5, 3.94, 2, 3.694)]
    public void TriangleAreaTheory(double a, double b, double c, double expectedArea)
    {
        var triangle = new Triangle(a, b, c);

        var area = triangle.CalculateArea();

        Assert.True(Math.Abs(area - expectedArea) < _fixture.Tolerance);
    }

    [Theory(DisplayName = "Triangle: периметр")]
    [InlineData(7, 7, 7, 21)]
    [InlineData(3, 4.6, 3, 10.6)]
    public void TrianglePerimeterTheory(double a, double b, double c, double expectedPerimeter)
    {
        var triangle = new Triangle(a, b, c);

        var perimeter = triangle.CalculatePerimeter();

        Assert.True(Math.Abs(perimeter - expectedPerimeter) < _fixture.Tolerance);
    }

    [Theory(DisplayName = "Triangle: некорректные длины сторон")]
    [InlineData(0, 6, 9)]
    [InlineData(56, -5, 8)]
    [InlineData(8, int.MinValue, 12)]
    [InlineData(4, 8, -3.3)]
    public void TriangleInvalidSidesTheory(double a, double b, double c)
    {
        try
        {
            var triangle = new Triangle(a, b, c);
        }
        catch (ArgumentException)
        {
            Assert.True(a <= 0 || b <= 0 || c <= 0);
        }
    }

    [Theory(DisplayName = "Triangle: не существует треугольника с такими сторонами")]
    [InlineData(15, 2, 9)]
    [InlineData(4, 9, 5)]
    [InlineData(3, 2, 1)]
    [InlineData(10, 15, 2)]
    public void SuchTriangleDoesNotExistTheory(double a, double b, double c)
    {
        try
        {
            var triangle = new Triangle(a, b, c);
        }
        catch (ArgumentException ex) when (ex.Message == "A triangle with specified sides does not exist")
        {
            Assert.True(a >= b + c || b >= a + c || c >= a + b);
        }
    }

    [Theory(DisplayName = "Triangle: прямоугольный треугольник")]
    [InlineData(3, 4, 5, true)]
    [InlineData(48, 55, 73, true)]
    [InlineData(1.4142, 1, 1, true)]
    [InlineData(10, 11, 2, false)]
    public void TriangleIsRightTheory(double a, double b, double c, bool expectedIsRightTriangle)
    {
        var triangle = new Triangle(a, b, c);

        Assert.True(triangle.IsRightTriangle() == expectedIsRightTriangle);
    }

    #endregion

    #region Rectangle

    [Theory(DisplayName = "Rectangle: площадь")]
    [InlineData(5, 5, 25)]
    [InlineData(5.33, 3, 15.99)]
    public void RectangleAreaTheory(double a, double b, double expectedArea)
    {
        var rectangle = new Rectangle(a, b);

        var area = rectangle.CalculateArea();

        Assert.True(Math.Abs(area - expectedArea) < _fixture.Tolerance);
    }

    [Theory(DisplayName = "Rectangle: периметр")]
    [InlineData(7, 7, 28)]
    [InlineData(3, 4.6, 15.2)]
    public void RectanglePerimeterTheory(double a, double b, double expectedPerimeter)
    {
        var rectangle = new Rectangle(a, b);

        var perimeter = rectangle.CalculatePerimeter();

        Assert.True(Math.Abs(perimeter - expectedPerimeter) < _fixture.Tolerance);
    }

    [Theory(DisplayName = "Rectangle: некорректные длины сторон")]
    [InlineData(92, 0)]
    [InlineData(4, -0.5)]
    [InlineData(12, int.MinValue)]
    public void RectangleInvalidSidesTheory(double a, double b)
    {
        try
        {
            var rectangle = new Rectangle(a, b);
        }
        catch (ArgumentException)
        {
            Assert.True(a <= 0 || b <= 0);
        }
    }

    #endregion
}