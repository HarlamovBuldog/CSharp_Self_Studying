using System;
using DemoApp.Model;

namespace DemoApp.DataAccess
{
    /// <summary>
    /// Event arguments used by GroupRepository's GroupAdded event.
    /// </summary>
    public class GroupAddedEventArgs : EventArgs
    {
        public GroupAddedEventArgs(Group newGroup)
        {
            this.NewGroup = newGroup;
        }

        public Group NewGroup { get; private set; }
    }
}