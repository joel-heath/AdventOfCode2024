using AdventOfCode2024.Utilities.Matrices;
using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day13 : IDay
{
    public int Day => 13;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "Button A: X+94, Y+34\r\nButton B: X+22, Y+67\r\nPrize: X=8400, Y=5400\r\n\r\nButton A: X+26, Y+66\r\nButton B: X+67, Y+21\r\nPrize: X=12748, Y=12176\r\n\r\nButton A: X+17, Y+86\r\nButton B: X+84, Y+37\r\nPrize: X=7870, Y=6450\r\n\r\nButton A: X+69, Y+23\r\nButton B: X+27, Y+71\r\nPrize: X=18641, Y=10279", "480" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "Button A: X+94, Y+34\r\nButton B: X+22, Y+67\r\nPrize: X=8400, Y=5400\r\n\r\nButton A: X+26, Y+66\r\nButton B: X+67, Y+21\r\nPrize: X=12748, Y=12176\r\n\r\nButton A: X+17, Y+86\r\nButton B: X+84, Y+37\r\nPrize: X=7870, Y=6450\r\n\r\nButton A: X+69, Y+23\r\nButton B: X+27, Y+71\r\nPrize: X=18641, Y=10279", "875318608908" }
    };

    public string SolvePart1(string input)
    {
        long summation = 0;
        var games = input.Split(Environment.NewLine + Environment.NewLine)
            .Select(g => g.Split(Environment.NewLine))
            .Select(g =>
            {
                var buttons = g.Take(2)
                    .Select(b => Regex.Matches(b, @"\d+")
                    .Select(m => long.Parse(m.Value)).ToArray())
                    .Select(m => (x: m[0], y: m[1]))
                    .ToArray();
                var prize = Regex.Matches(g.Last(), @"\d+").Select(m => long.Parse(m.Value)).ToArray();
                return (buttons, (x: prize[0], y: prize[1]));
            });

        foreach (var (buttons, prize) in games)
        {
            Matrix m = new(
            [
                [buttons[0].x, buttons[1].x],
                [buttons[0].y, buttons[1].y]
            ]);

            Matrix v = new(
            [
                [prize.x],
                [prize.y]
            ]);

            Matrix result = m.Inverse * v;

            /*
            if (result[0, 0] > 100 || result[1, 0] > 100)
                continue;*/

            if (Math.Abs(result[0, 0] - Math.Round(result[0, 0])) > 0.0001 || Math.Abs(result[1, 0] - Math.Round(result[1, 0])) > 0.0001)
                continue;


            summation += 3 * (long)Math.Round(result[0, 0]) + (long)Math.Round(result[1, 0]);
        }

        return $"{summation}";
    }

    public string SolvePart2(string input)
    {
        double summation = 0;
        var games = input.Split(Environment.NewLine + Environment.NewLine)
            .Select(g => g.Split(Environment.NewLine))
            .Select(g =>
            {
                var buttons = g.Take(2)
                    .Select(b => Regex.Matches(b, @"\d+")
                    .Select(m => long.Parse(m.Value)).ToArray())
                    .Select(m => (x: m[0], y: m[1]))
                    .ToArray();
                var prize = Regex.Matches(g.Last(), @"\d+").Select(m => long.Parse(m.Value)).ToArray();
                return (buttons, (x: prize[0] + 10000000000000, y: prize[1] + 10000000000000));
            });

        foreach (var (buttons, prize) in games)
        {
            Matrix m = new(
            [
                [buttons[0].x, buttons[1].x],
                [buttons[0].y, buttons[1].y]
            ]);

            Matrix v = new(
            [
                [prize.x],
                [prize.y]
            ]);

            Matrix result = m.Inverse * v;

            /*
            if (double.IsInfinity(result[0, 0]) || double.IsNaN(result[0, 0])
                || double.IsInfinity(result[1, 0]) || double.IsNaN(result[1, 0]))
                continue;

            if (!double.IsPositive(result[0, 0]) || !double.IsPositive(result[1, 0]))
                continue;
            */

            if (Math.Abs(result[0, 0] - Math.Round(result[0, 0])) > 0.0001 || Math.Abs(result[1, 0] - Math.Round(result[1, 0])) > 0.0001)
            //if (!double.IsInteger(result[0, 0]) || !double.IsInteger(result[1, 0]))
                continue;

            summation += 3 * result[0, 0] + result[1, 0];
        }

        return $"{summation}";
    }
}