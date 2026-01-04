# Known Limitations

## Max Proccessable Input

You cant build massive projects in this language. There are several limitations on the Input. In short these are:

- Max Chars per file: $2^{32} = 4.294.967.296$
- Max Lines per file: $2^{14} = 16.384$
- Max Columns per Line: $2^{10} = 1.024$
- Max used files: $2^{8} = 256$
  
This is because the internaly used Position struct is restricted to store all these values in a ulong. These values are then packed as follows:

| Index   | Line    | Column  | FileId |
| ------- | ------- | ------- | ------ |
| 32 bits | 14 bits | 10 bits | 8 bits |