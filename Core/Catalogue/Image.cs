using System;

namespace Core.Catalogue
{
    public struct Image
    {
        private Uri value { get; }

        private Image(Uri value)
        {
            this.value = value;
        }

        public static Image From(Uri input)
        {
            return new Image(input);
        }

        public static Image From(string input)
        {
            return new Image(new Uri(input));
        }

        public Uri ToUri()
        {
            return value;
        }

        override public string ToString()
        {
            return value.ToString();
        }
    }
}