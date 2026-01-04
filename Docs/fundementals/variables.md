# Variables

In **Ysharp**, variables are **immutable**.
This means once a variable is created, its value cannot be changed directly.
If you want a variable to hold a new value, you must create a new variable using the `VAR` keyword.

For example:

```ysharp
VAR x = 5
VAR x = 10   // this creates a new variable x with value 10
```

Even though it looks like you’re reassigning `x`, you’re actually creating a new version of `x` each time.



## Variable Types

Currently, YSharp supports **four main types of variables**:

1. **[String](./types/String.md)**

   * Text values wrapped in quotes.
   * Example:

     ```YSharp
     VAR name = "Alice"
     ```

2. **[Number](./types/Number.md)**

   * Whole numbers or decimals.
   * Example:

     ```YSharp
     VAR age = 25
     VAR pi = 3.14
     ```
4. **[Bool](./types/Bool.md)**

   * A 2 state variable
   * Example:

     ```YSharp
     VAR isVegan = TRUE
     VAR containsNuts = FALSE
     ```

3. **[DateTime](./types/DateTime.md)**

   * Represents dates and times.
   * Example:

     ```YSharp
     VAR now = TIME()
     ```

4. **[List](./types/List.md)**

   * A collection of values, which can be of the same or mixed types.
   * Example:

     ```YSharp
     VAR items = ["apple", "banana", "cherry"]
     VAR numbers = [1, 2, 3, 4, 5]
     ```


## Key Takeaways:

* Variables in YSharp never change after being created.
* To "update" a variable, you must create a new one with the same name.
* There are currently four data types: `String`, `Number`, `Bool`, `DateTime`, and `List`.

