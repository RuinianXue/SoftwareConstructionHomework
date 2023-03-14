// See https://aka.ms/new-console-template for more information
using System;
public interface ShapeI
{
    double area();
    bool validShape();
}
public class Rectangle : ShapeI
{
    public double length
    {
        get;
        set;
    }
    public double width
    {
        get;
        set;
    }
    public double area()
    {
        return length * width;
    }
    public Rectangle(double length, double width)
    {
        this.length = length;
        this.width = width;
    }
    public bool validShape()
    {
        if (length == 0 || width == 0) return false;
        return true;
    }
}

public class Square : Rectangle
{
    public Square(double side) :base(side,side)
    {
    }
    public new bool validShape()
    {
        if (length != width) return false;
        if (length == 0) return false;
        if (width == 0) return false;
        return true;
    }
}
public class Triangle : ShapeI
{
    public double a
    {
        get;
        set;
    }
    public double b
    {
        get;
        set;
    }
    public double c
    {
        get;
        set;
    }
    public double area()
    {
        double p = (a + b + c) / 2.00;
        return System.Math.Sqrt(p * (p - a) * (p - b) * (p - c));
    }
    public bool validShape()
    {
        if (a < b + c) return false;
        if (b < a + c) return false;
        if (c < a + b) return false;
        return true;
    }
    public Triangle(double a, double b, double c)
    {
        this.a = a;
        this.b = b;
        this.c = c;
    }
}
public class Circle : ShapeI
{
    private double r;
    public double area()
    {
        return 3.14 * r * r;
    }
    public bool validShape()
    {
        if (r != 0) return true;
        return false;
    }
    public Circle(double r)
    {
        this.r = r;
    }
}
public class ShapeFactory
{
    public static ShapeI CreateShape(string shapeType, params double[] arg)
    {
        switch (shapeType)
        {
            case "Rectangle":
                return new Rectangle(arg[0], arg[1]);
            case "Square":
                return new Square(arg[0]);
            case "Triangle":
                return new Triangle(arg[0], arg[1], arg[2]);
            case "Cricle":
                return new Circle(arg[0]);
            default:
                return new Square(0);
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        Random random = new Random();

        List<ShapeI> shapes = new List<ShapeI>();

        for (int i = 0; i < 10; i++)
        {
            string shapeType = random.Next(4) switch
            {
                0 => "Rectangle",
                1 => "Square",
                2 => "Cricle",
                _ => "Triangle"
            };

            int argCount = shapeType switch
            {
                "Rectangle" => 2,
                "Square" => 1,
                "Cricle" => 1,
                _ => 3
            };
            double[] arg = new double[argCount];
            for (int j = 0; j < argCount; j++)
            {
                arg[j] = random.Next(1, 11);
            }

            ShapeI shape = ShapeFactory.CreateShape(shapeType, arg);
            shapes.Add(shape);
        }

        double totalArea = 0.0;
        foreach (ShapeI shape in shapes)
        {
            if (shape.validShape())
            {
                totalArea += shape.area();
            }
        }

        // 输出结果  
        Console.WriteLine("The total area of random generated shapes is {0}", totalArea);
    }
}
