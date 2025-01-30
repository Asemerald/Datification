using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

internal class UnityMainThread : MonoBehaviour
{
    internal static UnityMainThread wkr;
    private Queue<Func<Task>> asyncJobs = new Queue<Func<Task>>();

    void Awake()
    {
        wkr = this;
    }

    async void Update()
    {
        while (asyncJobs.Count > 0)
        {
            Func<Task> job = asyncJobs.Dequeue();
            await job(); // Now properly await async tasks
        }
    }

    // Add synchronous jobs
    internal void AddJob(Action newJob)
    {
        asyncJobs.Enqueue(() => { newJob(); return Task.CompletedTask; });
    }

    // Properly supports awaitable jobs
    internal void AddJobAsync(Func<Task> newJob)
    {
        asyncJobs.Enqueue(newJob);
    }
}