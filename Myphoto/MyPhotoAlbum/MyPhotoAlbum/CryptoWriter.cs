using System;
using System.Text;
using System.IO;


namespace Manning.MyPhotoAlbum
{
    class CryptoWriter : StreamWriter
    {
        private CryptoTextBase _base;
        private object path;
        private object password;

        private CryptoTextBase CryptoBase
        {
            get { return _base; }
        }
        public CryptoWriter(Stream path, string password):base(path)
        {
            if (path == null || path.Length == 0)
            {
                throw new ArgumentNullException("path");
            }

            if (password == null || password.Length == 0)
            {
                throw new ArgumentNullException("password");
            }

            _base = new CryptoTextBase(password);
        }

        public CryptoWriter(string path, string password) : base(path)
        {
            this.password = password;
        }

        public override void WriteLine(string value)
        {
            string encrypted = CryptoBase.ProcessText(value, true);
            base.WriteLine(encrypted);
        }

        public void WriteUnencryptedLine(string value)
        {
            base.WriteLine(value);
        }
    }
}
