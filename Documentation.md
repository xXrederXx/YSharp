# Documentation Y-Sharp 2.0

- [Documentation Y-Sharp 2.0](#documentation-y-sharp-20)
  - [Syntax](#syntax)
  - [Data Types](#data-types)


---
## Syntax
- ### Variables
  The variables are **dynamically typed**. You can assign a variable like this.
  ```VAR x = 10``` Or ```VAR x: String = "I am a string"```
   First is the **VAR** keyword, and then the **identifier**. Optionally, you could write a type check. The interpreter ignores the type check. Then you write an **equals** and the **value** of the variable.
- ### Functions
  The functions can return a value. The value type can't be defined in the declaration of the function. A function can take as many arguments as you like. You can declare a function like this.
  ```
  FUN add(a, b)
    RETURN a + b
  END
  ```
  First, you write the **FUN** keyword, then the name of the function. After that, there are two parentheses, between them, you can write as many arguments as you like. Then you can write your code for the function. At the end of the function, there must be the **END** keyword.
- ### Loops
  - ### For
    You can use the For loop to iterate over a specified range of numbers. You can do it like this.
    ```
    FOR x = 0 TO 10 THEN    
      PRINT(x)            
    END
    ```
    First is the **FOR** keyword, then you need to write a name for the variable and assign a starting value to it. Then you write the **TO** keyword and the end value. After that, you write the **THEN** keyword. Now you can write your code for the for block. At the end, you write the **END** keyword.
    Optionally, you can add a step value like this.
    ```
    FOR x = 0 TO 20 STEP 2 THEN
      PRINT(x)
    END
    ```
    You just need to add the **STEP** keyword after the end value, and after the STEP keyword, you write your step amount.
  - #### While
    You can use while loops to do something until a condition is false. You can make a while loop like this.
    ```
    VAR x = "cool"
    WHILE x == "cool" THEN
      PRINT("x is cool")
      IF INPUT() == "bad" THEN
        VAR x = "bad"
      END
    END
    ```
    First you write the **WHILE** keyword, then the condition follows. After the condition is the **THEN** keyword. Now you can write some code inside the while loop. At the end of the loop is the **END** keyword.

---

## Data Types
- ### Number
   You can use numbers for everything. They support every operation and comparison. The numbers can be whole numbers or decimals; it doesn't matter. The numbers are internally stored as a C# double, which is stored in **8 bytes**. The minimum value is ±5.0 × 10^−324^ to ±1.7 × 10^308^ according to the Microsoft documentation.
- ### String
- ### Boolean
- ### List
- ### Date
---