using GiftOfTheGivers2.Models;

namespace GiftOfTheGivers2.Data
{
    // Simple thread-safe in-memory store standing in for the
    // Donations + TaxCertificates tables designed in Section B.
    // Data resets whenever the app restarts — acceptable at prototype stage
    // per the brief's "data persistence can be minimal" instruction.
    public class InMemoryDonationStore
    {
        private readonly List<Donation> _donations = new();
        private readonly object _lock = new();
        private int _nextId = 1;

        public Donation Add(Donation donation)
        {
            lock (_lock)
            {
                donation.DonationId = _nextId++;
                donation.CertificateNumber = $"GG-{DateTime.Now:yyyy}-{donation.DonationId:D5}";
                _donations.Add(donation);
                return donation;
            }
        }

        public Donation? GetById(int id)
        {
            lock (_lock)
            {
                return _donations.FirstOrDefault(d => d.DonationId == id);
            }
        }

        public List<Donation> GetAll()
        {
            lock (_lock)
            {
                return _donations.OrderByDescending(d => d.DonationDate).ToList();
            }
        }
    }
}