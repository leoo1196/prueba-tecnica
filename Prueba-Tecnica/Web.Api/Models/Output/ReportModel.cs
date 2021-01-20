namespace Api.Models.Output
{
    public class ReportModel
    {
        public int IdRoom { get; set; }

        public string RoomName { get; set; }

        public int TotalReservations { get; set; }

        public int TotalAssistants { get; set; }

        public int AverageAssistants { get; set; }

        public int ProjectorUtilization { get; set; }

        public int BlackboardUtilization { get; set; }

        public int InternetUtilization { get; set; }
    }
}
