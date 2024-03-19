// ==========================================================================
// Copyright (C) 2024 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Microsoft.Extensions.Logging;

namespace JitBugArm64
{
    #region Classes

    public class ItemSelector
    {
        #region Constants

        private readonly Guid m_id;

        private readonly ILogger m_logger;

        private readonly float m_scoreThreshold;

        private readonly float m_significantGap = 0.001f;

        #endregion

        #region Fields

        private Entity? m_selectedEntity;

        #endregion

        #region Constructors

        public ItemSelector(Guid id, Settings settings, ILogger logger)
        {
            m_id = id;
            m_scoreThreshold = settings.ScoreThreshold;
            m_logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Public Methods

        public void Update(EntityCollection entityCollection)
        {
            Entity? newEntity = entityCollection.Entities.FirstOrDefault(x => x.Id == m_id);

            if (newEntity != null)
            {
                Select(newEntity);
            }
        }

        #endregion

        #region Private Methods

        private static float GetScoreB()
        {
            // Dummy score, useful to reproduce problem.
            return Random.Shared.NextSingle();
        }

        private void Select(Entity trackingObject)
        {
            if (ShouldSelectEntity(trackingObject))
            {
                SelectEntity(trackingObject);
            }
        }

        private void SelectEntity(Entity entity)
        {
            if (entity.Score <= m_scoreThreshold)
            {
                // We should never get here because of the test in 
                // ShouldSelectEntity.
                throw new Exception(
                    $"Should not have selected this object because Score {entity.Score} is lower than threshold {m_scoreThreshold}");
            }

            m_logger.LogDebug("Selecting entity score={score} in id {id}",
                entity.Score, m_id);

            UnselectEntity();

            m_selectedEntity = entity;
        }

        /// <summary>
        /// THIS METHOD DOES NOT SEEM TO COMPILE/JIT PROPERLY.
        /// It should return false if entity.Score &lt;= m_scoreThreshold
        /// but it does not, as proved by the exception throwing in <see cref="SelectEntity" />
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        //[MethodImpl(MethodImplOptions.NoInlining)]
        private bool ShouldSelectEntity(Entity entity)
        {
            m_logger.LogInformation("!!");

            //m_logger.LogDebug("Id: {id}, Score: {score}, threshold: {threshold}",
            //    m_id, entity.Score, m_scoreThreshold);

            if (entity.TestObject != null ||
                entity.Score <= m_scoreThreshold) // THIS DOES NOT SEEM TO WORK!!!
                return false;

            m_logger.LogDebug("Score: {score}, threshold: {threshold}",
                entity.Score, m_scoreThreshold);

            if (m_selectedEntity == null)
            {
                return true;
            }

            float selectedScoreB = GetScoreB();
            float newScoreB = GetScoreB();

            bool newEntityIsBetter = entity.Score > m_selectedEntity!.Score &&
                Math.Abs(selectedScoreB - newScoreB) < m_significantGap;

            newEntityIsBetter = newEntityIsBetter && Random.Shared.Next(2) == 0;

            return newEntityIsBetter;
        }

        private void UnselectEntity()
        {
            m_selectedEntity = null;
        }

        #endregion
    }

    #endregion
}