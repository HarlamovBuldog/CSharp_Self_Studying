using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using DemoApp.DataAccess;
using DemoApp.Model;
using DemoApp.Properties;
using Microsoft.Win32;

namespace DemoApp.ViewModel
{
    /// <summary>
    /// A UI-friendly wrapper for a Group object.
    /// </summary>
    public class GroupViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        #region Fields

        readonly Group _group;
        readonly GroupRepository _groupRepository;
        //string _groupType;
        //string[] _groupTypeOptions;
        bool _isSelected;
        RelayCommand _saveCommand;
        RelayCommand _openFileDialogCommand;

        #endregion // Fields

        #region Constructor

        public GroupViewModel(Group group, GroupRepository groupRepository)
        {
            if (group == null)
                throw new ArgumentNullException("group");

            if (groupRepository == null)
                throw new ArgumentNullException("groupRepository");

            _group = group;
            _groupRepository = groupRepository;
            //_groupType = Strings.GroupViewModel_GroupTypeOption_NotSpecified;
        }

        #endregion // Constructor

        #region Group Properties

        public int Id
        {
            get { return _group.Id; }
            set
            {
                if (value == _group.Id)
                    return;

                _group.Id = value;

                base.OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get { return _group.Name; }
            set
            {
                if (value == _group.Name)
                    return;

                _group.Name = value;

                base.OnPropertyChanged("Name");
            }
        }

        public string ImgPath
        {
            get { return _group.ImgPath; }
            set
            {
                if (value == _group.ImgPath)
                    return;

                _group.ImgPath = value;

                base.OnPropertyChanged("ImgPath");
            }
        }

        #endregion // Group Properties

        #region Presentation Properties

        public override string DisplayName
        {
            get
            {
                if (this.IsNewGroup)
                {
                    return Strings.GroupViewModel_DisplayName;
                }
                else
                {
                    return _group.Name;
                }
            }
        }

        /// <summary>
        /// Gets/sets whether this group is selected in the UI.
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
        /// Returns a command that saves the group.
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

        public ICommand OpenFileDialogCommand
        {
            get
            {
                if (_openFileDialogCommand == null)
                {
                    _openFileDialogCommand = new RelayCommand(
                        param => this.OpenFileDialog()
                        );
                }
                return _openFileDialogCommand;
            }
        }

        public ICommand ChildsNavigateExecute { get; set; }



        #endregion // Presentation Properties

        #region Public Methods

        public void OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";
            if (openFileDialog.ShowDialog() == true)
                ImgPath = openFileDialog.FileName;
        }

        /// <summary>
        /// Saves the group to the repository.  This method is invoked by the SaveCommand.
        /// </summary>
        public void Save()
        {
            if (!_group.IsValid)
                throw new InvalidOperationException(Strings.GroupViewModel_Exception_CannotSave);

            if (this.IsNewGroup)
                _groupRepository.AddGroup(_group);

            base.OnPropertyChanged("DisplayName");
        }

        #endregion // Public Methods

        #region Private Helpers

        /// <summary>
        /// Returns true if this group was created by the user and it has not yet
        /// been saved to the group repository.
        /// </summary>
        bool IsNewGroup
        {
            get { return !_groupRepository.ContainsGroup(_group); }
        }

        /// <summary>
        /// Returns true if the group is valid and can be saved.
        /// </summary>
        bool CanSave
        {
            get { return _group.IsValid; }
        }

        #endregion // Private Helpers

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error
        {
            get { return (_group as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = null;

                error = (_group as IDataErrorInfo)[propertyName];
 
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