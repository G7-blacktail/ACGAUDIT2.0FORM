namespace ACG_AUDIT.ClassCollections
{
    internal class AntivirusProduct
    {
        public string DisplayName { get; set; }
        public string ProductState { get; set; }
    }

    internal class AntivirusProductList
    {
        public List<AntivirusProduct> Products { get; set; } = new List<AntivirusProduct>();
    }
}