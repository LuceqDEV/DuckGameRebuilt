﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DuckGame.ConsoleEngine
{
    public class CommandRunner
    {
        public readonly List<DSHCommand> Commands = new();
        public readonly List<ITypeInterpreter> TypeInterpreterModules = new();
        public Dictionary<Type, ITypeInterpreter> TypeInterpreterModulesMap = new();
        public Regex ValidNameRegex { get; } = new("^[^ ]+$", RegexOptions.Compiled);

        public const string INLINE_COMMAND_MARKER = "±";

        public virtual ValueOrException<object?> Run(string input)
        {
            if (string.IsNullOrEmpty(input))
                return ValueOrException<object>.FromValue(null);

            string[] commandSegments;

            try
            {
                commandSegments = Tokenize(input);
            }
            catch (Exception e)
            {
                return e;
            }

            if (commandSegments.Length == 0)
                return ValueOrException<object>.FromValue(null);

            if (commandSegments.Contains(";"))
            {
                List<string>[] commandSequence = new List<string>[commandSegments.Count(x => x == ";") + 1];

                for (int i = 0; i < commandSequence.Length; i++)
                {
                    commandSequence[i] = new List<string>();
                }
            
                for (int i=0, j=0; i < commandSegments.Length; i++)
                {
                    string segment = commandSegments[i];
                
                    if (segment == ";")
                    {
                        j++;
                        continue;
                    }

                    commandSequence[j].Add(segment);
                }

                ValueOrException<string> accumulativeResult = "";
                foreach (List<string> cmd in commandSequence)
                {
                    if (cmd.Count == 0)
                        continue;
                    
                    ValueOrException<string[]> cmdParseResult = ParseCodeBlocks(cmd);
                
                    ValueOrException<object?> result = null!;
                
                    cmdParseResult.TryUse(
                        tokens => result = RunFromTokens(tokens),
                        exception => result = exception
                    );

                    ValueOrException<string> stringResult = result.Failed
                        ? result.Error
                        : result.Value?.ToString();

                    accumulativeResult.AppendResult(stringResult);
                }

                return accumulativeResult.Failed
                    ? accumulativeResult.Error
                    : accumulativeResult.Value;
            }

            ValueOrException<string[]> parseResult = ParseCodeBlocks(commandSegments);
        
            return parseResult.Failed 
                ? parseResult.Error 
                : RunFromTokens(parseResult.Value);
        }

        public virtual string[]? Predict(string partialCommand, int index)
        {
            if (index < 0 || index > partialCommand.Length)
                throw new ArgumentOutOfRangeException(nameof(index), "Caret index cannot be outside the range of the command");

            if (partialCommand.Length == 0)
                return null;

            partialCommand = partialCommand.Substring(0, index);

            string[] tokens = Tokenize(partialCommand);
            
            return PredictFromTokens(tokens);
        }

        protected virtual string[]? PredictFromTokens(string[] tokens)
        {
            return tokens;
        }

        protected virtual ValueOrException<object?> RunFromTokens(string[] tokens)
        {
            if (tokens.Length == 0)
                throw new Exception("in RunFromTokens(string[] tokens), tokens.Length == 0");
        
            string commandName = tokens[0];

            string[] commandArgs = new string[tokens.Length - 1];
            for (int i = 1; i < tokens.Length; i++)
            {
                commandArgs[i - 1] = tokens[i];
            }

            foreach (DSHCommand command in Commands)
            {
                if (!string.Equals(command.Name, commandName, StringComparison.CurrentCultureIgnoreCase))
                    continue;

                Command.Parameter[] parameterInfos = command.Command.Parameters;
                object?[] appliedParameters = new object?[parameterInfos.Length];

                for (int i = 0; i < appliedParameters.Length; i++)
                {
                    Command.Parameter parameterInfo = parameterInfos[i];
                    object? appliedParameterValue;
                    if (i >= commandArgs.Length && !parameterInfos.Last().IsParams)
                    {
                        if (parameterInfo.IsOptional)
                            appliedParameterValue = parameterInfo.DefaultValue;
                        else return new Exception($"Missing Argument: {parameterInfo.Name}");
                    }
                    else
                    {
                        Type parseType = parameterInfo.ParameterType;
                        string argString;
                        ValueOrException<object?> parseResult = null;

                        if (i == appliedParameters.Length - 1
                            && (parameterInfo.IsParams || parseType == typeof(string)))
                        {
                            int length = commandArgs.Length - i;
                            string[] resultArray = new string[length];
                            Array.Copy(commandArgs, i, resultArray, 0, length);

                            if (parseType == typeof(string))
                                argString = string.Join(" ", resultArray);
                            else if (parameterInfo.IsParams)
                            {
                                if (length != 0)
                                {
                                    parseResult = ValueOrException<object>.FromValue(resultArray);
                                }
                                
                                argString = null;
                            }
                            else throw new InvalidOperationException();
                        }
                        else argString = commandArgs[i];

                        if (parseResult is null)
                        {
                            if (!TypeInterpreterModules.TryFirst(x => x.ParsingType.IsAssignableFrom(parseType),
                                    out ITypeInterpreter interpreter))
                                return new Exception($"No conversion module found: {parseType.Name}");

                            parseResult = interpreter.ParseString(argString, parseType, this);
                        }

                        if (parseResult.Failed)
                            return new Exception($"Parsing Error: {parseResult.Error.Message}");

                        appliedParameterValue = parseResult.Value;
                    }

                    appliedParameters[i] = appliedParameterValue;
                }

                ValueOrException<object?> result;
                try
                {
                    object? invokationValue = command.Command.Invoke(appliedParameters);

                    result = ValueOrException<object?>.FromValue(invokationValue);
                }
                catch (TargetInvocationException e)
                {
                    return e.InnerException ?? e;
                }
                catch (Exception e)
                {
                    result = ValueOrException<object>.FromError(e);
                }

                return result;
            }

            return new Exception($"Command not found: {commandName}");
        }

        private ValueOrException<string[]> ParseCodeBlocks(IReadOnlyList<string> commandSegments)
        {
            List<string> segments = new();
            foreach (string segment in commandSegments)
            {
                if (!segment.StartsWith(INLINE_COMMAND_MARKER))
                {
                    segments.Add(segment);
                    continue;
                }

                ValueOrException<object?> runResult = Run(segment.Substring(INLINE_COMMAND_MARKER.Length));
                runResult.TryUse(output =>
                {
                    segments.Add(output?.ToString() ?? "");
                }, _ => { });

                if (runResult.Failed)
                    return runResult.Error;
            }

            return segments.ToArray();
        }

        private static string[] Tokenize(string input)
        {
            List<string> split = new();

            StringBuilder currentSegment = new();
            bool treatNextAsDefault = false;

            bool awaitCloseCodeBlock = false;
            int ignoreCloseCodeBlocks = 0;

            bool awaitCloseSquareBracket = false;
            int ignoreCloseSquareBrackets = 0;

            bool awaitCommentEnd = false;

            bool addEmpty = false;

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (treatNextAsDefault)
                {
                    currentSegment.Append(c);
                    treatNextAsDefault = false;
                    continue;
                }
            
                switch (c)
                {
                    case '\\':
                        if (awaitCloseCodeBlock || awaitCommentEnd)
                            goto default;
                        
                        treatNextAsDefault = true;
                        continue;
                    
                    case '#':
                        if (awaitCloseCodeBlock || awaitCloseSquareBracket)
                            goto default;

                        awaitCommentEnd ^= true;

                        // if (awaitCommentEnd)
                        // {
                        //     ignoreCommentEnds++;
                        //     goto default;
                        // }
                        continue;
                
                    case '[':
                        if (awaitCloseCodeBlock || awaitCommentEnd)
                            goto default;
                    
                        if (awaitCloseSquareBracket)
                        {
                            ignoreCloseSquareBrackets++;
                            goto default;
                        }

                        awaitCloseSquareBracket = true;
                        continue;
                    case ']':
                        if (awaitCloseCodeBlock || awaitCommentEnd)
                            goto default;
                    
                        if (awaitCloseSquareBracket)
                        {
                            if (ignoreCloseSquareBrackets > 0)
                            {
                                ignoreCloseSquareBrackets--;
                                goto default;
                            }
                            else
                            {
                                awaitCloseSquareBracket = false;

                                if (input[i - 1] == '[')
                                    addEmpty = true;
                            }
                        }
                        continue;
                
                    case '{':
                        if (awaitCloseSquareBracket || awaitCommentEnd)
                            goto default;

                        if (awaitCloseCodeBlock)
                        {
                            ignoreCloseCodeBlocks++;
                            goto default;
                        }

                        awaitCloseCodeBlock = true;
                        currentSegment.Append(INLINE_COMMAND_MARKER);
                        break;
                    case '}':
                        if (awaitCloseSquareBracket || awaitCommentEnd)
                            goto default;

                        if (awaitCloseCodeBlock)
                        {
                            if (ignoreCloseCodeBlocks > 0)
                            {
                                ignoreCloseCodeBlocks--;
                                goto default;
                            }
                            else
                            {
                                awaitCloseCodeBlock = false;
                            }
                        }
                        break;

                    case '\n':
                    case '\r':
                        goto case ' ';

                    case ' ':
                        if (awaitCloseSquareBracket || awaitCloseCodeBlock || awaitCommentEnd)
                            goto default;
                    
                        if (currentSegment.Length > 0)
                            split.Add(currentSegment.ToString());
                        else if (addEmpty)
                        {
                            split.Add(string.Empty);
                            addEmpty = false;
                        }
                    
                        currentSegment.Clear();
                        break;
                
                    default:
                        if (awaitCommentEnd)
                            continue;
                        
                        currentSegment.Append(c);
                        break;
                }
            }
        
            if (currentSegment.Length > 0)
                split.Add(currentSegment.ToString());
            else if (addEmpty) 
                split.Add(string.Empty);

            return split.ToArray();
        }
    
        /// <param name="allClasses">The pool of classes to search from</param>
        public void AddTypeInterpretters(IEnumerable<TypeInfo>? allClasses = default)
        {
            allClasses ??= (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly())
                .DefinedTypes;

            foreach (TypeInfo typeInfo in allClasses)
            {
                if (!typeof(ITypeInterpreter).IsAssignableFrom(typeInfo) // isn't a type interpreter
                    || typeInfo.AsType() == typeof(ITypeInterpreter))    // or is ITypeInterpreter itself
                    continue;

                ITypeInterpreter interpreterInstance = (ITypeInterpreter) Activator.CreateInstance(typeInfo)!;
            
                TypeInterpreterModules.Add(interpreterInstance);
                TypeInterpreterModulesMap.Add(interpreterInstance.ParsingType, interpreterInstance);
            }
        }
    
        /// <param name="allMethods">The pool of methods to search from</param>
        public void AddCommandsUsingAttribute(IEnumerable<MethodInfo>? allMethods = default)
        {
            allMethods ??= (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly())
                .DefinedTypes
                .SelectMany(x => x.DeclaredMethods);

            foreach (MethodInfo methodInfo in allMethods)
            {
                if (methodInfo.GetCustomAttribute<DSHCommand>() is not { } attr)
                    continue; // not using attribute
            
                if (methodInfo.GetParameters().Any(x => x.ParameterType == typeof(object)))
                    throw new Exception("Imprecise type [Object] is invalid. Use [String] instead");
            
                attr.Name ??= methodInfo.Name;
                attr.Command = Command.FromMethodInfo(methodInfo);

                AddCommand(attr);
            }
        }

        /// <param name="command" />
        /// <param name="description">Describes the usage of this command</param>
        /// <param name="hidden">
        /// Whether or not this command is marked as "hidden".
        /// Doesn't matter by default, but can be used by your implementation
        /// </param>
        public void AddCommand(Command command, string? description = null, bool hidden = false)
        {
            AddCommand(new DSHCommand()
            {
                Name = command.Name,
                Hidden = hidden,
                Description = description,
                Command = command
            });
        }

        public void RemoveCommand(string commandName)
        {
            Commands.RemoveAll(x => string.Equals(x.Name, commandName, StringComparison.InvariantCultureIgnoreCase));
        }

        private void AddCommand(DSHCommand command)
        {
            if (!ValidNameRegex.IsMatch(command.Name))
                throw new Exception($"Invalid command name: {command.Name}");

            if (Commands.Any(x =>
                    x.Name == command.Name &&
                    x.Command.Parameters.SequenceEqual(command.Command.Parameters)))
                throw new Exception($"Duplicate command signature: {command.Name}");

            Commands.Add(command);
        }
    }
}