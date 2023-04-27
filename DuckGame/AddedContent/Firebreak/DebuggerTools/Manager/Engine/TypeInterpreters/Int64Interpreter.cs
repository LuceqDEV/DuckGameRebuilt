﻿using DuckGame.ConsoleEngine;
using System;

namespace DuckGame.ConsoleEngine.TypeInterpreters
{
    public class Int64Interpreter : ITypeInterpreter
    {
        public Type ParsingType { get; } = typeof(long);
        public ValueOrException<object> ParseString(string fromString, Type specificType, CommandRunner engine)
        {
            return long.TryParse(fromString, out var val)
                ? val 
                : new Exception($"Unable to parse to long: {fromString}"); 
        }
    }
}