using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using shared.Model;

namespace ordination_test
{
    [TestClass]
    public class DagligSkaevModelTest
    {
        private Laegemiddel GetDummyLaegemiddel()
        {
            return new Laegemiddel("Test", 1, 1, 1, "mg");
        }

        [TestMethod]
        public void TC1_SamletDosis_NormalInput_ReturnsCorrectResult()
        {
            var dagligSkaev = new DagligSkæv(new DateTime(2024, 5, 1), new DateTime(2024, 5, 3), GetDummyLaegemiddel()); // 3 dage

            dagligSkaev.opretDosis(new DateTime(2024, 5, 1, 8, 0, 0), 1.0);
            dagligSkaev.opretDosis(new DateTime(2024, 5, 1, 20, 0, 0), 1.0);

            double result = dagligSkaev.samletDosis(); // 3 * 2 = 6

            Assert.AreEqual(6.0, result);
        }

        [TestMethod]
        public void TC2_SamletDosis_NegativeDose_ThrowsException()
        {
            var dagligSkaev = new DagligSkæv(new DateTime(2024, 5, 1), new DateTime(2024, 5, 3), GetDummyLaegemiddel());

            Assert.ThrowsException<ArgumentException>(() =>
            {
                dagligSkaev.opretDosis(new DateTime(2024, 5, 1, 8, 0, 0), -1.0);
            });
        }

        [TestMethod]
        public void TC3_OpretDosis_AddsCorrectlyToList()
        {
            var dagligSkaev = new DagligSkæv(new DateTime(2024, 5, 1), new DateTime(2024, 5, 3), GetDummyLaegemiddel());

            dagligSkaev.opretDosis(new DateTime(2024, 5, 1, 8, 0, 0), 1.0);
            dagligSkaev.opretDosis(new DateTime(2024, 5, 1, 12, 30, 0), 1.5);
            dagligSkaev.opretDosis(new DateTime(2024, 5, 1, 20, 0, 0), 2.0);

            Assert.AreEqual(3, dagligSkaev.doser.Count);

            Assert.IsTrue(dagligSkaev.doser.Any(d => d.tid == new DateTime(2024, 5, 1, 8, 0, 0) && d.antal == 1.0));
            Assert.IsTrue(dagligSkaev.doser.Any(d => d.tid == new DateTime(2024, 5, 1, 12, 30, 0) && d.antal == 1.5));
            Assert.IsTrue(dagligSkaev.doser.Any(d => d.tid == new DateTime(2024, 5, 1, 20, 0, 0) && d.antal == 2.0));
        }

        [TestMethod]
        public void TC4_DoegnDosis_ReturnsSumOfDailyDoses()
        {
            var dagligSkaev = new DagligSkæv(new DateTime(2024, 5, 1), new DateTime(2024, 5, 1), GetDummyLaegemiddel());

            dagligSkaev.opretDosis(new DateTime(2024, 5, 1, 8, 0, 0), 1.0);
            dagligSkaev.opretDosis(new DateTime(2024, 5, 1, 12, 30, 0), 1.5);
            dagligSkaev.opretDosis(new DateTime(2024, 5, 1, 20, 0, 0), 2.0);

            double doegnDosis = dagligSkaev.doegnDosis();

            Assert.AreEqual(4.5, doegnDosis);
        }
    }
}
