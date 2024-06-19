using Azure.Search.Documents.Indexes;

namespace YourOwnData.Models
{
    public partial class Query
    {
        public string COMP_CODE { get; set; }

        [SimpleField(IsKey = true, IsFilterable = true)]
        public string PART_NO { get; set; }

        [SearchableField(IsFilterable = true, IsFacetable = true)]
        public string PART_GRP { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string PART_DESC { get; set; }
        public decimal? REVI_PRCE { get; set; }

        /*
        [SimpleField(IsKey = true, IsFilterable = true)]
        public string HotelId { get; set; }

        [SearchableField(IsSortable = true)]
        public string HotelName { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string Description { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.FrLucene)]
        [JsonPropertyName("Description_fr")]
        public string DescriptionFr { get; set; }

        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string Category { get; set; }

        [SearchableField(IsFilterable = true, IsFacetable = true)]
        public string[] Tags { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public bool? ParkingIncluded { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public DateTimeOffset? LastRenovationDate { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public double? Rating { get; set; }

        [SearchableField]
        public Address Address { get; set; }
        */
    }
}
