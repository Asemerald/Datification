using System;
using UnityEngine;

namespace Utils.Event
{

    public class StringEventArgs : EventArgs
    {
        public string String { get; }

        public StringEventArgs(string String)
        {
            String = String;
        }
    }
    
    public class IntEventArgs : EventArgs
    {
        public int Int { get; }

        public IntEventArgs(int Int)
        {
            Int = Int;
        }
    }
    
    public class FloatEventArgs : EventArgs
    {
        public float Float { get; }

        public FloatEventArgs(float Float)
        {
            Float = Float;
        }
    }
    
    public class BoolEventArgs : EventArgs
    {
        public bool Bool { get; }

        public BoolEventArgs(bool Bool)
        {
            Bool = Bool;
        }
    }
    
    public class GameObjectEventArgs : EventArgs
    {
        public GameObject GameObject { get; }

        public GameObjectEventArgs(GameObject GameObject)
        {
            GameObject = GameObject;
        }
    }
    
    public class TransformEventArgs : EventArgs
    {
        public Transform Transform { get; }

        public TransformEventArgs(Transform Transform)
        {
            Transform = Transform;
        }
    }
    
    
    
}
