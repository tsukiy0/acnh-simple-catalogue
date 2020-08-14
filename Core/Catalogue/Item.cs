namespace Core.Catalogue
{
    public struct Item
    {
        public readonly Id id;
        public readonly Name name;
        public readonly CatalogueStatus catalogueStatus;
        public readonly Image image;
        public readonly Variant? variant;

        public Item(Id id, Name name, CatalogueStatus catalogueStatus, Image image, Variant? variant)
        {
            this.id = id;
            this.name = name;
            this.catalogueStatus = catalogueStatus;
            this.image = image;
            this.variant = variant;
        }

        public struct Id
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

        public struct Name
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

        public struct Variant
        {
            public readonly Id id;
            public readonly Name name;

            public Variant(Id id, Name name)
            {
                this.id = id;
                this.name = name;
            }

            public struct Id
            {
                private readonly string value;

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

            public struct Name
            {
                private readonly string value;

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