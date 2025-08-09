- Add a Node for the var++ and var--
  - Update Parser to create this
  - Update Interpreter to execute it

- Reduce object instantiation
- Rework the dot notation in the parser
- Make ELIF work
  
## Implementation
- [ ] Implement call logic in `Internal/Interpreter.cs` (see //TODO comment)
- [ ] Implement execution methods for built-in functions in `Types/ValueTypes/FunctionTypes/BuiltInFunction.cs`
- [ ] Complete method in `Utility/BenchReportWriter.cs` that throws NotImplementedException

## Error Handling
- [ ] Add format exception handling in `Types/ValueTypes/DataTypes/Number.cs`

## Refactoring & Code Quality
- [ ] Refactor `Types/Node.cs` to use properties instead of fields
- [ ] Review and optimize token and position handling in `Types/InternalTypes/Token.cs` and `Types/InternalTypes/Position.cs`

## Testing
- [ ] Expand and automate tests in the `tests/` directory to cover all features and edge cases

## Documentation
- [ ] Improve inline documentation throughout the codebase
- [ ] Update main documentation to reflect recent changes and refactors
- [ ] Add usage examples and troubleshooting tips

## Cleanup
- [ ] Remove or refactor temporary code, workarounds, and deprecated logic
- [ ] Clean up commented-out or unused code

## Performance
- [ ] Review and optimize code marked as "optimized" for performance