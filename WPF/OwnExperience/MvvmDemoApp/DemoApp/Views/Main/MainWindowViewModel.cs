using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using DemoApp.DataAccess;
using DemoApp.Model;
using DemoApp.Properties;
using Support;

namespace DemoApp.ViewModel
{
    /// <summary>
    /// The ViewModel for the application's main window.
    /// </summary>
    public class MainWindowViewModel : WorkspaceViewModel
    {
        #region Fields
        
        ReadOnlyCollection<CommandViewModel> _getChildsCommands;
        ReadOnlyCollection<CommandViewModel> _commands;
        readonly GroupRepository _groupRepository;
        readonly ChildRepository _childRepository;
        //Dictionary<string, ReadOnlyCollection<CommandViewModel>> _commandsExecByChilds;
        ObservableCollection<WorkspaceViewModel> _workspaces;
        private WorkspaceViewModel _currentPageViewModel;
        string _curVMDisplayName;
        RelayCommand _returnBackCommand;
        bool _enableEditCommands;
//        ICommand _getChildsCommand;

        #endregion // Fields

        #region State Properties

        public bool EnableEditCommands
        {
            get
            {
                return _enableEditCommands;
            }
            set
            {
                if (_enableEditCommands != value)
                {
                    _enableEditCommands = value;
                    OnPropertyChanged("EnableEditCommands");
                }
            }
        }

        /// <summary>
        /// Returns a string DisplayName 
        /// of currently opened Workspace.
        /// Used for databinding with title 
        /// of container representing workspace area.
        /// </summary>
        public string CurVMDisplayName
        {
            get
            {
                return _curVMDisplayName;
            }
            set
            {
                if (_curVMDisplayName != value)
                {
                    _curVMDisplayName = value;
                    OnPropertyChanged("CurVMDisplayName");
                }
            }
        }

        public WorkspaceViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }


        #endregion // State Properties

        #region Constructor

        public MainWindowViewModel(string[] customerDataFile)
        {
            base.DisplayName = Strings.MainWindowViewModel_DisplayName;
            _groupRepository = new GroupRepository();
            _childRepository = new ChildRepository();
            _curVMDisplayName = this.DisplayName;
            this.ShowAllGroups();
            //next string throws exception
            //this.PropertyChanged += OnEnableEditCommandsPropertyChanged;
            _enableEditCommands = true;
        }

        #endregion // Constructor

        #region Commands

        public ICommand ReturnBackCommand
        {
            get
            {
                if (_returnBackCommand == null)
                {
                    _returnBackCommand = new RelayCommand(
                        param => this.ReturnBack()
                        );
                }
                return _returnBackCommand;
            }
        }

        /// <summary>
        /// Returns a read-only list of commands 
        /// that the UI can display and execute.
        /// </summary>
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (_commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();
                    _commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _commands;
            }
        }

        List<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel(
                    "Create",
                    new RelayCommand(param => this.CreateNewGroup())),

                new CommandViewModel(
                    "Refresh",
                    new RelayCommand(param => this.CreateNewGroup())),
                //Do i actually need refresh? Guess i need to call it automatically
                //each time i change something.
                //Or this update means that db has changed externally. Not via programm.
                new CommandViewModel(
                    "Edit",
                    new RelayCommand(param => this.CreateNewGroup())),

                new CommandViewModel(
                    "Delete",
                    new RelayCommand(param => this.CreateNewGroup()))

            };
        }

        public ReadOnlyCollection<CommandViewModel> GetChildsCommands
        {
            get
            {
                if (_getChildsCommands == null)
                {
                    List<CommandViewModel> cmds = this.CreateGetChildsCommands();
                    _getChildsCommands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _getChildsCommands;
            }
        }

        List<CommandViewModel> CreateGetChildsCommands()
        {
            List<CommandViewModel> cmds = new List<CommandViewModel>();
            foreach (Group gr in _groupRepository.GetGroups())
            {
                cmds.Add(new CommandViewModel(
                    gr.Name,
                    new RelayCommand(param => this.ShowAllChildsInt(gr.Id))
                    ));
            }
            return cmds;
        }

        #endregion // Commands

        #region Workspaces

        /// <summary>
        /// Returns the collection of available workspaces to display.
        /// A 'workspace' is a ViewModel that can request to be closed.
        /// </summary>
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _workspaces;
            }
        }

        void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }

        void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            workspace.Dispose();
            this.Workspaces.Remove(workspace);
        }

        #endregion // Workspaces

        #region Private Helpers

        void CreateNewGroup()
        {
            Group newGroup = Group.CreateNewGroup();
            GroupViewModel workspace = new GroupViewModel(newGroup, _groupRepository);
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        void ShowAllGroups()
        {
            AllGroupsViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllGroupsViewModel)
                as AllGroupsViewModel;

            if (workspace == null)
            {
                workspace = new AllGroupsViewModel(_groupRepository, GetChildsCommands);
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }
        
        void ShowAllChildsInt(int groupId)
        {
            AllChildsViewModel workspace = 
                new AllChildsViewModel(new ChildRepository(groupId));
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
            //CurVMDisplayName += ">" + workspace.DisplayName; 

        }

        void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
            {
                collectionView.MoveCurrentTo(workspace);
            }
            CurrentPageViewModel = workspace;
            //CurVMDisplayName = workspace.DisplayName;
        }

        void ReturnBack()
        {
            CurrentPageViewModel = Workspaces.Single(vm => vm is AllGroupsViewModel)
                as AllGroupsViewModel;
        }

        #endregion // Private Helpers

        #region Event Handling Methods

        void OnEnableEditCommandsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var allGrVM = _workspaces.Single(vm => vm is AllGroupsViewModel)
                            as AllGroupsViewModel;

            if (allGrVM.SelectedGroup == null ||
                String.IsNullOrWhiteSpace(allGrVM.SelectedGroup.ToString()))
            {
                EnableEditCommands = false;
            }
            else
            {
                EnableEditCommands = true;
            }

            this.OnPropertyChanged("EnableEditCommands");
        }

        #endregion
    }
}