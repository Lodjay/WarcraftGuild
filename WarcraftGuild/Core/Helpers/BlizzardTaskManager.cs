using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Configuration;

namespace WarcraftGuild.Core.Helpers
{
    public class BlizzardTaskManager
    {
        private readonly Limiter _taskLimiter;
        private List<List<KeyValuePair<Task, long>>> Tasks;

        public BlizzardTaskManager(Limiter limiter)
        {
            _taskLimiter = limiter;
        }

        private void AddTaskToList(Task task, long blizzardCalls, int i = 0)
        {
            if (i < Tasks.Count)
            {
                if (Tasks[i] == null)
                    Tasks[i] = new List<KeyValuePair<Task, long>>();
                if (Tasks[i].Sum(x => x.Value) + blizzardCalls < _taskLimiter.RatesPerTimespan)
                    Tasks[i].Add(new KeyValuePair<Task, long>(task, blizzardCalls));
                else
                    AddTaskToList(task, blizzardCalls, i + 1);
            }
        }

        public void AddTask(Task task, long blizzardCalls = 1)
        {
            if (Tasks == null)
                Tasks = new List<List<KeyValuePair<Task, long>>>();
            AddTaskToList(task, blizzardCalls);
        }

        public async Task RunTaskes()
        {
            bool first = true;
            foreach (List<KeyValuePair<Task, long>> tasks in Tasks)
            {
                if (first)
                    first = false;
                else
                    await Task.Delay(_taskLimiter.TimeBetweenLimitReset);
                List<Task> toDoTasks = new();
                foreach (KeyValuePair<Task, long> task in tasks)
                    toDoTasks.Add(task.Key);
                await Task.WhenAll(toDoTasks).ConfigureAwait(false);
                
            }
            Tasks.Clear();
        }
    }
}