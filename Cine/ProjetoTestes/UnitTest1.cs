using Cine;
using Cine.Database;
using Moq;
using System.Collections.ObjectModel;

namespace ProjetoTestes
{
    public class Tests
    {
        public Mock<IDatabase> mockBase;

        public ObservableCollection<Cinema> cinemas;
        public Cinema newCine;
        public Filme newFilme;
        [SetUp]
        public void Setup()
        {
            mockBase = new();
            cinemas = new ObservableCollection<Cinema>
            {
                new Cinema(
                        1,
                        new ObservableCollection<Filme>(),
                        "Plaza",
                        "AA",
                        0
                    )
            };
            newCine = new Cinema(2, new ObservableCollection<Filme>(), "CinePARK", "USA", 41);
            newFilme = new Filme(1, "New Hope", "George Lucas", 127);
        }

        [Test]
        public void TestReadCinema()
        {
            mockBase.Setup(db => db.ReadCinemas()).Returns(cinemas);
            
            DatabaseManager dbService = new DatabaseManager(mockBase.Object);

            var result = dbService.ReadCinemas();
            Assert.That(result, Is.EqualTo(cinemas));
        }

        [Test]
        public void TestDeleteCinema()
        {
            mockBase.Setup(db => db.Delete(1, cinemas[0])).Returns(true);

            DatabaseManager dbService = new DatabaseManager(mockBase.Object);

            var result = dbService.Delete(1, cinemas[0]);
            Assert.That(result, Is.True);
        }
        [Test]
        public void TestCreateCinema()
        {
            mockBase.Setup(db => db.Create(cinemas[0])).Returns(true);

            DatabaseManager dbService = new DatabaseManager(mockBase.Object);

            var result = dbService.Create(cinemas[0]);
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestUpdateCinema()
        {
            mockBase.Setup(db => db.Update(newCine)).Returns(true);

            DatabaseManager dbService = new DatabaseManager(mockBase.Object);

            var result = dbService.Update(newCine);
            Assert.That(result, Is.True);
        }
        [Test]
        public void TestCreateFilme()
        {
            mockBase.Setup(db => db.Create(newFilme)).Returns(true);

            DatabaseManager dbService = new DatabaseManager(mockBase.Object);

            var result = dbService.Create(newFilme);
            Assert.That(result, Is.True);
        }
        [Test]
        public void TestDeleteFilme()
        {
            mockBase.Setup(db => db.Delete(1, newFilme)).Returns(true);

            DatabaseManager dbService = new DatabaseManager(mockBase.Object);

            var result = dbService.Delete(1, newFilme);
            Assert.That(result, Is.True);
        }
        [Test]
        public void TestUpdateFilme()
        {
            mockBase.Setup(db => db.Update(newFilme)).Returns(true);

            DatabaseManager dbService = new DatabaseManager(mockBase.Object);

            var result = dbService.Update(newFilme);
            Assert.That(result, Is.True);
        }

    }
}