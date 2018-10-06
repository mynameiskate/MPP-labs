namespace DynamicListLab
{
    public class TestItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public override string ToString()
        {
            return $"Item no. {ItemId}: Name: {Name}, location: {Location}";
        }
    }
}
