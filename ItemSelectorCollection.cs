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

    public class ItemSelectorCollection
    {
        #region Constants

        private readonly ILogger m_logger;

        private readonly Settings m_options;

        private readonly Dictionary<Guid, ItemSelector> m_selectors = new();

        #endregion

        #region Constructors

        public ItemSelectorCollection(IOptions<Settings> options,
            ILogger logger)
        {
            m_logger = logger ?? throw new ArgumentNullException(nameof(logger));

            m_options = options.Value;
        }

        #endregion

        #region Public Methods

        public void Add(Guid id)
        {
            var trackItemSelector = new ItemSelector(id, m_options, m_logger);
            m_selectors.Add(id, trackItemSelector);
        }

        public void End(Guid id)
        {
            if (m_selectors.ContainsKey(id))
            {
                m_selectors.Remove(id);
            }
        }

        public void Update(EntityCollection entityCollection)
        {
            foreach (ItemSelector trackItemSelector in m_selectors.Values.ToList())
            {
                trackItemSelector.Update(entityCollection);
            }
        }

        #endregion
    }

    #endregion
}