﻿using DuckGame.ConsoleEngine;
using System;

namespace DuckGame.ConsoleEngine.TypeInterpreters
{
    public class EnumInterpreter : ITypeInterpreter
    {
        public Type ParsingType { get; } = typeof(Enum);
        public ValueOrException<object> ParseString(string fromString, Type specificType, CommandRunner engine)
        {
            try
            {
                return Enum.Parse(specificType, fromString, true);
            }
            catch (Exception e)
            {
                return new Exception($"Unable to parse to {specificType.Name}: {fromString}", e);
            }
        }
    }
}