using Hand2Note.Calc.Exceptions;
using System;
using System.IO;

namespace Hand2Note.Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                InteractiveWelcome();
                Process(Console.In, Console.Out);
            }
            else if (args.Length == 1)
            {
                Usage();
            }
            else
            {
                Console.Error.WriteLine("Error -- too many arguments!");
                Usage();
            }
        }

        private static void Process(TextReader reader, TextWriter writer)
        {
            var c = 0;
            string line;
            do
            {
                ++c;
                line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) break;

                try
                {
                    var calc = new CalculatorMvp();
                    writer.WriteLine(calc.Calculate(line));
                }
                catch (SyntaxErrorException se_ex)
                {
                    Console.Error.WriteLine($"  Error in line {c}: " + se_ex.Message);
                    Console.Error.WriteLine("  " + line);
                    Console.Error.WriteLine("  " + new string('-', se_ex.Location) + "^");
                    // not a fatal error, continue with the next line
                    writer.WriteLine($"#ERROR parsing line {c}");
                }

            } while (true);
        }

        static void InteractiveWelcome()
        {
            // FIXME: the message should be in a resource file

            Console.WriteLine(
@"Welcome to this very simple calculator!

Enter an expression and hit Enter when done.
Enter en empty string to quit.
Invoke with --help for more details.
");
        }

        static void Usage()
        {
            // FIXME: the message should be in a resource file
            Console.WriteLine(
@"Usage:

This is a simple console app implementing a mathematical calculator with 
basic calculus operations (+, -, *, /) and the 'abs' function returning absolute
value of its argument.Parentheses are also supported.

Example: for the following input

```
2 * (2 + 3.5)
```

it is expected to produce 

```
11
```

as the output.

");
        }


    }
}
