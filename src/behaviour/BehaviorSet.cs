using Godot;
using System.Collections.Generic;

namespace Behaviours
{
    public class BehaviorSet
    {
        public List<Behaviours.Behaviour> Behaviours = new List<Behaviours.Behaviour>();
        public string Name { get; private set; }

        public BehaviorSet(string name)
        {
            Name = name;
        }

        public void AddBehavior(Behaviours.Behaviour behavior)
        {
            Behaviours.Add(behavior);
        }
    }
}