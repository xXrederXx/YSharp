# Trubbleshot Documentation Y-Sharp 2.0

1. [Errors](#errors)
   1. [YS01xx](#ys01xx)
   1. [YS02xx](#ys02xx)
   1. [YS03xx](#ys03xx)
   1. [YS04xx](#ys04xx)
2. [Known Issues](#known-issues)

# Errors
  - ### YS01xx
    The YS01xx errors are errors which occur on the lexer basis. This means you likley have a non valide charachter somewhere in your code. The lexer also counts the number of parethesies and square brackets. If there are more Open or closed ones it will throw an error.
    <br>
    - **YS0101 - Unclosed Brackets Error**
        This error indicates that the lexer found a difference in the number of open and closed brackets or parethesies. Try looking for a non closing parenthesie or squarebracket.
    <br>
    - **YS0110 - Illegal Char Error**
        This error indicates that you have a charachter in your code which is not allowed. I. e. the lexer cant process emojis.
    <br>
    - **YS0111 - Expected Char Error**
        This error indicates that there needs to be a spesific charachter somewhere, which isnt in your code. Try adding the charachter which error specified at the right position.

  - ### YS02xx
    The YS02xx errors occur on the parser level. It is likly that a keyword is written the wrong way or is missing. 
    <br>
    - **YS0220 - Invalid Syntax Error**
        Your Syntax is invalid in some point of your code. This is a very generic error and could be a lot of things. Try searching for missing or false keywors. 
    <br>
    - **YS0221 - Expected Keyword Error**
        This is a more spesific version of the YS0220 error. This specifies that the parser expects a spesific keyword at a specific position in your code. I. e. you missed the THEN keyword after starting a WHILE loop.
    <br>
    - **YS0222 - Expected Token Error**
        This is a more spesific version of the YS0220 error. This specifies that you missed a spesific token. This will likly be a single charachter but it could be a series of charachters too.

  - ### YS03xx
    The YS03xx errors occur in the interpreter. The error could come from many places. I. e. it could be that you didnt assign a variable or call a function with too many arguments.
    <br>
    - **YS0301 - Var Not Found Error**
        This error indicates that you try to use a variable which is not defined. Keep in mind that you will need to assing a variable above its usecases. Just assign the variable or maybe check the spelling.
    <br>
    - **YS0302 - Func Not Found Error**
        This error indicates that you try to use a function which is not defined. Keep in mind that you will need to assing a function above its usecases. Just create the function or maybe check the spelling.
    <br>
    - **YS0304 - Wrong Format Error**
        This error indicates that you pass in a value with a data type which is not compatible. This error can only be acctivated in builtin functions, because user defined functions currently cant define an argument data type.
    <br>
    - **YS0305 - Num Args Error**
        This error indicates that you either passed too many or too less arguments into a function. Keep in mynd that method-overloading will not work. Try counting the number of arguments needed and the number you passed and ajust one thing.
    <br>
    - **YS006 - Arg Out Of Range Error**
        This error indicates that you are trying to access a part of a list which is out of range. Try checking the length of the list before accessing it.
    <br>
    - **YS0310 - Run Time Error**
        This is an error which occurs at runtime. This is very generic, but it will generate a traceback for you. Try going the way of the tracback and finding the error.
    <br>
    - **YS0311 - Illigal Operation Error**
        This is a more specific version of the YS0310 error. This error indicates that you have an illigal operation somewhere in your code. Try finding the operation and change it to something valid. You can look in the documentation which operations are valide.

  - ### YS04xx
    These are special errors, which occur in spesific portions of the code.
    <br>
    - **YS0400 - Internal Error**
        This is an internal error. Something in the process of processing your code went wrong. You probably cant fix this error. But you can try to restart the Interpreter and your pc.

# Known Issues