# Troubleshoot Documentation Y-Sharp 2.0

1. [Errors](#errors)
   1. [YS01xx](#ys01xx)
   1. [YS02xx](#ys02xx)
   1. [YS03xx](#ys03xx)
   1. [YS04xx](#ys04xx)
2. [Known Issues](#known-issues)

# Errors
  - ### YS01xx 
    The YS01xx errors are errors that occur on a lexer basis. This means you likely have a non-valid character somewhere in your code. The lexer also counts the number of parentheses and square brackets. If there are more Open or closed ones, it will throw an error.
    <br>
    
    - **YS0101 - Unclosed Brackets Error**<br>
        This error indicates that the lexer found a difference in the number of open and closed brackets or parentheses. Try looking for a non-closing parenthesis or square bracket.
    <br>
    
    - **YS0110 - Illegal Char Error**<br>
        This error indicates that you have a character in your code that is not allowed. I.e., the Lexer can't process emojis.
    <br>
    
    - **YS0111 - Expected Char Error**<br>
        This error indicates that there needs to be a specific character somewhere, which isn't in your code. Try adding the character with the error specified at the right position.

  - ### YS02xx
    The YS02xx errors occur on the parser level. It is likely that a keyword is written the wrong way or is missing. 
    <br>
    
    - **YS0220 - Invalid Syntax Error**<br>
        Your Syntax is invalid at some point in your code. This is a very generic error and could be a lot of things. Try searching for missing or false keywords. 
    <br>
    
    - **YS0221 - Expected Keyword Error**<br>
        This is a more specific version of the YS0220 error. This specifies that the parser expects a specific keyword at a specific position in your code. I.e. you missed the THEN keyword after starting a WHILE loop.
    <br>
    
    - **YS0222 - Expected Token Error**<br>
        This is a more specific version of the YS0220 error. It indicates that you missed a specific token. This will likely be a single character, but it could be a series of characters too.

  - ### YS03xx
    The YS03xx errors occur in the interpreter. The error could come from many places. I.e., it could be that you didn't assign a variable or call a function with too many arguments.
    <br>
    
    - **YS0301 - Var Not Found Error**<br>
        This error indicates that you tried to use a variable that is not defined. Keep in mind that you will need to assign a variable above its use case. Just assign the variable, or maybe check the spelling.
    <br>
    
    - **YS0302 - Func Not Found Error**<br>
        This error indicates that you tried to use a function that was not defined. Keep in mind that you will need to assign a function above its use cases. Just create the function, or maybe check the spelling.
    <br>
    
    - **YS0304 - Wrong Format Error**<br>
        This error indicates that you passed in a value with a data type that is not compatible. This error can only be activated in built-in functions because user-defined functions currently can't define an argument data type.
    <br>
    
    - **YS0305 - Num Args Error**<br>
        This error indicates that you either passed too many or too fewer arguments into a function. Keep in mind that method overloading will not work. Try counting the number of arguments needed and the number you passed, and just one thing.
    <br>
    
    - **YS006 - Arg Out Of Range Error**<br>
        This error indicates that you are trying to access a part of a list that is out of range. Try checking the length of the list before accessing it.
    <br>
    
    - **YS0310 - Run Time Error**<br>
        This is an error that occurs at runtime. This is very generic, but it will generate a traceback for you. Try going the way of the traceback and finding the error.
    <br>
    
    - **YS0311 - Illigal Operation Error**<br>
        This is a more specific version of the YS0310 error. This error indicates that you have an illegal operation somewhere in your code. Try finding the operation and changing it to something valid. You can see in the documentation which operations are valid.

  - ### YS04xx
    These are special errors that occur in specific portions of the code.
    <br>
    
    - **YS0400 - Internal Error**<br>
        This is an internal error. Something in the process of processing your code went wrong. You probably can't fix this error. But you can try to restart the Interpreter and your PC.

# Known Issues
