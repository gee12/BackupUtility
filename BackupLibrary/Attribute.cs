using System;
using System.Collections.Generic;
using System.Text;

namespace BackupLibrary
{
    class XMLAttribute<T>
    {
        public string Name;
        public T Default;
        public T Value;
        public Type Type;

        public XMLAttribute(string name, T def)
        {
            this.Name = name;
            this.Default = def;
            this.Type = typeof(T);
        }

        public XMLAttribute(XMLAttribute<T> attr, string value)
        {
            this.Name = attr.Name;
            this.Default = attr.Default;
            this.Type = typeof(T);
            SetValue(value);
        }

        public void SetValue(T value)
        {
            this.Value = value;
        }

        public void SetValue(string value)
        {
            this.Value = (T)Convert.ChangeType(value, typeof(T));
        }

        //public T GetValue(T type)
        //{
        //    (T)Convert.ChangeType(value, typeof(type));
        //}
    }
}
