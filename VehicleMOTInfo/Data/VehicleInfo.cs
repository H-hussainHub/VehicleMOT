namespace VehicleMOTInfo.Data
{
    public class VehicleInfo
    {
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? PrimaryColour { get; set; }
        public List<MotTest>? MotTests { get; set; }

        public MotTest? GetLatestMotTest()
        {
            return MotTests?.OrderByDescending(t => t.CompletedDate).FirstOrDefault();
        }
    }

    public class MotTest
    {
        public string? CompletedDate { get; set; }
        public string? TestResult { get; set; }
        public string? ExpiryDate { get; set; }
        public string? OdometerValue { get; set; }
        public string? OdometerUnit { get; set; }
    }

}
