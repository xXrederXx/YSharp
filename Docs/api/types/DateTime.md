# Type: `DateTime`

## Overview

This datatype stores the date and the time. This can be usefull to Timestamp things. 

## Syntax

```ysharpe
VAR myVar = TIME()
```

## Properties

| Property      | Type     | Description                          |
| ------------- | -------- | ------------------------------------ |
| `Microsecond` | `Number` | Returns the Microseconds stored      |
| `Millisecond` | `Number` | Returns the Milliseconds stored      |
| `Second`      | `Number` | Returns the Seconds stored           |
| `Minute`      | `Number` | Returns the Minutes stored           |
| `Hour`        | `Number` | Returns the Hours stored             |
| `DayOfMonth`  | `Number` | Returns the Day of  the Month stored |
| `DayOfWeek`   | `String` | Returns the Day of the Week stored   |
| `DayOfYear`   | `Number` | Returns the Day of the year stored   |
| `Month`       | `Number` | Returns the Month stored             |
| `Year`        | `Number` | Returns the Year stored              |


## Functions

| Function | Parameters | Returns | Description |
| -------- | ---------- | ------- | ----------- |
| `-`      | `-`        | `-`     | -           |


## Notes

* You can currently only create a date time by using `TIME()`


