using System.Collections.Generic;
using System.Linq;
using AntDesign;
using Core.Catalogue;
using Core.Shared;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace Web.Shared.Index
{
    public partial class ItemFilterView : ComponentBase
    {
        [Parameter]
        public ItemFilter Model { get; set; }

        [Parameter]
        public EventCallback<ItemFilter> OnChange { get; set; }

        private void OnChangeSearch(string input)
        {
            OnChange.InvokeAsync(new ItemFilter(
                input,
                Model.CatalogueStatuses,
                Model.Sources
            ));
        }

        private void OnChangeCatalogueStatuses(OneOf<string, IEnumerable<string>, LabeledValue, IEnumerable<LabeledValue>> value, OneOf<SelectOption, IEnumerable<SelectOption>> option)
        {
            var catalogueStatuses = value.AsT1
                 .Select(catalogueStatusFromString)
                 .ToList();

            OnChange.InvokeAsync(new ItemFilter(
                Model.Search,
                catalogueStatuses,
                Model.Sources
            ));
        }

        private void OnChangeSources(OneOf<string, IEnumerable<string>, LabeledValue, IEnumerable<LabeledValue>> value, OneOf<SelectOption, IEnumerable<SelectOption>> option)
        {
            var sources = value.AsT1
                 .Select(sourceFromString)
                 .ToList();

            OnChange.InvokeAsync(new ItemFilter(
                Model.Search,
                Model.CatalogueStatuses,
                sources
            ));
        }

        private class BadCatalogueStatusString : BaseException { }

        private CatalogueStatus catalogueStatusFromString(string input)
        {
            switch (input)
            {
                case "FOR_SALE":
                    return CatalogueStatus.FOR_SALE;
                case "NOT_FOR_SALE":
                    return CatalogueStatus.NOT_FOR_SALE;
                case "NOT_EXIST":
                    return CatalogueStatus.NOT_EXIST;
                default:
                    throw new BadCatalogueStatusString();
            }
        }

        private class BadSourceString : BaseException { }

        private Source sourceFromString(string input)
        {
            switch (input)
            {
                case "NOOKS_CRANNY":
                    return Source.NOOKS_CRANNY;
                case "CRAFTING":
                    return Source.CRAFTING;
                case "UNKNOWN":
                    return Source.UNKNOWN;
                default:
                    throw new BadSourceString();
            }
        }
    }
}