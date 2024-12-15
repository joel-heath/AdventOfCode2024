using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day14 : IDay
{
    public int Day => 14;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "p=0,4 v=3,-3\r\np=6,3 v=-1,-3\r\np=10,3 v=-1,2\r\np=2,0 v=2,-1\r\np=0,0 v=1,3\r\np=3,0 v=-2,-2\r\np=7,6 v=-1,-3\r\np=3,0 v=-1,-2\r\np=9,3 v=2,3\r\np=7,3 v=-1,2\r\np=2,4 v=2,-3\r\np=9,5 v=-3,-3", "12" }
    };
    public Dictionary<string, string> UnitTestsP2 => [];

    private static (Point p, Point v) MoveRobot((Point p, Point v) robot, int width, int height, int k = 1)
        => (p: (Utils.Mod(robot.p.X + k * robot.v.X, width), Utils.Mod(robot.p.Y + k * robot.v.Y, height)), robot.v);

    private static int[] CountQuadrants((Point p, Point v)[] robots, int width, int height)
        => robots.Select(r =>
                  r.p.X < width / 2 && r.p.Y < height / 2 ? 0
                : r.p.X > width / 2 && r.p.Y < height / 2 ? 1
                : r.p.X < width / 2 && r.p.Y > height / 2 ? 2
                : r.p.X > width / 2 && r.p.Y > height / 2 ? 3
                : -1)
            .Where(x => x != -1)
            .GroupBy(x => x)
            .Select(g => g.Count())
            .ToArray();

    private static bool RobotsOverlap((Point p, Point v)[] robots)
        => robots.Select(r => r.p).Distinct().Count() != robots.Length;

    public string SolvePart1(string input)
    {
        var (width, height) = UnitTestsP1.ContainsKey(input) ? (11, 7) : (101, 103);
        var robots = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(l => Utils.GetLongs(l).ToList())
            .Select(l => (p: new Point(l[0], l[1]), v: new Point(l[2], l[3])))
            .Select(r => MoveRobot(r, width, height, 100))
            .ToArray();

        return $"{CountQuadrants(robots, width, height).Aggregate((x, y) => x * y)}";
    }

    public string SolvePart2(string input)
    {
        var (width, height) = UnitTestsP1.ContainsKey(input) ? (11, 7) : (101, 103);
        var robots = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(l => Utils.GetLongs(l).ToList())
            .Select(l => (p: new Point(l[0], l[1]), v: new Point(l[2], l[3])))
            .ToArray();

        int seconds;
        for (seconds = 0; RobotsOverlap(robots); seconds++)
            robots = robots.Select(r => MoveRobot(r, width, height)).ToArray();

        return $"{seconds}";
    }
}