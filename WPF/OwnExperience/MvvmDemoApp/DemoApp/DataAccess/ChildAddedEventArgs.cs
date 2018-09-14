using System;
using DemoApp.Model;

namespace DemoApp.DataAccess
{
    /// <summary>
    /// Event arguments used by ChildRepository's ChildAdded event.
    /// </summary>
    public class ChildAddedEventArgs : EventArgs
    {
        public ChildAddedEventArgs(Child newChild)
        {
            this.NewChild = newChild;
        }

        public Child NewChild { get; private set; }
    }
}