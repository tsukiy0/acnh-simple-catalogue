using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign;
using Core.Catalogue;
using Core.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OneOf;

namespace Web.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        private IItemService itemService { get; set; }

        public class FilterCache
        {
            public string Search { get; set; }
            public List<CatalogueStatus> CatalogueStatuses { get; set; }
            public List<Source> Sources { get; set; }
        }

        private FilterCache filterCache = new FilterCache
        {
            Search = "",
            CatalogueStatuses = new List<CatalogueStatus>(),
            Sources = new List<Source>()
        };

        private List<Item> items = new List<Item>();

        private void OnChangeSearch(string input)
        {
            filterCache.Search = input;
        }

        private void OnChangeCatalogueStatuses(OneOf<string, IEnumerable<string>, LabeledValue, IEnumerable<LabeledValue>> value, OneOf<SelectOption, IEnumerable<SelectOption>> option)
        {
            filterCache.CatalogueStatuses = value.AsT1
                .Select(catalogueStatusFromString)
                .ToList();
        }

        private void OnChangeSources(OneOf<string, IEnumerable<string>, LabeledValue, IEnumerable<LabeledValue>> value, OneOf<SelectOption, IEnumerable<SelectOption>> option)
        {
            filterCache.Sources = value.AsT1
                .Select(sourceFromString)
                .ToList();
        }

        private void OnApplyFilters(EditContext editContext)
        {
            items = itemService.List(new ItemFilter(filterCache.Search, filterCache.CatalogueStatuses, filterCache.Sources));
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