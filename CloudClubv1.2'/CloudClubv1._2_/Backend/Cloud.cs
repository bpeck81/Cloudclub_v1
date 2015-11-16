using System;

namespace Backend
{
    public class Cloud : DBItem
    {
        public string Id { get; set; }

        public DateTime? Time { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Radius { get; set; }


        //TODO: in the future, clouds will have to have more specific locations than just a geofence circle for states
        public Cloud(string title, string description, double latitude, double longitude, double radius)
        {
            Time = DateTime.Now;

            Title = title;
            Description = description;
            Latitude = latitude;
            Longitude = longitude;
            Radius = radius;
        }
    }
}

