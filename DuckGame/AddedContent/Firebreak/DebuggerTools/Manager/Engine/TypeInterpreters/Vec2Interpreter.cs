﻿using DuckGame.ConsoleEngine;
using System;

namespace DuckGame.ConsoleEngine.TypeInterpreters
{
    public class Vec2Interpreter : ITypeInterpreter
    {
        public Type ParsingType { get; } = typeof(Vec2);
        public ValueOrException<object> ParseString(string fromString, Type specificType, CommandRunner engine)
        {
            return Vec2.TryParse(fromString, out var val)
                ? val 
                : new Exception($"Unable to parse to Vec2: {fromString}"); 
        }
    }
}