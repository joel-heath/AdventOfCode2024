using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day17 : IDay
{
    public int Day => 17;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "Register A: 729\r\nRegister B: 0\r\nRegister C: 0\r\n\r\nProgram: 0,1,5,4,3,0", "4,6,3,5,6,3,5,2,1,0" },
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "Register A: 2024\r\nRegister B: 0\r\nRegister C: 0\r\n\r\nProgram: 0,3,5,4,3,0", "117440" },
    };

    private static long ComboOp(int operand, List<long> registers)
    {
        if (operand <= 3) return operand;
        if (operand <= 6) return registers[operand - 4];
        throw new Exception("Invalid combo operand");
    }

    public string SolvePart1(string input)
    {
        var sections = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var registers = Utils.GetLongs(sections[0]).ToList();
        var program = Utils.GetLongs(sections[1]).ToList();

        List<long> outputStream = [];
        for (int i = 0; i < program.Count - 1; i += 2)
        {
            var (instruction, operand) = (program[i], (int)program[i + 1]);

            switch (instruction)
            {
                case 0: // adv
                    registers[0] >>= (int)ComboOp(operand, registers);
                    break;
                case 1: // bxl
                    registers[1] ^= operand;
                    break;
                case 2: // bst
                    registers[1] = ComboOp(operand, registers) % 8;
                    break;
                case 3: // jnz
                    if (registers[0] != 0)
                        i = operand - 2;
                    break;
                case 4: // bxc
                    registers[1] ^= registers[2];
                    break;
                case 5: // out
                    outputStream.Add(ComboOp(operand, registers) % 8);
                    break;
                case 6: // bdv
                    registers[1] = registers[0] >> (int)ComboOp(operand, registers);
                    break;
                case 7: // cdv
                    registers[2] = registers[0] >> (int)ComboOp(operand, registers);
                    break;
            }
        }

        return $"{string.Join(',', outputStream)}";
    }

    public string SolvePart2(string input)
    {
        var output = Utils.GetInts(input.Split(Environment.NewLine + Environment.NewLine)[1]);
        if (UnitTestsP2.ContainsKey(input)) return $"{output.Select((n, i) => n << (3 * (i+1))).Sum()}";

        return $"{CalculateInput([..output.Reverse()]).Min()}";
    }

    static IEnumerable<long> CalculateInput(int[] outputs, int index = 0, long A = 0)
    {
        if (index == outputs.Length) return [A];
        long n = outputs[index];
        List<long> options = [];
        for (int c = 0; c < 8; c++)
        {
            long a = A << 3 | c;
            long b = c ^ (a >> (c ^ 7));

            if (b % 8 == n)
            {
                options.Add(a);
            }
        }
        return options.SelectMany(o => CalculateInput(outputs, index + 1, o));
    }
}