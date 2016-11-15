using PdmProAddIn.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PdmProAddIn.ViewModels
{
    public class AAFileRefViewModel : ViewModel
    {
        AAFileRefsViewModel _owner;
        AAFileRef _data;

        public AAFileRefViewModel(AAFileRefsViewModel owner, AAFileRef data)
        {
            _owner = owner;
            _data = data;

            this._path = _data.Path;
            this._isIncluded = true;
        }

        protected override void RaisePropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IsIncluded):
                    // Handle to support multi-select check/uncheck of many items at once.
                    foreach(var res in _owner.Results.Where(x => x != this && x.IsSelected))
                    {
                        res.IsIncluded = this.IsIncluded;
                    }
                    break;
            }
            base.RaisePropertyChanged(propertyName);
        }

        private bool _isIncluded;
        public bool IsIncluded
        {
            get { return _isIncluded; }
            set { SetProperty(ref _isIncluded, value); }
        }

        private string _path;
        public string Path
        {
            get { return _path; }
            set { SetProperty(ref _path, value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }
    }

    public class AAFileRefsViewModel : ViewModel
    {
        public AAFileRefsViewModel(string parentFilePath, IEnumerable<AAFileRef> fileRefs)
        {
            this._parentFilePath = parentFilePath;
            this.OkCommand = new DelegateCommand(Ok, CanOk);
            this.Results = new ObservableCollection<AAFileRefViewModel>(fileRefs.Select(x => new AAFileRefViewModel(this, x)));
        }

        public ObservableCollection<AAFileRefViewModel> Results { get; set; }

        private string _parentFilePath;
        /// <summary>
        /// The file that was originally selected by the user.
        /// </summary>
        public string ParentFilePath
        {
            get
            {
                return _parentFilePath;
            }

            set
            {
                SetProperty(ref _parentFilePath, value);
            }
        }

        public bool OkWasClicked { get; private set; }

        public DelegateCommand OkCommand { get; set; }
        public void Ok()
        {
            OkWasClicked = true;
        }
        public bool CanOk()
        {
            return Results.Any(x => x.IsIncluded == true);
        }
    }
}
