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
using DemoApp.Model;

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
