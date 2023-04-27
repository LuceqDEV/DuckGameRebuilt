﻿using DuckGame.ConsoleEngine;
using System;

namespace DuckGame.ConsoleEngine.TypeInterpreters
{
    public class UInt64Interpreter : ITypeInterpreter
    {
        public Type ParsingType { get; } = typeof(ulong);
        public ValueOrException<object> ParseString(string fromString, Type specificType, CommandRunner engine)
        {
            return ulong.TryParse(fromString, out var val)
                ? val 
                : new Exception($"Unable to parse to ulong: {fromString}"); 
        }
    }
}