﻿using AddedContent.Firebreak;
using System;
using System.Collections.Generic;

namespace DuckGame.ConsoleEngine.TypeInterpreters
{
    public static partial class TypeInterpreters
    {
        [Marker.DSHTypeInterpreterAttribute]
        public class Int32Interpreter : ITypeInterpreter
        {
            public Type ParsingType { get; } = typeof(int);

            public ValueOrException<object> ParseString(string fromString, Type specificType, CommandRunner engine)
            {
                return int.TryParse(fromString, out var val)
                    ? val
                    : new Exception($"Unable to parse to int: {fromString}");
            }

            public IList<string> Options(string fromString, Type specificType, CommandRunner engine)
            {
                return Array.Empty<string>();
            }
        }
    }
}