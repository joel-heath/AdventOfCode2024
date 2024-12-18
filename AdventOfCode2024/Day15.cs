using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day15 : IDay
{
    public int Day => 15;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "########\r\n#..O.O.#\r\n##@.O..#\r\n#...O..#\r\n#.#.O..#\r\n#...O..#\r\n#......#\r\n########\r\n\r\n<^^>>>vv<v>>v<<", "2028" },
        { "##########\r\n#..O..O.O#\r\n#......O.#\r\n#.OO..O.O#\r\n#..O@..O.#\r\n#O#..O...#\r\n#O..O..O.#\r\n#.OO.O.OO#\r\n#....O...#\r\n##########\r\n\r\n<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^\r\nvvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v\r\n><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<\r\n<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^\r\n^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><\r\n^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^\r\n>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^\r\n<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>\r\n^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>\r\nv^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^", "10092" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "#######\r\n#...#.#\r\n#.....#\r\n#..OO@#\r\n#..O..#\r\n#.....#\r\n#######\r\n\r\n<vv<<^^<<^^", "618" },
        { "##########\r\n#..O..O.O#\r\n#......O.#\r\n#.OO..O.O#\r\n#..O@..O.#\r\n#O#..O...#\r\n#O..O..O.#\r\n#.OO.O.OO#\r\n#....O...#\r\n##########\r\n\r\n<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^\r\nvvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v\r\n><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<\r\n<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^\r\n^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><\r\n^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^\r\n>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^\r\n<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>\r\n^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>\r\nv^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^", "9021" },
    };

    private static readonly char[] directions = ['^', '>', 'v', '<'];
    private static readonly Point[] vectors = [(0, -1), (1, 0), (0, 1), (-1, 0)];

    private static bool MoveBoxesP1(Grid<char> map, Point box, Point vect)
    {
        var newPos = box + vect;
        bool canMove = (map[newPos] == '.')
            || (map[newPos] != '#'
            && MoveBoxesP1(map, newPos, vect));

        if (canMove)
        {
            map[box] = '.';
            map[newPos] = 'O';
        }
        return canMove;
    }

    public string SolvePart1(string input)
    {
        var data = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var map = Grid<char>.FromString(data[0]);
        var instructions = string.Concat(data[1].Split(Environment.NewLine));

        var player = map.AllPositions().Where(p => map[p] == '@').First();
        map[player] = '.';

        for (int i = 0; i < instructions.Length; i++)
        {
            var instruction = instructions[i];
            var dir = vectors[Array.IndexOf(directions, instruction)];
            var newPos = player + dir;

            if (map[newPos] == '.' || map[newPos] == 'O' && MoveBoxesP1(map, newPos, dir))
                player = newPos;
        }

        return $"{map.AllPositions()
            .Where(p => map[p] == 'O')
            .Sum(p => 100 * p.Y + p.X)}";
    }

    private static bool MoveBoxesP2(Point left, Point dir, List<Point> boxes, Point[] walls)
    {
        var right = left + (1, 0);
        var newLeft = left + dir;
        var newRight = right + dir;
        var doubleStep = newLeft + dir;

        bool canMove = !walls.Contains(newLeft) && !walls.Contains(newRight) && (
            !boxes.Contains(newLeft)
            && (dir.X == 0
                ? !boxes.Contains(newRight) && !boxes.Contains(newLeft + (-1, 0))
                : dir.X < 0 && !boxes.Contains(newLeft + (-1, 0)) || !boxes.Contains(newRight))
            || (dir.X != 0
                ? boxes.Contains(doubleStep)
                    ? MoveBoxesP2(doubleStep, dir, boxes, walls)
                    : MoveBoxesP2(newLeft, dir, boxes, walls)
                : boxes.Contains(newLeft)
                    ? MoveBoxesP2(newLeft, dir, boxes, walls)
                    : ((!boxes.Contains(newLeft + (-1, 0)) || MoveBoxesP2(newLeft + (-1, 0), dir, boxes, walls))
                        && (!boxes.Contains(newRight) || MoveBoxesP2(newRight, dir, boxes, walls)))));

        if (canMove)
        {
            boxes.Remove(left);
            boxes.Add(newLeft);
        }
        return canMove;
    }

    public string SolvePart2(string input)
    {
        var data = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        
        var map = Grid<char>.FromString(string.Join(Environment.NewLine,
            data[0].Split(Environment.NewLine).Select(line => string.Concat(line.Select(c => $"{(c == 'O' ? '[' : c)}{(c == '@' ? '.' : c == 'O' ? ']' : c)}")))));
        var instructions = string.Concat(data[1].Split(Environment.NewLine));

        var player = map.AllPositions().Where(p => map[p] == '@').First();
        map[player] = '.';

        var boxes = map.AllPositions().Where(p => map[p] == '[').ToList();
        Point[] walls = map.AllPositions().Where(p => map[p] == '#').ToArray();

        for (int i = 0; i < instructions.Length; i++)
        {
            var instruction = instructions[i];
            var dir = vectors[Array.IndexOf(directions, instruction)];
            var newPos = player + dir;

            if (!walls.Contains(newPos))
            {
                var newBoxes = boxes.ToList();

                var left = boxes.Contains(newPos);
                var right = boxes.Contains(newPos + (-1, 0));
                if (!left && !right)
                    player = newPos;
                else if (MoveBoxesP2(left ? newPos : newPos + (-1, 0), dir, newBoxes, walls))
                {
                    player = newPos;
                    boxes = newBoxes;
                }
            }
        }

        return $"{boxes.Sum(p => 100 * p.Y + p.X)}";
    }
}