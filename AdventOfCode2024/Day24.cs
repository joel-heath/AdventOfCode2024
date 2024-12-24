using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day24 : IDay
{
    public int Day => 24;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "x00: 1\r\nx01: 0\r\nx02: 1\r\nx03: 1\r\nx04: 0\r\ny00: 1\r\ny01: 1\r\ny02: 1\r\ny03: 1\r\ny04: 1\r\n\r\nntg XOR fgs -> mjb\r\ny02 OR x01 -> tnw\r\nkwq OR kpj -> z05\r\nx00 OR x03 -> fst\r\ntgd XOR rvg -> z01\r\nvdt OR tnw -> bfw\r\nbfw AND frj -> z10\r\nffh OR nrd -> bqk\r\ny00 AND y03 -> djm\r\ny03 OR y00 -> psh\r\nbqk OR frj -> z08\r\ntnw OR fst -> frj\r\ngnj AND tgd -> z11\r\nbfw XOR mjb -> z00\r\nx03 OR x00 -> vdt\r\ngnj AND wpb -> z02\r\nx04 AND y00 -> kjc\r\ndjm OR pbm -> qhw\r\nnrd AND vdt -> hwm\r\nkjc AND fst -> rvg\r\ny04 OR y02 -> fgs\r\ny01 AND x02 -> pbm\r\nntg OR kjc -> kwq\r\npsh XOR fgs -> tgd\r\nqhw XOR tgd -> z09\r\npbm OR djm -> kpj\r\nx03 XOR y03 -> ffh\r\nx00 XOR y04 -> ntg\r\nbfw OR bqk -> z06\r\nnrd XOR fgs -> wpb\r\nfrj XOR qhw -> z04\r\nbqk OR frj -> z07\r\ny03 OR x01 -> nrd\r\nhwm AND bqk -> z03\r\ntgd XOR rvg -> z12\r\ntnw OR pbm -> gnj", "2024" }
    };
    public Dictionary<string, string> UnitTestsP2 => [];

    private static (Dictionary<string, bool> registers, Dictionary<string, (string op1, string op2, string opCode)> map, int zRegCount) ParseInput(string input)
    {
        var sections = input.Split(Environment.NewLine + Environment.NewLine);
        var registers = sections[0].Split(Environment.NewLine)
            .Select(l => l.Split(": "))
            .ToDictionary(l => l[0], l => l[1] == "1");
        Dictionary<string, (string op1, string op2, string opCode)> map =
            sections[1].Split(Environment.NewLine).Select(l => l.Split(' '))
            .ToDictionary(l => l[^1], l => (l[0], l[2], l[1]));

        return (registers, map, map.Keys.Count(k => k[0] == 'z'));
    }

    private static bool EvaluateRegister(Dictionary<string, (string op1, string op2, string opCode)> map, Dictionary<string, bool> registers, string register)
    {
        var (op1, op2, opCode) = map[register];
        var val1 = registers.TryGetValue(op1, out bool v1)
            ? v1 : EvaluateRegister(map, registers, op1);
        var val2 = registers.TryGetValue(op2, out bool v2)
            ? v2 : EvaluateRegister(map, registers, op2);

        return registers[register] = opCode switch
        {
            "AND" => val1 & val2,
            "OR" => val1 | val2,
            "XOR" => val1 ^ val2,
            _ => throw new NotImplementedException()
        };
    }

    public string SolvePart1(string input)
    {
        var (registers, map, zRegCount) = ParseInput(input);

        return $"{Enumerable.Range(0, zRegCount)
            .Sum(i => EvaluateRegister(map, registers, $"z{i:00}")
                ? Math.Pow(2, i) : 0)}";
    }

    public string SolvePart2(string input)
    {
        var (registers, map, zRegCount) = ParseInput(input);

        return $"{string.Join(',',
            map.Select(c => (res: c.Key, c.Value.op1, c.Value.op2, c.Value.opCode))
                .Where(c =>
                    // All outputs must by XORs, except the last bit which is the carry out bit of the last x & y bits.
                    (c.res[0] == 'z' && c.opCode != "XOR" && c.res != $"z{zRegCount - 1}")
                    // All XORs must be have at least an x input, y input or z output. If has none of these it's invalid.
                    || (c.opCode == "XOR" && c.res[0] != 'z' && c.op1[0] != 'x' && c.op2[0] != 'x' && c.op1[0] != 'y' && c.op2[0] != 'y')
                    // All ANDs (except x00 & y00) must go into ORs. (x00 & y00 goes straight into XOR with x01, x02)
                    || (c.opCode == "AND" && !(c.op1 == "x00" || c.op2 == "x00") && map.Where(kvp => kvp.Value.op1 == c.res || kvp.Value.op2 == c.res).Any(kvp => kvp.Value.opCode != "OR"))
                    // All XORs must NOT go into ORs.
                    || (c.opCode == "XOR" && map.Where(kvp => kvp.Value.op1 == c.res || kvp.Value.op2 == c.res).Any(kvp => kvp.Value.opCode == "OR")))
                .Select(c => c.res)
                .Order())}";
    }
}