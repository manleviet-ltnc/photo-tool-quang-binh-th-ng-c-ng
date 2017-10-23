using System;

namespace Manning.MyPhotoAlbum
{
    internal class CryptoTextBase
    {
        private string password;
        private object password1;

        public CryptoTextBase(string password)
        {
            this.password = password;
        }

        public CryptoTextBase(object password1)
        {
            this.password1 = password1;
        }

        internal string ProcessText(string value, bool v)
        {
            throw new NotImplementedException();
        }
    }
}