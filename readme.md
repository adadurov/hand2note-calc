### Simple calculator in C#

This is a simple console app implementing a mathematical calculator with basic calculus operations (+, -, *, /) 
and the 'abs' function returning absolute value of its argument. Parentheses are also supported.

Example: for the following input

```
2*(2+3)
```

it is expected to produce 

```
10
```

### Implementation details

This solution uses .NET 5 and Visual Studio 2019.

The calculator is implemented based on Pratt parser with parselets for individual expressions. Numbers are processed as double precision floating-point numbers.

Therefore, division by zero produces Infinity instead of exceptions.

Exit code is always 0 (unless interrupted with Ctrl+C).

### Usage

```
  hand2note-calc.exe
```


#### Show usage summary
```
  hand2note-calc.exe
```

#### Testing

The solution includes the test project (based on NUnit). You will need a visual studio extension for NUnit to run the tests.

#### Error handling
1. A line starting with #ERROR is printed to the output (either STDOUT or the output file).
2. Error information is printed to STDERR with a reference to the problematic attempt number.
