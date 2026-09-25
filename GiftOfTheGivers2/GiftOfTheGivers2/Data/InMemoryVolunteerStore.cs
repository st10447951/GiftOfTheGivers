using GiftOfTheGivers2.Models;

namespace GiftOfTheGivers2.Data
{
    // Stands in for the Volunteers table designed in Section B.
    // In-memory only at prototype stage — resets on app restart.
    public class InMemoryVolunteerStore
    {
        private readonly List<Volunteer> _volunteers = new();
        private readonly object _lock = new();
        private int _nextId = 1;

        public Volunteer Add(Volunteer volunteer)
        {
            lock (_lock)
            {
                volunteer.VolunteerId = _nextId++;
                _volunteers.Add(volunteer);
                return volunteer;
            }
        }

        public List<Volunteer> GetAll()
        {
            lock (_lock)
            {
                return _volunteers.OrderByDescending(v => v.RegisteredDate).ToList();
            }
        }
    }
}