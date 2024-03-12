namespace HabitTracker.Models
{
    public class AvailableIcon
    {
        public List<string> AvailableIconsDone { get; set; }
        public List<string> AvailableIconsPartiallyDone { get; set; }
        public AvailableIcon()
        {
            AvailableIconsDone = new List<string>
            {
                "bi bi-check-square-fill",
                "bi bi-star-fill",
                "bi bi-balloon-heart-fill",
                "bi bi-check-circle-fill",
                "bi bi-gift-fill",
                "bi bi-hand-thumbs-up-fill",
                "bi bi-emoji-sunglasses-fill",
                "bi bi-rocket-takeoff-fill"


            };

            AvailableIconsPartiallyDone = new List<string>
            {
                "bi bi-check-square",
                "bi bi-star",
                "bi bi-balloon-heart",
                "bi bi-check-circle",
                "bi bi-gift",
                "bi bi-hand-thumbs-up",
                "bi bi-emoji-sunglasses",
                "bi bi-rocket-takeoff"

            };
        }

    }
}
