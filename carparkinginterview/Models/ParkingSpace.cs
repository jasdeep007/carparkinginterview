namespace carparkinginterview.Models
{
    public class ParkingSpace
    {
        public int Id { get; set; }
        public string Size { get; set; } // Small, Medium, Large
        public bool IsOccupied { get; set; }
    }
}
