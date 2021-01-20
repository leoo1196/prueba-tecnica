namespace Api.Models.Output
{
    public class RoomModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public bool HasProjector { get; set; }

        public bool HasBlackboard { get; set; }

        public bool HasInternet { get; set; }

        public bool IsReserved { get; set; }
    }
}
