using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Xml;
using System.Xml.Linq;
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

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Creates a new repository of childs.
        /// </summary>
        /// <param name="childDataFile">The relative path to an XML resource file that contains child data.</param>
        public ChildRepository(string childDataFile)
        {
            _childs = LoadChilds(childDataFile);
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

        static List<Child> LoadChilds(string childDataFile)
        {
            // In a real application, the data would come from an external source,
            // but for this demo let's keep things simple and use a resource file.
            using (Stream stream = GetResourceStream(childDataFile))
            using (XmlReader xmlRdr = new XmlTextReader(stream))
                return
                    (from childElem in XDocument.Load(xmlRdr).Element("childs").Elements("child")
                     select Child.CreateChild(
                        (int)childElem.Attribute("code"),
                        (string)childElem.Attribute("name"),
                        (string)childElem.Attribute("simpleName"),
                        (int)childElem.Attribute("groupCode"),
                        (string)childElem.Attribute("imgPath")
                         )).ToList();
        }

        static Stream GetResourceStream(string resourceFile)
        {
            Uri uri = new Uri(resourceFile, UriKind.RelativeOrAbsolute);

            StreamResourceInfo info = Application.GetResourceStream(uri);
            if (info == null || info.Stream == null)
                throw new ApplicationException("Missing resource file: " + resourceFile);

            return info.Stream;
        }

        #endregion // Private Helpers
    }
}