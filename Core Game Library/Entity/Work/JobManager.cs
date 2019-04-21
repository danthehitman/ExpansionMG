using System.Collections.Generic;

namespace HML.Expansion.Entity.Work
{

    public class JobManager
    {
        private SortedList<Job.PriorityLevel, Job> jobQueue;
        private Dictionary<string, Job> beingWorked;

        public JobManager()
        {
            jobQueue = new SortedList<Job.PriorityLevel, Job>();
            beingWorked = new Dictionary<string, Job>();
        }

        public void ApplyForWork(BaseEntity entity)
        {
            for (int i = 0; i < jobQueue.Count; i++)
            {
                var job = jobQueue.Values[i];
                if (AssignWork(entity, job))
                {
                    jobQueue.RemoveAt(i);
                    return;
                }
            }
        }

        public bool AssignWork(BaseEntity entity, Job job)
        {
            var assigned = false;
            if (job.CanBeWorkedByEntity(entity))
                assigned = job.AddWorker(entity);
            return assigned;
        }
    }
}

