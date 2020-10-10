﻿using System.Collections.Generic;

namespace OpenFL.Core.Instructions.Variables
{
    public class VariableManager<T>
    {

        private readonly VariableManager<T> Parent;
        private readonly Dictionary<string, T> Variables;

        public VariableManager()
        {
            Variables = new Dictionary<string, T>();
        }

        public VariableManager(Dictionary<string, T> variables)
        {
            Variables = variables;
        }

        private VariableManager(VariableManager<T> parent) : this()
        {
            Parent = parent;
        }

        public bool IsDefined(string varName)
        {
            return IsDefinedLocal(varName) || Parent != null && Parent.IsDefined(varName);
        }

        public bool IsDefinedLocal(string varName)
        {
            return Variables.ContainsKey(varName);
        }

        public void ChangeLocalVariable(string varName, T value)
        {
            Variables[varName] = value;
        }

        public void ChangeGlobalVariable(string varName, T value)
        {
            if (Parent != null)
            {
                Parent.ChangeGlobalVariable(varName, value);
            }
            else
            {
                ChangeLocalVariable(varName, value);
            }
        }

        public void ChangeVariable(string varName, T value)
        {
            if (Variables.ContainsKey(varName))
            {
                Variables[varName] = value;
                return;
            }

            if (Parent != null)
            {
                Parent.ChangeVariable(varName, value);
                return;
            }

            throw new Byt3Exception("Changing variable failed. Can not find variable.");
        }

        public T GetVariable(string varName)
        {
            if (Variables.ContainsKey(varName))
            {
                return Variables[varName];
            }

            if (Parent != null)
            {
                return Parent.GetVariable(varName);
            }

            throw new Byt3Exception("Variable Not defined");
        }

        public Dictionary<string, T>.Enumerator GetEnumerator()
        {
            return Variables.GetEnumerator();
        }

        public VariableManager<T> AddScope()
        {
            return new VariableManager<T>(this);
        }

    }
}