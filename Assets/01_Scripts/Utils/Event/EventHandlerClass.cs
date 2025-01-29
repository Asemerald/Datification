using System;
using UnityEngine;

namespace Utils.Event
{

    public class StringEventArgs : EventArgs
    {
        public string String { get; }

        public StringEventArgs(string argString)
        {
            String = argString;
        }
    }
    
    public class IntEventArgs : EventArgs
    {
        public int Int { get; }

        public IntEventArgs(int argInt)
        {
            Int = argInt;
        }
    }
    
    public class FloatEventArgs : EventArgs
    {
        public float Float { get; }

        public FloatEventArgs(float argFloat)
        {
            Float = argFloat;
        }
    }
    
    public class BoolEventArgs : EventArgs
    {
        public bool Bool { get; }

        public BoolEventArgs(bool argBool)
        {
            Bool = argBool;
        }
    }
    
    public class GameObjectEventArgs : EventArgs
    {
        public GameObject GameObject { get; }

        public GameObjectEventArgs(GameObject argGameObject)
        {
            GameObject = argGameObject;
        }
    }
    
    public class TransformEventArgs : EventArgs
    {
        public Transform Transform { get; }

        public TransformEventArgs(Transform argTransform)
        {
            Transform = argTransform;
        }
    }
    
    
    
}
