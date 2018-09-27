using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChessGame
{
    public class Event
    {
        public Event(object data)
        {
            Data = data;
        }

        public object Data { get; }
    }

    internal class Target
    {
        public readonly Action<Event> Action;
        public Target(Action<Event> action)
        {
            Action = action;
        }

        public void Activate(object data)
        {
            var e = new Event(data);
            Action(e);
        }
    }
    public class EventEmitter
    {
        private readonly Dictionary<string, List<Target>>targets;
        public EventEmitter()
        {
            targets = new Dictionary<string, List<Target>>();
        }
        public void On(string name, Action<Event> action)
        {
            var target = new Target(action);
            if (!targets.ContainsKey(name))
            {
                targets[name] = new List<Target>();
            }
            targets[name].Add(target);
        }

        public void Off(string name, Action<Event> action)
        {
            if (!targets.ContainsKey(name)) return;
            
            var list = targets[name];
            foreach (var target in list)
            {
                if (target.Action != action) continue;
                list.Remove(target);
                return;
            }
        }

        public void Emit(string name, object data=null)
        {
            if (!targets.ContainsKey(name)) return;
            
            var list = new Target[targets[name].Count];
            targets[name].CopyTo(list);
            //
            foreach (var target in list)
            {
                target.Activate(data);
            }
        }
    }
}
