using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Manning.MyPhotoAlbum
{
    public class AlbumManager
    {
        static private string _defaultPath;
        static public string DefaultPath
        {
            get { return _defaultPath; }
            set { _defaultPath = value; }
        }

        static AlbumManager()
        {
            _defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Album";
        }

        private int _pos = -1;
        public int Index
        {
            get
            {
                int count = Album.Count;
                if (_pos >= count)
                    _pos = count - 1;
                return _pos;
            }
            set
            {
                if (value < 0 || value >= Album.Count)
                    throw new IndexOutOfRangeException("The given index is out of bounds");
                _pos = value;
            }
        }

        private string _name = String.Empty;
        public string FullName
        {
            get { return _name; }
            private set { _name = value; }
        }

        public string ShortName
        {
            get
            {
                if (string.IsNullOrEmpty(FullName))
                    return null;
                else
                    return Path.GetFileNameWithoutExtension(FullName);

            }
        }

        private PhotoAlbum _album;
        public PhotoAlbum Album
        {
            get { return _album; }
        }

        public AlbumManager()
        {
            _album = new PhotoAlbum();
        }

        public AlbumManager(string name) : this()
        {
            _name = name;
            // TODO; load the album
            throw new NotImplementedException();
        }

        public Photograph Current
        {
            get
            {
                if (Index < 0 || Index >= Album.Count)
                    return null;
                return Album[_pos];
            }
        }
        public Bitmap CurrentImage
        {
            get
            {
                if (Current == null)
                    return null;
                return Current.Image;
            }
        }

        static public bool AlbumExits(string name)
        {
            //TODO: implement AlbumExits method
            return false;
        }
        public void Save()
        {
            //TODO: implement Save method
            throw new NotImplementedException();
        }

        public void Save(string name, bool overwrite)
        {
            //TODO: implement Save(name) method
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            if (Index >= Album.Count)
                return false;

            Index++;
            return true;
        }

        public bool MovePrev()
        {
            if (Index <= 0)
                return false;

            Index--;
            return true;

        }
    } 
}
