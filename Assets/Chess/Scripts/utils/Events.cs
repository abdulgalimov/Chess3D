using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChessGame
{
    public class Event
    {
        public Event(object data)
        {
            _data = data;
        }

        private object _data;
        public object Data
        {
            get { return _data; }
        }
        
    }

    class Target
    {
        public Action<Event> _action;
        public Target(Action<Event> action)
        {
            _action = action;
        }

        public void Activate(object data)
        {
            Event e = new Event(data);
            _action(e);
        }
    }
    public class EventEmitter
    {
        private Dictionary<string, List<Target>>_targets;
        public EventEmitter()
        {
            _targets = new Dictionary<string, List<Target>>();
        }
        public void on(string name, Action<Event> action)
        {
            Target target = new Target(action);
            if (!_targets.ContainsKey(name))
            {
                _targets[name] = new List<Target>();
            }
            _targets[name].Add(target);
        }

        public void off(string name, Action<Event> action)
        {
            if (!_targets.ContainsKey(name)) return;
            List <Target> list = _targets[name];
            foreach (Target target in list)
            {
                if (target._action == action)
                {
                    list.Remove(target);
                    return;
                }
            }
        }

        public void emit(string name, object data=null)
        {
            if (_targets.ContainsKey(name))
            {
                List<Target> list = _targets[name];
                foreach (Target target in list)
                {
                    target.Activate(data);
                }
            }
        }
    }
}
