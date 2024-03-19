// ==========================================================================
// Copyright (C) 2024 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JitBugArm64
{
    #region Classes

    /// <summary>
    /// Program reproducing what seems to be a problem in compiler/JIT
    /// when running under Arm 64 with .NET Runtime 8.0.3.
    /// </summary>
    internal class Program
    {
        #region Public Methods

        public static void Main()
        {
            Console.WriteLine("Program reproducing what seems to be a problem in compiler/JIT");
            Console.WriteLine("when running under Arm 64 with .NET Runtime 8.0.3.");

            using ILoggerFactory loggerFactory =
                LoggerFactory.Create(builder =>
                    builder.AddSimpleConsole(options =>
                    {
                        options.IncludeScopes = true;
                        options.SingleLine = true;
                        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss.ffffff ";
                    }));

            Settings settings = new();

            ItemSelectorCollection selectorCollection = new(
                Options.Create(settings),
                loggerFactory.CreateLogger("Dummy")
            );

            // Simulate typical scenario,
            // Creating objects, updating them, ending them.

            List<Guid> ids = new();
            Random random = new(0);

            while (!Console.KeyAvailable)
            {
                while (random.NextDouble() < 0.2)
                {
                    var id = Guid.NewGuid();
                    ids.Add(id);

                    selectorCollection.Add(id);
                }

                while (random.NextDouble() < 0.5)
                {
                    int objectCount = Math.Min(ids.Count, random.Next(5));
                    var entities = new Entity[objectCount];

                    for (var j = 0; j < entities.Length; j++)
                    {
                        Guid id = random.NextDouble() < 0.5 ? Guid.Empty : ids[random.Next(ids.Count)];

                        entities[j] = CreateEntity(random, id);
                    }

                    EntityCollection entityCollection = new(entities);
                    selectorCollection.Update(entityCollection);
                }

                while (random.NextDouble() < 0.22 && ids.Count > 0)
                {
                    int index = random.Next(ids.Count);
                    Guid id = ids[index];
                    ids.RemoveAt(index);

                    selectorCollection.End(id);
                }
            }
        }

        #endregion

        #region Private Methods

        private static Entity CreateEntity(Random random, Guid trackId)
        {
            object? testObject =
                random.NextDouble() < 0.5
                    ? null
                    : new object();

            var entity = new Entity
            {
                Score = (float)random.NextDouble(),
                TestObject = testObject,
                Id = trackId,
            };

            return entity;
        }

        #endregion
    }

    #endregion
}