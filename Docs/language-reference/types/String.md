# Type: `String`

## Overview

A string stores a series of charachters.

## Syntax

```ysharpe
VAR myVar = "Hello World"
```

## Properties

| Property | Type     | Description                      |
| -------- | -------- | -------------------------------- |
| `Length` | `Number` | Returns the Length of the string |

## Functions


| Function   | Parameters | Returns  | Description                                                        |
| ---------- | ---------- | -------- | ------------------------------------------------------------------ |
| `ToNumber` | `-`        | `Number` | Tryes to convert the string to a number. Throws if not succsessful |
| `ToBool`   | `-`        | `Bool`   | Returns a bool based of if the string is not empty                 |
| `ToUpper`   | `-`        | `-`   | Returns a new string with all characters changed to Uppercase|
| `ToLower`   | `-`        | `-`   | Returns a new string with all characters changed to Lowercase|
| `Split`   | `splitString:String`        | `List`   | Splits the string when encountering the `splitString` and returns a list containing one or multiple strings, depending on how many times `splitString` has ben found|
