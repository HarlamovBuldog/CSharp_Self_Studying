using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using DemoApp.DataAccess;
using DemoApp.Properties;
using DemoApp.Support;

namespace DemoApp.ViewModel
{
    public class AllGroupsViewModel : WorkspaceViewModel
    {
        #region Fields

        readonly GroupRepository _groupRepository;


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
            
        }

        public AllGroupsViewModel(GroupRepository groupRepository,
            ReadOnlyCollection<CommandViewModel> childsNavigateCommandList)
        {
            if (groupRepository == null)
                throw new ArgumentNullException("groupRepository");

            base.DisplayName = Strings.AllGroupsViewModel_DisplayName;

            _groupRepository = groupRepository;

            // Subscribe for notifications of when a new group is saved.
            _groupRepository.GroupAdded += this.OnGroupAddedToRepository;

            // Populate the AllGroups collection with GroupViewModels.
            this.CreateAllGroups(childsNavigateCommandList);
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

        void CreateAllGroups(ReadOnlyCollection<CommandViewModel> childsNavigateCommandList)
        {
            List<GroupViewModel> all =
                (from cust in _groupRepository.GetGroups()
                 select new GroupViewModel(cust, _groupRepository)).ToList();

            foreach (GroupViewModel gvm in all)
            {
                gvm.PropertyChanged += this.OnGroupViewModelPropertyChanged;
                foreach (CommandViewModel cmndVM in childsNavigateCommandList)
                {
                    if (gvm.DisplayName == cmndVM.DisplayName)
                        gvm.ChildsNavigateExecute = cmndVM.Command;
                }
                    
            }

            this.AllGroups = new ObservableCollection<GroupViewModel>(all);
            this.AllGroups.CollectionChanged += this.OnCollectionChanged;
        }

        #endregion // Constructor

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

        public GroupViewModel SelectedGroup { get; set; }
        /*
        public bool IsPropertyEmpty(object obj)
        {
            var type = obj.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var hasProperty = properties.Select(x => x.GetValue(this, null))
                                        .Any(x => !x.IsNullOrEmpty());
            return !hasProperty;
        }
        */

        #endregion // Public Interface

        #region CRUD Functions

        protected override void CreateNew()
        {
        }
        protected override void EditCurrent()
        {
        }
        protected override void CommitUpdates()
        {
        }
        protected override void DeleteCurrent()
        {
        }
        protected override void RefreshData()
        {
        }

        #endregion // CRUD Functions

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
            
            // When a group is selected or unselected, we must let the
            // world know that the TotalSelectedSales property has changed,
            // so that it will be queried again for a new value.
            if (e.PropertyName == IsSelected)
                this.OnPropertyChanged("SelectedGroup");
             
        }

        void OnGroupAddedToRepository(object sender, GroupAddedEventArgs e)
        {
            var viewModel = new GroupViewModel(e.NewGroup, _groupRepository);
            this.AllGroups.Add(viewModel);
        }

        #endregion // Event Handling Methods
    }
}
