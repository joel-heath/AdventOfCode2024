using AdventOfCode2024.Utilities;
using System.Numerics;

namespace AdventOfCode2024;

public class Day14 : IDay
{
    public int Day => 14;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "p=0,4 v=3,-3\r\np=6,3 v=-1,-3\r\np=10,3 v=-1,2\r\np=2,0 v=2,-1\r\np=0,0 v=1,3\r\np=3,0 v=-2,-2\r\np=7,6 v=-1,-3\r\np=3,0 v=-1,-2\r\np=9,3 v=2,3\r\np=7,3 v=-1,2\r\np=2,4 v=2,-3\r\np=9,5 v=-3,-3", "12" },
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        //{ "p=0,4 v=3,-3\r\np=6,3 v=-1,-3\r\np=10,3 v=-1,2\r\np=2,0 v=2,-1\r\np=0,0 v=1,3\r\np=3,0 v=-2,-2\r\np=7,6 v=-1,-3\r\np=3,0 v=-1,-2\r\np=9,3 v=2,3\r\np=7,3 v=-1,2\r\np=2,4 v=2,-3\r\np=9,5 v=-3,-3", "ExpectedOutput1" },
    };

    private static (Point p, Point v) MoveRobot((Point p, Point v) robot, int width, int height)
    {
        Point newPos = robot.p + robot.v;
        newPos = (Utils.Mod(newPos.X, width), Utils.Mod(newPos.Y, height));

        return (p: newPos, robot.v);
    }

    public string SolvePart1(string input)
    {
        var (width, height) = UnitTestsP1.ContainsKey(input) ? (11, 7) : (101, 103);

        var robots = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(l => Utils.GetLongs(l).ToList())
            .Select(l => (p: new Point(l[0], l[1]), v: new Point(l[2], l[3])))
            .ToArray();


        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < robots.Length; j++)
            {
                robots[j] = MoveRobot(robots[j], width, height);
            }
        }

        long[] quadrants = new long[4];
        foreach (var (p, v) in robots)
        {
            if (p.X < width / 2 && p.Y < height / 2)
            {
                quadrants[0]++;
            }
            else if (p.X > width / 2 && p.Y < height / 2)
            {
                quadrants[1]++;
            }
            else if (p.X < width / 2 && p.Y > height / 2)
            {
                quadrants[2]++;
            }
            else if (p.X > width / 2 && p.Y > height / 2)
            {
                quadrants[3]++;
            }
        }

        return $"{quadrants.Aggregate((x, y) => x * y)}";
    }

    private static bool[][] RobotsAsMap((Point p, Point v)[] robots, int width, int height)
        => Enumerable.Range(0, width).Select(r => Enumerable.Range(0, height).Select(x => false).ToArray()).ToArray();

    private static void DrawRobots((Point p, Point v)[] robots)
    {
        foreach (var (p, v) in robots)
        {
            Console.CursorLeft = (int)p.X;
            Console.CursorTop = (int)p.Y;
            Console.Write("X");
        }
    }

    /*
    private static readonly bool[][] tree =
        new int[][]
        {
            [0, 0, 0, 1, 0, 0, 0],
            [0, 0, 1, 1, 1, 0, 0],
            [0, 1, 1, 1, 1, 1, 0],
            [1, 1, 1, 1, 1, 1, 1],
            [0, 0, 1, 0, 1, 0, 0],
            [0, 1, 0, 0, 0, 1, 0],
        }.Transpose().Select(x => x.Select(y => Convert.ToBoolean(y)).ToArray()).ToArray();

    private static bool CheckForTreeAtPos(bool[][] robots, int r, int c)
    {
        for (int x = 0; x < tree.Length; x++)
        {
            for (int y = 0; y < tree[x].Length; y++)
            {
                if (tree[x][y] && !robots[c + x][r + y])
                {
                    return false;
                }
            }
        }
        return true;
    }


    private static bool CheckForTree(bool[][] robots, int width, int height)
    {
        for (int c = 0; c < width - tree.Length + 1; c++)
        {
            for (int r = 0; r < height - tree[0].Length; r++)
            {
                if (CheckForTreeAtPos(robots, r, c))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /*
    private static bool CheckForTree(bool[][] robots, int width, int height)
    {
        int rows = height - 2;
        int totalChars = rows * 2 - 1;
        int stars = 3;
        int spaces = (totalChars - stars) / 2;
        int centering = (width - totalChars - 2) / 2;


        var (left, top) = ((width - 1) / 2, 0);
        if (!robots[left][top])
        {
            return false;
        }
        
        top = 1;
        for (int row = 0; row < rows; row++)
        {
            left = centering + spaces;
            for (int i = 0; i < stars; i++)
            {
                if (!robots[left + i][top])
                {
                    return false;
                }
            }

            top++;
            spaces--;
            stars += 2;
        }
        left = (width - 3) / 2;
        if (!robots[left][top] || !robots[left + 2][top])
            return false;

        top++;
        left--;

        if (!robots[left][top] || !robots[left + 4][top])
            return false;

        return true;
    }*/

    private static bool RobotsOverlap((Point p, Point v)[] robots)
    {
        for (int i = 0; i < robots.Length; i++)
        {
            var here = robots[i].p;
            if (robots.Where((r, j) => i != j).Any(r => r.p == here))
                return true;
        }

        return false;
    }


    public string SolvePart2(string input)
    {
        var (width, height) = UnitTestsP1.ContainsKey(input) ? (11, 7) : (101, 103);

        var robots = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(l => Utils.GetLongs(l).ToList())
            .Select(l => (p: new Point(l[0], l[1]), v: new Point(l[2], l[3])))
            .ToArray();

        bool treeFound = false;
        int i;
        for (i = 1; !treeFound; i++)
        {
            //Console.Clear();
            //DrawRobots(robots);
            for (int j = 0; j < robots.Length; j++)
            {
                robots[j] = MoveRobot(robots[j], width, height);
            }
            if (!RobotsOverlap(robots))
            {
                Console.Clear();
                DrawRobots(robots);
                treeFound = true;
                Console.ReadKey();
                break;
            }
            //if (CheckForTree(RobotsAsMap(robots, width, height), width, height))
        }

        return $"{i}";
    }
}