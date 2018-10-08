using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using System.IO;
using DemoApp.Properties;
using DemoApp.Model;

namespace DemoApp.DataAccess
{
    /// <summary>
    /// Represents a source of childs in the application.
    /// </summary>
    public class ChildRepository
    {
        #region Fields

        readonly List<Child> _childs;
        private static SQLiteConnection _dbConnection;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Creates a new repository of childs.
        /// </summary>
        public ChildRepository()
        {
            CreateAndOpenDb();
            _childs = LoadChilds();
        }

        public ChildRepository(int groupId)
        {
            CreateAndOpenDb();
            _childs = LoadChilds();
            _childs = GetChildsBelongToGroup(groupId);
        }

        #endregion // Constructor

        #region Public Interface

        /// <summary>
        /// Raised when a child is placed into the repository.
        /// </summary>
        public event EventHandler<ChildAddedEventArgs> ChildAdded;

        /// <summary>
        /// Places the specified child into the repository.
        /// If the child is already in the repository, an
        /// exception is not thrown.
        /// </summary>
        public void AddChild(Child child)
        {
            if (child == null)
                throw new ArgumentNullException("child");

            if (!_childs.Contains(child))
            {
                _childs.Add(child);

                if (this.ChildAdded != null)
                    this.ChildAdded(this, new ChildAddedEventArgs(child));
            }
        }

        /// <summary>
        /// Returns true if the specified child exists in the
        /// repository, or false if it is not.
        /// </summary>
        public bool ContainsChild(Child child)
        {
            if (child == null)
                throw new ArgumentNullException("child");

            return _childs.Contains(child);
        }

        /// <summary>
        /// Returns a shallow-copied list of all childs in the repository.
        /// </summary>
        public List<Child> GetChilds()
        {
            return new List<Child>(_childs);
        }

        #endregion // Public Interface

        #region Private Helpers

        static void CreateAndOpenDb()
        {
            var dbFilePath = Strings.DefaultDbFilePath;
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }
            _dbConnection =
            new SQLiteConnection(ConfigurationManager.
            ConnectionStrings["DefaultConnection"].ConnectionString);
            //_dbConnection.Open();
        }

        static List<Child> LoadChilds()
        {
            // Ensure we have a connection
            if (_dbConnection == null)
            {
                throw new NullReferenceException("Please provide a connection");
            }

            using (_dbConnection)
            {
                return _dbConnection.Query<Child>
                    ("Select * From Children").ToList();
            }
        }


        public List<Child> GetChildsBelongToGroup(int groupId)
        {
            List<Child> children = new List<Child>();
            foreach (Child child in _childs)
            {
                if (child.GroupCode == groupId)
                {
                    children.Add(child);
                }
            }
            return children;
        }


        #endregion // Private Helpers
    }
}