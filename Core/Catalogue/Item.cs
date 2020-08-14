namespace Core.Catalogue
{
    public class Item
    {
        private Id id;
        private Name name;
        private CatalogueStatus catalogueStatus;
        private IList<Variant> variations;
        private Image image;

        public Item(Id id, Name name, CatalogueStatus catalogueStatus, Image image)
        {
            this.id = id;
            this.name = name;
            this.catalogueStatus = catalogueStatus;
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

        public class Variant
        {
            private Id id;
            private Name name;
            private Image image;

            public Item(Id id, Name name, Image image)
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