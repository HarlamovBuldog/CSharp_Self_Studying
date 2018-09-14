using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using DemoApp.DataAccess;
using DemoApp.Properties;

namespace DemoApp.ViewModel
{
    public class AllGroupsViewModel : WorkspaceViewModel
    {
        #region Fields

        readonly GroupRepository _groupRepository;
        readonly ChildRepository _childRepository;
        CommandViewModel _getChildsCommand;

        public CommandViewModel GetChildsCommand
        {
            get
            {
                if (_getChildsCommand == null)
                {
                    CommandViewModel cmd = new CommandViewModel(
                        new RelayCommand(param => this.ShowAllChilds())
                        );
                    _getChildsCommand = cmd;        
                }
                return _getChildsCommand;
            }
        }

        ObservableCollection<WorkspaceViewModel> _childWorkspaces;

        #endregion // Fields

        #region Constructor

        public AllGroupsViewModel(GroupRepository groupRepository)
        {
            if (groupRepository == null)
                throw new ArgumentNullException("groupRepository");

            base.DisplayName = Strings.AllGroupsViewModel_DisplayName;

            _groupRepository = groupRepository;

            // Subscribe for notifications of when a new group is saved.
            _groupRepository.GroupAdded += this.OnGroupAddedToRepository;

            // Populate the AllGroups collection with GroupViewModels.
            this.CreateAllGroups();

            //
            _childRepository = new ChildRepository("Data/childs.xml");
        }

        void CreateAllGroups()
        {
            List<GroupViewModel> all =
                (from cust in _groupRepository.GetGroups()
                 select new GroupViewModel(cust, _groupRepository)).ToList();

            foreach (GroupViewModel gvm in all)
                gvm.PropertyChanged += this.OnGroupViewModelPropertyChanged;

            this.AllGroups = new ObservableCollection<GroupViewModel>(all);
            this.AllGroups.CollectionChanged += this.OnCollectionChanged;
        }

        #endregion // Constructor

        #region Workspaces

        /// <summary>
        /// Returns the collection of available workspaces to display.
        /// A 'workspace' is a ViewModel that can request to be closed.
        /// </summary>
        public ObservableCollection<WorkspaceViewModel> ChildWorkspaces
        {
            get
            {
                if (_childWorkspaces == null)
                {
                    _childWorkspaces = new ObservableCollection<WorkspaceViewModel>();
                    _childWorkspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _childWorkspaces;
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
            this.ChildWorkspaces.Remove(workspace);
        }

        #endregion // Workspaces

        #region Public Interface

        /// <summary>
        /// Returns a collection of all the GroupViewModel objects.
        /// </summary>
        public ObservableCollection<GroupViewModel> AllGroups { get; private set; }

        /*
        /// <summary>
        /// Returns the total sales sum of all selected groups.
        /// </summary>
        public double TotalSelectedSales
        {
            get
            {
                return this.AllGroups.Sum(
                    custVM => custVM.IsSelected ? custVM.TotalSales : 0.0);
            }
        }
        */

        #endregion // Public Interface

        #region  Base Class Overrides

        protected override void OnDispose()
        {
            foreach (GroupViewModel custVM in this.AllGroups)
                custVM.Dispose();

            this.AllGroups.Clear();
            this.AllGroups.CollectionChanged -= this.OnCollectionChanged;

            _groupRepository.GroupAdded -= this.OnGroupAddedToRepository;
        }

        #endregion // Base Class Overrides

        #region Private Helpers

        void ShowAllChilds()
        {

            AllChildsViewModel workspace =
                this.ChildWorkspaces.FirstOrDefault(vm => vm is AllChildsViewModel)
                as AllChildsViewModel;

            if (workspace == null)
            {
                workspace = new AllChildsViewModel(_childRepository);
                this.ChildWorkspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);

        }

        void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.ChildWorkspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.ChildWorkspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        #endregion // Private Helpers

        #region Event Handling Methods

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (GroupViewModel custVM in e.NewItems)
                    custVM.PropertyChanged += this.OnGroupViewModelPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (GroupViewModel custVM in e.OldItems)
                    custVM.PropertyChanged -= this.OnGroupViewModelPropertyChanged;
        }

        void OnGroupViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            // Make sure that the property name we're referencing is valid.
            // This is a debugging technique, and does not execute in a Release build.
            (sender as GroupViewModel).VerifyPropertyName(IsSelected);
            /*
            // When a group is selected or unselected, we must let the
            // world know that the TotalSelectedSales property has changed,
            // so that it will be queried again for a new value.
            if (e.PropertyName == IsSelected)
                this.OnPropertyChanged("TotalSelectedSales");
             */
        }

        void OnGroupAddedToRepository(object sender, GroupAddedEventArgs e)
        {
            var viewModel = new GroupViewModel(e.NewGroup, _groupRepository);
            this.AllGroups.Add(viewModel);
        }

        #endregion // Event Handling Methods
    }
}
