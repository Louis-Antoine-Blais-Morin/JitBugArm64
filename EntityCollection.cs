// ==========================================================================
// Copyright (C) 2024 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Collections.Immutable;

namespace JitBugArm64
{
    #region Classes

    public class EntityCollection
    {
        #region Properties

        public IReadOnlyList<Entity> Entities { get; }

        #endregion

        #region Constructors

        public EntityCollection(IEnumerable<Entity> trackingObjects)
        {
            Entities = trackingObjects.ToImmutableList();
        }

        #endregion
    }

    #endregion
}