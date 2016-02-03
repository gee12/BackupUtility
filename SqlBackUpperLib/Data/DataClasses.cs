using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Linq;
using System.Text;

namespace SqlBackUpperLib
{
    /// <summary>
    /// 
    /// </summary>
    public class INotifyPropertiesChanged : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private static PropertyChangedEventArgs allPropsChangedEventArgs = new PropertyChangedEventArgs(null);

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        public virtual void SendAllPropertiesChanged()
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, allPropsChangedEventArgs);
            }
        }

        public virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ITypeReadableName
    {
        string GetReadableTypeName();
    }

    /// <summary>
    /// 
    /// </summary>
    //public interface IRecordId
    //{
    //    int GetRecordId();
    //}

    /// <summary>
    /// 
    /// </summary>
    public abstract class IRecord<T> : INotifyPropertiesChanged, ITypeReadableName, /*IRecordId, */ICloneable, IComparable, IComparable<T>
    {
        public const int DEF_ID = 0;

        //public abstract int GetRecordId();

        public virtual string GetReadableTypeName()
        {
            return string.Empty;
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        public virtual int CompareTo(T other)
        {
            return 0;
        }

        public virtual int CompareTo(object other)
        {
            return this.CompareTo((T)other);
        }
    }
}
