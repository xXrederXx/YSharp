# Main Documentation Y-Sharp 2.0

1. [Syntax](#syntax)
   1. [Operations](#operations)
   1. [Comparisons](#Comparisons)
   2. [Variables](#variables)
   3. [Functions](#functions)
   4. [Loops](#loops)
2. [Data Types](#data-types)
   1. [Numbers](#number)
   2. [Strings](#string)
   3. [Boolean](#boolean)
   4. [List](#list)
   5. [Date](#date)

---
## Syntax
- ### Operations
    You can use different Operations depending on the data types. Here you can see which data types support which Operations. If there need to be 2 parts of data it is assumed, that they are of the same type. For some operations you will find a description in the section about the data taype.
    |-------      |Number|String|Boolian|List|Date|
    |:---:        |:---:|:---:|:---:|:---:|:---:|
    |+ & += & ++  |&#9989;|&#9989;|&#10060;|&#9989;|&#9989;|
    |- & -= & --  |&#9989;|&#10060;|&#10060;|&#10060;|&#9989;|
    |* & *=       |&#9989;|&#9989;*|&#10060;|&#9989;*|&#10060;|
    |/ & /=       |&#9989;|&#10060;|&#10060;|&#10060;|&#10060;|
    |^            |&#9989;|&#10060;|&#10060;|&#10060;|&#10060;|
    |AND          |&#10060;|&#10060;|&#9989;|&#10060;|&#10060;|
    |OR           |&#10060;|&#10060;|&#9989;|&#10060;|&#10060;|
    |NOT          |&#10060;|&#10060;|&#9989;|&#10060;|&#10060;|

    .* Only with a Number


- ### Comparisons
    You can use different Comparisons depending on the data types. Here you can see which data types support which Comparisons. If there need to be 2 parts of data it is assumed, that they are of the same type. For some comparisons you will find a description in the section about the data taype.
    |-------|Number|String|Boolian|List|Date|
    |:---:|:---:|:---:|:---:|:---:|:---:|
    |==|&#9989;|&#9989;|&#9989;|&#9989;|&#10060;|
    |!=|&#9989;|&#9989;|&#9989;|&#9989;|&#10060;|
    |<|&#9989;|&#10060;|&#10060;|&#10060;|&#10060;|
    |<=|&#9989;|&#10060;|&#10060;|&#10060;|&#10060;|
    |>|&#9989;|&#10060;|&#10060;|&#10060;|&#10060;|
    |>=|&#9989;|&#10060;|&#10060;|&#10060;|&#10060;|

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
  - **For loop**
    You can use the For loop to iterate over a specified range of numbers. You  can do it like this.
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
    
    <br>
  
  - **While loop**
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
  You can use numbers for everything. They support every operation and comparison. The numbers can be whole numbers or decimals; it doesn't matter. The numbers are internally stored as a C# double, which is stored in **8 bytes**. The minimum value is ±5.0 × $10^{−324}$ to $±1.7 × 10^{308}$ according to the Microsoft documentation.

  <br>

  | Methods | Description |
  |---|---|
  | ToString()  | Convert a number to a string  |
  | ToString(string format)| Convert a number to a string with a specified format|
  | ToBool()  | Converts a number to a bool. Rturns True if the number is not 0.|

  <br>

  | Variables | Description |
  |---|---|
  |   |   |

- ### String
  You can use strings for doing things with text. Strings support adding and multiplication, and for comparison they can compare if they are (not) equal. To create a string use ""

  <br>

  | Methods | Description |
  |---|---|
  | ToNumber()  | This returns the string parsed to a number. If it wasn't able to do this it will throw an error  |
  | ToBool()  | This will return true if the string is not empty  |

  <br>

  | Variables | Description |
  |---|---|
  | Length | This will return the legth of the string  |


- ### Boolean
  Booleans can have the value true or false. They can be compared against each other if the are (not) equal. They also support some other Comparisons such as AND, OR and NOT

  <br>

  | Methods | Description |
  |---|---|
  |   |   |

  <br>

  | Variables | Description |
  |---|---|
  |   |   |

- ### List
  A list can be used to store multiple items at once. You can use multiple datatypes in one list. Lists can be added to and multiplyed. They also support comparing if they are (not) equal. To create a list use []

  <br>

  | Methods | Description |
  |---|---|
  | Add(Value item1, Value item2 ...)  | Adds as many items to a list as given for the arguments  |
  | Get(Number index)  | Returns the item at the index of the argument  |
  | Remove(Number index)  | Removes the item at the specified index from the list  |
  | IndexOf(Value value)  | Searches the list for the specified value and returns the index if found. If not found it returns -1  |


  <br>

  | Variables | Description |
  |---|---|
  | Count  | Returns the number of elements the list currently contains  |

- ### Date
  You can create a date value with the TIME() function, this wil just be the current time and date. You can only add times and substract them.

  <br>

  | Methods | Description |
  |---|---|
  |   |   |


  <br>

  | Variables | Description |
  |---|---|
  | Microsecond  | returns the Microsecond as a Number|
  | Millisecond  | returns the Millisecond as a Number|
  | Second  | returns the Second as a Number|
  | Minute  | returns the Minute as a Number|
  | Hour  | returns the Hour (in a 24 hour based format) as a Number|
  | DayOfMonth  | returns the day of the month as a Number|
  | DayOfWeek  | returns the day name as a String (in english)|
  | DayOfYear  | returns the day of the year as a Number|
  | Month  | returns the Month as a Number|
  | Year  | returns the Year as a Number|


---