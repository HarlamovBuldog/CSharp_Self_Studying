using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Xml;
using System.Xml.Linq;
using DemoApp.Model;
using System.Data.Entity;

namespace DemoApp.DataAccess
{
    /// <summary>
    /// Represents a source of groups in the application.
    /// </summary>
    public class GroupRepository
    {
        #region Fields

        readonly List<Group> _groups;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Creates a new repository of groups.
        /// </summary>
        /// <param name="groupDataFile">The relative path to an XML resource file that contains group data.</param>
        public GroupRepository(ApplicContext dataContext)
        {
            _groups = LoadGroups(dataContext);
        }

        #endregion // Constructor

        #region Public Interface

        /// <summary>
        /// Raised when a group is placed into the repository.
        /// </summary>
        public event EventHandler<GroupAddedEventArgs> GroupAdded;

        /// <summary>
        /// Places the specified group into the repository.
        /// If the group is already in the repository, an
        /// exception is not thrown.
        /// </summary>
        public void AddGroup(Group group)
        {
            if (group == null)
                throw new ArgumentNullException("group");

            if (!_groups.Contains(group))
            {
                _groups.Add(group);

                if (this.GroupAdded != null)
                    this.GroupAdded(this, new GroupAddedEventArgs(group));
            }
        }

        /// <summary>
        /// Returns true if the specified group exists in the
        /// repository, or false if it is not.
        /// </summary>
        public bool ContainsGroup(Group group)
        {
            if (group == null)
                throw new ArgumentNullException("group");

            return _groups.Contains(group);
        }

        /// <summary>
        /// Returns a shallow-copied list of all groups in the repository.
        /// </summary>
        public List<Group> GetGroups()
        {
            return new List<Group>(_groups);
        }

        #endregion // Public Interface

        #region Private Helpers
        static List<Group> LoadGroups(ApplicContext db)
        {
            db.Groups.Load();
            return db.Groups.Local.ToList();
        }

        #endregion // Private Helpers
    }
}