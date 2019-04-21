using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace HML.Expansion.Entity.Work
{

    public class Job
    {
        public EventHandler JobFinished;

        public enum PriorityLevel
        {
            Low,
            Medium,
            High
        }

        public Vector3 Position { get; set; }
        public Action<object> JobAction { get; set; }
        public PriorityLevel Level { get; set; }
        public float Priority { get; set; }
        public List<BaseEntity> Workers { get; set; }
        public bool CanBeTeamWorked { get; set; }
        public bool Active { get; set; }

        public Job()
        {

        }

        public bool CanBeWorkedByEntity(BaseEntity entity)
        {
            if (entity is HumanEntity)
                return true;
            return false;
        }

        public bool AddWorker(BaseEntity entity, bool priority = false)
        {
            Workers.Add(entity);
            entity.QueueJob(this);
            return true;
        }

        internal void Work(BaseEntity baseEntity)
        {
            Console.WriteLine("Entity is doing work.");
            if (JobAction != null)
                JobAction(baseEntity);
            JobFinished(this, new EventArgs());
        }
    }

}
