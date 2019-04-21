using System;
using System.Collections.Generic;
using System.ComponentModel;
using HML.Expansion.Entity.Work;
using HML.Expansion.Model;
using Microsoft.Xna.Framework;

namespace HML.Expansion.Entity
{
    public abstract class BaseEntity : INotifyPropertyChanged
    {
        private float x;
        private float y;
        private Inventory entityInventory;

        public const string XPropertyName = "X";
        public float X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
                OnPropertyChanged(XPropertyName);
            }
        }

        public const string YPropertyName = "Y";
        public float Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
                OnPropertyChanged(YPropertyName);
            }
        }



        public float GetMaxSpeed()
        {
            return 1.5f;
        }

        public Job ActiveJob { get; set; }

        public const string EntityInventoryPropertyName = "EntityInventory";
        public Inventory EntityInventory
        {
            get
            {
                return entityInventory;
            }

            set
            {
                entityInventory = value;
                OnPropertyChanged(EntityInventoryPropertyName);
            }
        }

        protected Queue<Job> JobQueue = new Queue<Job>();

        public EventHandler EntityDataChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        //TODO: Need to have a specific  position changed event?
        public void SetPosition(float x, float y)
        {
            this.x = x;
            this.y = y;
            OnPropertyChanged(StringContants.ALL_PROPERTIES_PROPERTY_NAME);
        }

        public void QueueJob(Job job)
        {
            job.JobFinished += OnJobFinished;
            JobQueue.Enqueue(job);
        }

        public void OnJobFinished(object sender, EventArgs e)
        {
            ActiveJob = null;
        }

        public void Update()
        {
            //if (ActiveJob != null)
            //{
            //    if (Vector3.Distance(ActiveJob.Position, new Vector3(x, y, 0)) > .05f)
            //    {
            //        WorldController.Instance.EntityController.MoveEntityToward(ActiveJob.Position, this);
            //    }
            //    else
            //    {
            //        ActiveJob.Work(this);
            //    }
            //}
            //else if (JobQueue.Count > 0)
            //{
            //    ActiveJob = JobQueue.Dequeue();
            //}
        }
    }

}
