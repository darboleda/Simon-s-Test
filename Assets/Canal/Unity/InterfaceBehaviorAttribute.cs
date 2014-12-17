using System;
using System.Linq;
using System.Reflection;

using UnityEngine;

namespace Canal.Unity
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InterfaceBehaviorAttribute : PropertyAttribute
    {
        private Type targetType;
        public Type TargetType { get { return targetType; } }

        public InterfaceBehaviorAttribute(Type targetType)
        {
            this.targetType = targetType;
        }

        public bool IsValidTarget(Behavior behavior)
        {
            return behavior != null && behavior.GetType().GetInterfaces().Any((x) => this.targetType.IsAssignableFrom(x));
        }
    }
}
