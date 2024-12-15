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

    /*
    private static bool MoveBox2(Point box, Point vect, List<Point> boxes, Point[] walls)
    {
        if (vect.Y == 0 && boxes.Contains(box + vect))
            return MoveBox2(box + vect, vect, boxes, walls);

        var left = box + vect;
        var right = left + (1, 0);

        //bool canMove = !walls.Contains(left) && !walls.Contains(right)
        ////  if going left, `right` will be ourself, so no need to check for wall blocking on the right
        //    && (!boxes.Contains(left) && (right == box || !boxes.Contains(right))
        //    || MoveBox2(left, vect, boxes, walls));
        bool canMove = !walls.Contains(left) && !walls.Contains(right);
        if (canMove)
        {
            var temp = !boxes.Contains(left) && !boxes.Contains(right);
            if (!temp)
            {
                temp = MoveBox2(left, vect, boxes, walls);
            }
            canMove = temp;
        }

        if (canMove)
        {
            boxes.Remove(box);
            boxes.Add(left);
        }
        return canMove;
    }*/

    private static bool MoveBox2(Point box, Point vect, List<Point> boxes, Point[] walls)
    {
        if (vect.Y == 0 && boxes.Contains(box + vect))
            return MoveBox2(box + vect, vect, boxes, walls);

        if (!walls.Contains(box) && !walls.Contains(box + (1, 0)) && !boxes.Contains(box) && (vect.X == -1 || !boxes.Contains(box + (1, 0))))
        {
            boxes.Add(box);
            return true;
        }

        bool canMove;
        var left = box + vect;
        var right = left + (1, 0);
        if (vect.Y == 0)
        {
            canMove = !walls.Contains(left) && !walls.Contains(right)
                && (!boxes.Contains(left) && !boxes.Contains(right)
                || MoveBox2(left, vect, boxes, walls));
        }
        else
        {
            canMove = !walls.Contains(left) && !walls.Contains(right)
                && (!boxes.Contains(left) && !boxes.Contains(right)
                || (MoveBox2(left, vect, boxes, walls) && MoveBox2(right, vect, boxes, walls)));
        }


        if (canMove)
        {
            boxes.Remove(box);
            boxes.Add(left);
        }
        return canMove;
    }

    /*
    private static bool MoveBox2(Grid<char> map, Point box, Point vect)
    {
        var newPos = box + vect;
        bool canMove = (map[newPos] == '.' && map[newPos + (1, 0)] != '.')
            || (map[newPos] != '#' && map[newPos + (1, 0)] != '#'
            && MoveBox2(map, newPos, vect));

        if (canMove)
        {
            map[box] = '.';
            map[box + (1, 0)] = '.';
            map[newPos] = '[';
            map[newPos + (1, 0)] = ']';
        }
        return canMove;
    }*/

    public string SolvePart2(string input)
    {
        /*
        var data = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var map = Grid<char>.FromString(string.Join(Environment.NewLine,
                    data[0].Split(Environment.NewLine).Select(line => string.Concat(line.Select(c => $"{(c == 'O' ? '[' : c)}{(c == '@' ? '.' : c == 'O' ? ']' : c)}"))))); var instructions = string.Concat(data[1].Split(Environment.NewLine));
        var player = map.AllPositions().Where(p => map[p] == '@').First();
        map[player] = '.';

        for (int i = 0; i < instructions.Length; i++)
        {
            var instruction = instructions[i];
            var vect = vectors[Array.IndexOf(directions, instruction)];
            var newPos = player + vect;

            if (map[newPos] != '#')
            {
                var left = map[newPos] == '[';
                var right = map[newPos] == ']';
                if (map[newPos] == '.' || MoveBox2(map, left ? newPos : newPos + (-1, 0), vect))
                    player = newPos;
            }

            Console.SetCursorPosition(0, 0);
            for (int r = 0; r < map.Height; r++)
            {
                for (int c = 0; c < map.Width; c++)
                {
                    if (player == (c, r))
                        Console.Write('@');
                    else
                        Console.Write(map[(c, r)]);
                }
                Console.WriteLine();
            }
            Console.ReadKey();
            
        }

        return $"{map.AllPositions()
            .Where(p => map[p] == '[')
            .Sum(p => 100 * p.Y + p.X)}";
        */

        
        var data = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        
        var map = Grid<char>.FromString(string.Join(Environment.NewLine,
            data[0].Split(Environment.NewLine).Select(line => string.Concat(line.Select(c => $"{(c == 'O' ? '[' : c)}{(c == '@' ? '.' : c == 'O' ? ']' : c)}")))));
        var instructions = string.Concat(data[1].Split(Environment.NewLine));

        var player = map.AllPositions().Where(p => map[p] == '@').First();
        map[player] = '.';

        var boxes = map.AllPositions().Where(p => map[p] == '[').ToList();
        Point[] walls = map.AllPositions().Where(p => map[p] == '#').ToArray();

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
        for (int i = 0; i < instructions.Length; i++)
        {
            var instruction = instructions[i];
            var vect = vectors[Array.IndexOf(directions, instruction)];
            var newPos = player + vect;

            if (!walls.Contains(newPos))
            {
                var left = boxes.Contains(newPos);
                var right = boxes.Contains(newPos + (-1, 0));
                if (!left && !right)
                    player = newPos;
                //else if (MoveBox2(left ? newPos : newPos + (-1, 0), vect, boxes, walls))
                else if (MoveBox2(newPos, vect, boxes, walls))
                    player = newPos;
            }

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

        return $"{boxes.Sum(p => 100 * p.Y + p.X)}";
    }
}