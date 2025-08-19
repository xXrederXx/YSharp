## Implementation
- [ ] Implement call logic in `Internal/Interpreter.cs` (see //TODO comment)
- [ ] Implement execution methods for built-in functions in `Types/ValueTypes/FunctionTypes/BuiltInFunction.cs`
- [ ] Implement IMPORT logic
- [ ] Make Try Catch work
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

## Optimizing AST
- [ ] Create an Optimizer for the AST
- [ ] Add cli arg to toggle AST optimizer
- [ ] Implement Constant-folding
- [ ] Implement Dead-Code Elimination

## Add Functionalety
- [ ] Add more Builtins