using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using DemoApp.DataAccess;
using DemoApp.Properties;

namespace DemoApp.ViewModel
{
    /// <summary>
    /// Represents a container of ChildViewModel objects
    /// that has support for staying synchronized with the
    /// ChildRepository.  This class also provides information
    /// related to multiple selected childs.
    /// </summary>
    public class AllChildsViewModel : WorkspaceViewModel
    {
        #region Fields

        readonly ChildRepository _childRepository;

        #endregion // Fields

        #region Constructor

        public AllChildsViewModel(ChildRepository childRepository)
        {
            if (childRepository == null)
                throw new ArgumentNullException("childRepository");

            base.DisplayName = Strings.AllChildsViewModel_DisplayName;

            _childRepository = childRepository;

            // Subscribe for notifications of when a new child is saved.
            _childRepository.ChildAdded += this.OnChildAddedToRepository;

            // Populate the AllChilds collection with ChildViewModels.
            this.CreateAllChilds();
        }

        void CreateAllChilds()
        {
            List<ChildViewModel> all =
                (from cust in _childRepository.GetChilds()
                 select new ChildViewModel(cust, _childRepository)).ToList();

            foreach (ChildViewModel cvm in all)
                cvm.PropertyChanged += this.OnChildViewModelPropertyChanged;

            this.AllChilds = new ObservableCollection<ChildViewModel>(all);
            this.AllChilds.CollectionChanged += this.OnCollectionChanged;
        }

        #endregion // Constructor

        #region Public Interface

        /// <summary>
        /// Returns a collection of all the ChildViewModel objects.
        /// </summary>
        public ObservableCollection<ChildViewModel> AllChilds { get; private set; }

        /*
        /// <summary>
        /// Returns the total sales sum of all selected childs.
        /// </summary>
        public double TotalSelectedSales
        {
            get
            {
                return this.AllChilds.Sum(
                    custVM => custVM.IsSelected ? custVM.TotalSales : 0.0);
            }
        }
        */

        #endregion // Public Interface

        #region  Base Class Overrides

        protected override void OnDispose()
        {
            foreach (ChildViewModel custVM in this.AllChilds)
                custVM.Dispose();

            this.AllChilds.Clear();
            this.AllChilds.CollectionChanged -= this.OnCollectionChanged;

            _childRepository.ChildAdded -= this.OnChildAddedToRepository;
        }

        #endregion // Base Class Overrides

        #region Event Handling Methods

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (ChildViewModel custVM in e.NewItems)
                    custVM.PropertyChanged += this.OnChildViewModelPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (ChildViewModel custVM in e.OldItems)
                    custVM.PropertyChanged -= this.OnChildViewModelPropertyChanged;
        }

        void OnChildViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            // Make sure that the property name we're referencing is valid.
            // This is a debugging technique, and does not execute in a Release build.
            (sender as ChildViewModel).VerifyPropertyName(IsSelected);

            // When a child is selected or unselected, we must let the
            // world know that the TotalSelectedSales property has changed,
            // so that it will be queried again for a new value.
            //if (e.PropertyName == IsSelected)
               // this.OnPropertyChanged("TotalSelectedSales");
        }

        void OnChildAddedToRepository(object sender, ChildAddedEventArgs e)
        {
            var viewModel = new ChildViewModel(e.NewChild, _childRepository);
            this.AllChilds.Add(viewModel);
        }

        #endregion // Event Handling Methods
    }
}