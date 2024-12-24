using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day24 : IDay
{
    public int Day => 24;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "x00: 1\r\nx01: 0\r\nx02: 1\r\nx03: 1\r\nx04: 0\r\ny00: 1\r\ny01: 1\r\ny02: 1\r\ny03: 1\r\ny04: 1\r\n\r\nntg XOR fgs -> mjb\r\ny02 OR x01 -> tnw\r\nkwq OR kpj -> z05\r\nx00 OR x03 -> fst\r\ntgd XOR rvg -> z01\r\nvdt OR tnw -> bfw\r\nbfw AND frj -> z10\r\nffh OR nrd -> bqk\r\ny00 AND y03 -> djm\r\ny03 OR y00 -> psh\r\nbqk OR frj -> z08\r\ntnw OR fst -> frj\r\ngnj AND tgd -> z11\r\nbfw XOR mjb -> z00\r\nx03 OR x00 -> vdt\r\ngnj AND wpb -> z02\r\nx04 AND y00 -> kjc\r\ndjm OR pbm -> qhw\r\nnrd AND vdt -> hwm\r\nkjc AND fst -> rvg\r\ny04 OR y02 -> fgs\r\ny01 AND x02 -> pbm\r\nntg OR kjc -> kwq\r\npsh XOR fgs -> tgd\r\nqhw XOR tgd -> z09\r\npbm OR djm -> kpj\r\nx03 XOR y03 -> ffh\r\nx00 XOR y04 -> ntg\r\nbfw OR bqk -> z06\r\nnrd XOR fgs -> wpb\r\nfrj XOR qhw -> z04\r\nbqk OR frj -> z07\r\ny03 OR x01 -> nrd\r\nhwm AND bqk -> z03\r\ntgd XOR rvg -> z12\r\ntnw OR pbm -> gnj", "2024" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "x00: 0\r\nx01: 1\r\nx02: 0\r\nx03: 1\r\nx04: 0\r\nx05: 1\r\ny00: 0\r\ny01: 0\r\ny02: 1\r\ny03: 1\r\ny04: 0\r\ny05: 1\r\n\r\nx00 AND y00 -> z05\r\nx01 AND y01 -> z02\r\nx02 AND y02 -> z01\r\nx03 AND y03 -> z03\r\nx04 AND y04 -> z04\r\nx05 AND y05 -> z00", "z00,z01,z02,z05" }
    };

    public string SolvePart1(string input)
    {
        var sections = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var registers = sections[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Split(": "))
            .ToDictionary(l => l[0], l => int.Parse(l[1]));
        var instructions = sections[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        for (int i = 0; instructions.Count > 0; i = i >= instructions.Count - 1 ? 0 : i + 1)
        {
            var instruction = instructions[i];
            var instr = instruction.Split(' ');
            var reg1 = instr[0];
            var reg2 = instr[2];
            var operation = instr[1];
            var regRes = instr[4];

            if (!registers.TryGetValue(reg1, out int val1) || !registers.TryGetValue(reg2, out int val2))
                continue;

            registers[regRes]
                = operation switch
                {
                    "AND" => val1 & val2,
                    "OR" => val1 | val2,
                    "XOR" => val1 ^ val2,
                    _ => throw new NotImplementedException()
                };

            instructions.Remove(instruction);
        }

        return $"{Convert.ToInt64(
            string.Concat(
                registers.Where(kvp => kvp.Key[0] == 'z')
                    .OrderByDescending(kvp => kvp.Key)
                    .Select(kvp => kvp.Value)), 2)}";
    }

    public string SolvePart2(string input)
    {
        var sections = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var registersRaw = sections[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Split(": ")).ToList();
        var registers = registersRaw
            .ToDictionary(l => l[0], l => int.Parse(l[1]));
        var instructions = sections[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        var yIndex = registersRaw.FindIndex(x => x[0] == "y00");
        var intendedOutput =
            Convert.ToInt64(string.Concat(registersRaw[..yIndex].Select(r => r[1])), 2)
          + Convert.ToInt64(string.Concat(registersRaw[yIndex..].Select(r => r[1])), 2);

        for (int i = 0; instructions.Count > 0; i = i >= instructions.Count - 1 ? 0 : i + 1)
        {
            var instruction = instructions[i];
            var instr = instruction.Split(' ');
            var reg1 = instr[0];
            var reg2 = instr[2];
            var operation = instr[1];
            var regRes = instr[4];

            if (!registers.TryGetValue(reg1, out int val1) || !registers.TryGetValue(reg2, out int val2))
                continue;

            registers[regRes]
                = operation switch
                {
                    "AND" => val1 & val2,
                    "OR" => val1 | val2,
                    "XOR" => val1 ^ val2,
                    _ => throw new NotImplementedException()
                };

            instructions.Remove(instruction);
        }

        return $"{Convert.ToInt64(
            string.Concat(
                registers.Where(kvp => kvp.Key[0] == 'z')
                    .OrderByDescending(kvp => kvp.Key)
                    .Select(kvp => kvp.Value)), 2)}";
    }
}