﻿using System;
using System.Collections.Generic;
using DuckGame;

namespace AddedContent.Hyeve
{
    public static class FixedCommandHistory
    {
        [AutoConfigField(External = "CommandHistory")]
        public static List<string> SavedCommandHistory
        {
            get => DevConsole.core.previousLines.FastTakeFromEnd(25);
            set
            {
                DevConsole.core.previousLines.AddRange(value);
                DevConsole.core.lastCommandIndex = value.Count - 1;
            }
        }

        private static List<string> FastTakeFromEnd(this IReadOnlyList<string> list, int limit)
        {
            int smartLimit = Math.Min(list.Count, limit);
            List<string> result = new(smartLimit);

            for (int i = list.Count - smartLimit; i < list.Count; i++)
            {
                result.Add(list[i]);
            }

            return result;
        }
    }
}