﻿using AddedContent.Firebreak;
using System.Linq;

namespace DuckGame.ConsoleEngine
{
    public static partial class Commands
    {
        [Marker.DevConsoleCommand(Description = "Creates a temporary command.", To = ImplementTo.DuckShell)]
        public static void Def(string name, string[] definedArgs, [CommandAutoCompl(false)] string command)
        {
            ShellCommand.Parameter[] parameters = definedArgs.Select(x => new ShellCommand.Parameter()
            {
                Name = x,
                ParameterType = typeof(string),
                IsOptional = true,
                DefaultValue = "",
            }).ToArray();

            // ensures no collision
            // will also overrite real commands if you want which is funny
            console.Shell.RemoveCommand(name);

            console.Shell.AddCommand(new ShellCommand(name, parameters, givenArgs =>
            {
                for (int i = 0; i < givenArgs.Length; i++)
                {
                    Set(parameters[i].Name, givenArgs[i]?.ToString() ?? "");
                }

                ValueOrException<object> result = console.Shell.Run(command);

                for (int i = 0; i < parameters.Length; i++)
                {
                    Pop(VariableRegister[parameters[i].Name]);
                }

                return result.Unpack();
            }), "User defined command");
        }
    }
}