using System;
using System.Collections.Generic;
using System.Linq;
using BackedDeveloperAlgorithmProject.Models;

namespace BackedDeveloperAlgorithmProject
{
    public class Program
    {
        public static Event FindNextEvent(List<Event> events)
        {
            return events.OrderByDescending(e => e.Priority).FirstOrDefault();
        }
        public static void Main(string[] args)
        {
            List<Event> events = new List<Event>
            {
                new Event { Id = 1, StartTime = "10:00", EndTime = "12:00", Location = "A", Priority = 50 },
                new Event { Id = 2, StartTime = "10:00", EndTime = "11:00", Location = "B", Priority = 30 },
                new Event { Id = 3, StartTime = "11:30", EndTime = "12:30", Location = "A", Priority = 40 },
                new Event { Id = 4, StartTime = "14:30", EndTime = "16:00", Location = "C", Priority = 70 },
                new Event { Id = 5, StartTime = "14:25", EndTime = "15:30", Location = "B", Priority = 60 },
                new Event { Id = 6, StartTime = "13:00", EndTime = "14:00", Location = "D", Priority = 80 }
            };

            List<DurationBetweenLocations> duration_between_locations_minutes_matrix = new List<DurationBetweenLocations>
            {
                new DurationBetweenLocations { From = "A", To = "B", Duration = 15 },
                new DurationBetweenLocations { From = "A", To = "C", Duration = 20 },
                new DurationBetweenLocations { From = "A", To = "D", Duration = 10 },
                new DurationBetweenLocations { From = "B", To = "C", Duration = 5 },
                new DurationBetweenLocations { From = "B", To = "D", Duration = 25 },
                new DurationBetweenLocations { From = "C", To = "D", Duration = 25 }
            };

            // Some declarations
            List<List<int>> paths = new List<List<int>>();
            List<int> path_scores = new List<int>();

            TimeSpan currentTime;
            String currentLocation;
            int totalPathPoint;

            foreach (Event e in events.ToList()) 
            {
                List<Event> temp_events = new List<Event>(events);
                List<int> path = new List<int>();

                // Initial pose adding in path
                path.Add(e.Id);

                // Current time and location updating
                currentTime = TimeSpan.Parse(e.EndTime);
                currentLocation = e.Location;
                totalPathPoint = e.Priority;

                // Itself and passed events removing
                temp_events.RemoveAll(x => TimeSpan.Parse(x.StartTime) < currentTime);

                // Find next events if its exists
                while (true)
                {
                    Event nextEvent = FindNextEvent(temp_events);

                    if (nextEvent != null)
                    {
                        path.Add(nextEvent.Id);
                        currentTime += TimeSpan.Parse(nextEvent.EndTime) - TimeSpan.Parse(nextEvent.StartTime);
                        currentTime += TimeSpan.FromMinutes(duration_between_locations_minutes_matrix.FirstOrDefault(x => (x.From == currentLocation && x.To == nextEvent.Location) || (x.From == nextEvent.Location && x.To == currentLocation)).Duration);
                        totalPathPoint += nextEvent.Priority;
                        currentLocation = nextEvent.Location;
                        temp_events.RemoveAll(x => TimeSpan.Parse(x.StartTime) < TimeSpan.Parse(nextEvent.EndTime));
                    }
                    else
                    {
                        paths.Add(path);
                        path_scores.Add(totalPathPoint);
                        break;
                    }
                }
            }

            List<Tuple<List<int>, int>> allListsAndScores = new List<Tuple<List<int>, int>>();

            for (int i = 0; i < paths.Count; i++)
            {
                allListsAndScores.Add(new Tuple<List<int>, int>(paths[i], path_scores[i]));
            }

            Tuple<List<int>, int> resultPathAndScore = allListsAndScores.OrderByDescending(x => x.Item2).FirstOrDefault();



            Console.WriteLine("Katılınabilecek Maksimum Etkinlik Sayısı :" + resultPathAndScore.Item1.Count);
            Console.WriteLine("Katılınabilecek Etkinliklerin ID'leri :" + string.Join(", ", resultPathAndScore.Item1));
            Console.WriteLine("Toplam Değer :" + resultPathAndScore.Item2);
            Console.ReadKey();
        }
    }
}
