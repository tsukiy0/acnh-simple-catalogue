using System.Collections.Generic;

namespace Core.Catalogue
{
    public class Item
    {
        private Id id;
        private Name name;
        private CatalogueStatus catalogueStatus;
        private Image image;
        private IList<Variant> variants;

        public Item(Id id, Name name, CatalogueStatus catalogueStatus, Image image, IList<Variant> variants)
        {
            this.id = id;
            this.name = name;
            this.catalogueStatus = catalogueStatus;
            this.image = image;
            this.variants = variants;
        }

        public class Id
        {
            private string value { get; }

            private Id(string value)
            {
                this.value = value;
            }

            public static Id From(string input)
            {
                return new Id(input);
            }

            override public string ToString()
            {
                return value;
            }
        }

        public class Name
        {
            private string value { get; }

            private Name(string value)
            {
                this.value = value;
            }

            public static Name From(string input)
            {
                return new Name(input);
            }

            override public string ToString()
            {
                return value;
            }
        }

        public class Variant
        {
            private Id id;
            private Name name;
            private Image image;

            public Variant(Id id, Name name, Image image)
            {
                this.id = id;
                this.name = name;
                this.image = image;
            }

            public class Id
            {
                private string value { get; }

                private Id(string value)
                {
                    this.value = value;
                }

                public static Id From(string input)
                {
                    return new Id(input);
                }

                override public string ToString()
                {
                    return value;
                }
            }

            public class Name
            {
                private string value { get; }

                private Name(string value)
                {
                    this.value = value;
                }

                public static Name From(string input)
                {
                    return new Name(input);
                }

                override public string ToString()
                {
                    return value;
                }
            }
        }
    }
}