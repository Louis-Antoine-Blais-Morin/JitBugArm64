// ==========================================================================
// Copyright (C) 2024 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

namespace JitBugArm64
{
    #region Classes

    public class Entity
    {
        #region Properties

        public Guid Id { get; set; }

        public float Score { get; set; }

        /// <summary>
        /// Useful to reproduce problem: an object that will be tested for nullity.
        /// </summary>
        public object? TestObject { get; set; }

        #endregion
    }

    #endregion
}