using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using System.Data.Entity;

namespace Cash_desk
{
    public class GroupViewModel : CrudVMBase
    {
        public GroupVM SelectedGroup { get; set; }
        public ObservableCollection<GroupVM> GroupList { get; set; }
        
        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;
            ObservableCollection<GroupVM> _groups = new ObservableCollection<GroupVM>();
            var groups = await (from p in db.Groups
                                  orderby p.name
                                  select p).ToListAsync();
            foreach (Groups group in groups)
            {
                _groups.Add(new GroupVM { IsNew = false, TheGroup = group });
            }
            GroupList = _groups;
            RaisePropertyChanged("Group");
            ThrobberVisible = Visibility.Collapsed;
        }

        /*
        protected override void CommitUpdates()
        {
            UserMessage msg = new UserMessage();
            var inserted = (from c in GroupList
                            where c.IsNew
                            select c).ToList();
            if (db.ChangeTracker.HasChanges() || inserted.Count > 0)
            {
                foreach (GroupVM c in inserted)
                {
                    db.Groups.Add(c.TheGroup);
                }
                try
                {
                    db.SaveChanges();
                    msg.Message = "Database Updated";
                }
                catch (Exception e)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        ErrorMessage = e.InnerException.GetBaseException().ToString();
                    }
                    msg.Message = "There was a problem updating the database";
                }
            }
            else
            {
                msg.Message = "No changes to save";
            }
            Messenger.Default.Send<UserMessage>(msg);
        }
        protected override void DeleteCurrent()
        {
            UserMessage msg = new UserMessage();
            if (SelectedGroup != null)
            {
                int NumLines = NumberOfOrderLines();
                if (NumLines > 0)
                {
                    msg.Message = string.Format("Cannot delete - there are {0} Order Lines for this Product", NumLines);
                }
                else if (NumLines < 0)
                {
                    msg.Message = "Cannot delete since not committed to database yet";
                }
                else
                {
                    db.Groups.Remove(SelectedGroup.TheGroup);
                    GroupList.Remove(SelectedGroup);
                    RaisePropertyChanged("Groups");
                    msg.Message = "Deleted";
                }
            }
            else
            {
                msg.Message = "No Product selected to delete";
            }
            Messenger.Default.Send<UserMessage>(msg);
        }
        private int NumberOfOrderLines()
        {
            var prod = db.Groups.Find(SelectedGroup.TheGroup.id);
            if (prod == null)
            {
                return -1;
            }
            // Count how many Order Lines there are for the Product
            int linesCount = db.Entry(prod)
                               .Collection(p => p.OrderLines)
                               .Query()
                               .Count();
            return linesCount;
        }
        */

        public GroupViewModel()
            : base()
        {
        }


    }
}
