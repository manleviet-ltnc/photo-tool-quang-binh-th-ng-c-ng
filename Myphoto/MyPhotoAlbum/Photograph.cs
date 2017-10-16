using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manning.MyPhotoAlbum
{
    /// <summary>
    /// The photograph class represents a photographic
    /// image stored in the file system
    /// </summary>
    public class Photograph : IDisposable
    {
        private string _fileName;
        public string FileName
        {
            get { return this._fileName;  }
        }

        private Bitmap _bitmap;
        public Bitmap Image
        {
            get
            {
                if (_bitmap == null)
                    _bitmap = new Bitmap(_fileName);
                return _bitmap;
            }
        }

        private string _caption = "";
        public string Caption
        {
            get { return _caption; }
            set
            {
                if (_caption !=value)
                {
                    _caption = value;
                    HasChanged = true;
                }
            }
        }

        private string _photographer = "";
        public string Photograper
        {
            get { return _photographer; }
            set
            {
                if (_photographer != value)
                {
                    _photographer = value;
                    HasChanged = true;
                }
            }
        }

        private DateTime _dataTaken = DateTime.Now;
        public DateTime DataTaken
        {
            get { return _dataTaken; }
            set
            {
                if (_dataTaken != value)
                {
                    _dataTaken = value;
                    HasChanged = true;
                }
            }
        }

        private string _notes = "";
        public string Note
        {
            get { return _notes; }
            set
            {
                if(_notes != value)
                {
                    _notes = value;
                    HasChanged = true;
                }
            }
        }

        private bool _hasChanged = true;
        public bool HasChanged
        {
            get { return _hasChanged; }
            set { _hasChanged = value; }

        }
        public Photograph(string fileName)
        {
            _fileName = fileName;
            _bitmap = null;
            _caption = System.IO.Path.GetFileNameWithoutExtension(fileName);
        }
        public override bool Equals(object obj)
        {
            if (obj is Photograph)
            {
                Photograph p = (Photograph)obj;
                return string.Equals(FileName, p.FileName, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return FileName.ToLowerInvariant().GetHashCode();
        }
        public override string ToString()
        {
            return FileName;
        }

        public void ReleaseImage()
        {
            if (_bitmap != null)
            {
                _bitmap.Dispose();
                _bitmap = null;
            }
        }

        public void Dispose()
        {
            ReleaseImage();
        }
    }
}
