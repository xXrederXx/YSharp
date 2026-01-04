## Built-in Variables

- **TRUE**: Boolean value representing true.
- **FALSE**: Boolean value representing false.
- **MATH**: Provides mathematical functions and constants (see separate documentation for available math operations).

## Built-in Functions

### PRINT(value)
Prints the given value to the console.  
- **Usage:** `PRINT("Hello World")`
- **Arguments:** `value` (any type)
- **Returns:** `null`

### INPUT()
Prompts the user for input and returns the entered string.  
- **Usage:** `INPUT()`
- **Arguments:** none
- **Returns:** string

### RUN(fileName)
Executes a YSharp script from the specified file.  
- **Usage:** `RUN("script.ys")`
- **Arguments:** `fileName` (string, path to script)
- **Returns:** `null` (errors are printed to the console)

### TIMETORUN(fileName)
Measures and returns the execution time of a script, broken down by interpreter phases.  
- **Usage:** `TIMETORUN("script.ys")`
- **Arguments:** `fileName` (string, path to script)
- **Returns:** string with timing information for each phase:
  - Init Lexer
  - Create Tokens
  - Init Parser
  - Create AST
  - Init Context
  - Run Interpreter
  - Whole Time (ms)

### TIME()
Returns the current system time as a date-time value.  
- **Usage:** `TIME()`
- **Arguments:** none
- **Returns:** date-time object