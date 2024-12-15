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

    private static bool MoveBox(Grid<char> map, Point box, Point vect)
    {
        var newPos = box + vect;
        bool canMove = (map[newPos] == '.')
            || (map[newPos] != '#'
            && MoveBox(map, newPos, vect));

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
            var vect = vectors[Array.IndexOf(directions, instruction)];
            var newPos = player + vect;

            if (map[newPos] == '.' || map[newPos] == 'O' && MoveBox(map, newPos, vect))
            {
                player = newPos;
            }
        }

        return $"{map.AllPositions()
            .Where(p => map[p] == 'O')
            .Sum(p => 100 * p.Y + p.X)}";
    }

    // moves itself and all others onward, returning whether it moved, indicating that `point` is empty and can be moved to
    private static bool MoveBoxes(Point left, Point dir, List<Point> boxes, Point[] walls)
    {
        bool canMove;
        var right = left + (1, 0);
        var newLeft = left + dir;
        var newRight = right + dir;

        if (walls.Contains(newLeft) || walls.Contains(newRight))
            return false;

        // if this space is empty we can move there:
        // For vertical movement that means checking new left, new left - 1, new left + 1 (new right)
        // For left movement we DO NOT check new left + 1, because that's where we came from
        // For right movement we DO NOT check new left - 1, because that's where we came from
        if (!boxes.Contains(newLeft)
            && (dir.X == 0 ? !boxes.Contains(newRight) && !boxes.Contains(newLeft + (-1, 0)) :
                  dir.X < 0 &&    !boxes.Contains(newLeft + (-1, 0))
            || /* dir.X > 0 && */ !boxes.Contains(newRight)))
            canMove = true;

        // going horizontal is trivial as boxes have height 1
        // we go in steps of 2 to skip the right sides. .[][].. -> ...[][]
        // but if theres empty space we only go 1 step  .[].... -> ..[]...
        else if (dir.X != 0)
        {
            var doubleStep = newLeft + dir;
            if (boxes.Contains(doubleStep))
                canMove = MoveBoxes(doubleStep, dir, boxes, walls);
            else
                canMove = MoveBoxes(newLeft, dir, boxes, walls);
        }

        // going vertical is anything but trivial
        else
        {
            // first check for the trivial case, the boxes are aligned vertically
            // []
            // []

            if (boxes.Contains(newLeft))
                canMove = MoveBoxes(newLeft, dir, boxes, walls);
            else
            {
                // the harder case: boxes are offset
                // 1     2     3
                // []    []   [][]
                //  []  []     []

                // 1 (and 3)
                canMove = true;
                if (boxes.Contains(newLeft + (-1, 0)))
                    canMove &= MoveBoxes(newLeft + (-1, 0), dir, boxes, walls);
                // 2 (and 3)
                if (boxes.Contains(newRight))
                    canMove &= MoveBoxes(newRight, dir, boxes, walls);
            }
        }

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

        //PrintMap(map, player, boxes, walls);
        for (int i = 0; i < instructions.Length; i++)
        {
            var instruction = instructions[i];
            var vect = vectors[Array.IndexOf(directions, instruction)];
            var newPos = player + vect;

            if (!walls.Contains(newPos))
            {
                var newBoxes = boxes.ToList();

                var left = boxes.Contains(newPos);
                var right = boxes.Contains(newPos + (-1, 0));
                if (!left && !right)
                    player = newPos;
                else if (MoveBoxes(left ? newPos : newPos + (-1, 0), vect, newBoxes, walls))
                {
                    player = newPos;
                    boxes = newBoxes;
                }
            }

            //PrintMap(map, player, boxes, walls);
        }
        //PrintMap(map, player, boxes, walls);
        return $"{boxes.Sum(p => 100 * p.Y + p.X)}";
    }

    private static void PrintMap(Grid<char> map, Point player, List<Point> boxes, Point[] walls)
    {
        Console.SetCursorPosition(0, 0);
        for (int r = 0; r < map.Height; r++)
        {
            for (int c = 0; c < map.Width; c++)
            {
                Console.Write(player == (c, r) ? '@'
                    : walls.Contains((c, r)) ? '#'
                    : boxes.Contains((c, r)) ? '['
                    : boxes.Contains((c - 1, r)) ? ']' : '.');
            }
            Console.WriteLine();
        }
        Console.ReadKey();
    }
}