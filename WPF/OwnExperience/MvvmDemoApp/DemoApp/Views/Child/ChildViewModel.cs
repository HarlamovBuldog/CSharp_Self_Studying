using System;
using System.ComponentModel;
using System.Windows.Input;
using DemoApp.DataAccess;
using DemoApp.Model;
using DemoApp.Properties;

namespace DemoApp.ViewModel
{
    /// <summary>
    /// A UI-friendly wrapper for a Child object.
    /// </summary>
    public class ChildViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        #region Fields

        readonly Child _child;
        readonly ChildRepository _childRepository;
        //string _childType;
        //string[] _childTypeOptions;
        bool _isSelected;
        RelayCommand _saveCommand;

        #endregion // Fields

        #region Constructor

        public ChildViewModel(Child child, ChildRepository childRepository)
        {
            if (child == null)
                throw new ArgumentNullException("child");

            if (childRepository == null)
                throw new ArgumentNullException("childRepository");

            _child = child;
            _childRepository = childRepository;
            //_childType = Strings.ChildViewModel_ChildTypeOption_NotSpecified;
        }

        #endregion // Constructor

        #region Child Properties

        public int Id
        {
            get { return _child.Code; }
            set
            {
                if (value == _child.Code)
                    return;

                _child.Code = value;

                base.OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get { return _child.Name; }
            set
            {
                if (value == _child.Name)
                    return;

                _child.Name = value;

                base.OnPropertyChanged("Name");
            }
        }

        public string SimpleName
        {
            get { return _child.SimpleName; }
            set
            {
                if (value == _child.SimpleName)
                    return;

                _child.SimpleName = value;

                base.OnPropertyChanged("SimpleName");
            }
        }

        public int GroupCode
        {
            get { return _child.GroupCode; }
            set
            {
                if (value == _child.GroupCode)
                    return;

                _child.GroupCode = value;

                base.OnPropertyChanged("GroupCode");
            }
        }

        public string ImgPath
        {
            get { return _child.ImgPath; }
            set
            {
                if (value == _child.ImgPath)
                    return;

                _child.ImgPath = value;

                base.OnPropertyChanged("ImgPath");
            }
        }

        #endregion // Child Properties

        #region Presentation Properties

        public override string DisplayName
        {
            get
            {
                if (this.IsNewChild)
                {
                    return Strings.ChildViewModel_DisplayName;
                }
                else
                {
                    return _child.Name;
                }
            }
        }

        /// <summary>
        /// Gets/sets whether this child is selected in the UI.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected)
                    return;

                _isSelected = value;

                base.OnPropertyChanged("IsSelected");
            }
        }

        /// <summary>
        /// Returns a command that saves the child.
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        param => this.Save(),
                        param => this.CanSave
                        );
                }
                return _saveCommand;
            }
        }

        #endregion // Presentation Properties

        #region Public Methods

        /// <summary>
        /// Saves the child to the repository.  This method is invoked by the SaveCommand.
        /// </summary>
        public void Save()
        {
            if (!_child.IsValid)
                throw new InvalidOperationException(Strings.ChildViewModel_Exception_CannotSave);

            if (this.IsNewChild)
                _childRepository.AddChild(_child);

            base.OnPropertyChanged("DisplayName");
        }

        #endregion // Public Methods

        #region Private Helpers

        /// <summary>
        /// Returns true if this child was created by the user and it has not yet
        /// been saved to the child repository.
        /// </summary>
        bool IsNewChild
        {
            get { return !_childRepository.ContainsChild(_child); }
        }

        /// <summary>
        /// Returns true if the child is valid and can be saved.
        /// </summary>
        bool CanSave
        {
            get { return _child.IsValid; }
        }

        #endregion // Private Helpers

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error
        {
            get { return (_child as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = null;

                error = (_child as IDataErrorInfo)[propertyName];

                // Dirty the commands registered with CommandManager,
                // such as our Save command, so that they are queried
                // to see if they can execute now.
                CommandManager.InvalidateRequerySuggested();

                return error;
            }
        }

        #endregion // IDataErrorInfo Members
    }
}