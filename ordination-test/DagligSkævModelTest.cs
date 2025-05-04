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
            var dagligSkaev = new DagligSkæv(
                new DateTime(2025, 1, 1),
                new DateTime(2025, 1, 3), 
                GetDummyLaegemiddel());

            dagligSkaev.opretDosis(new DateTime(2025), 1.0); 
            dagligSkaev.opretDosis(new DateTime(2025), 1.0); 

            double result = dagligSkaev.samletDosis(); 

            Assert.AreEqual(6.0, result);
        }

        [TestMethod]
        public void TC2_SamletDosis_NegativeDose_ThrowsException()
        {
            var dagligSkaev = new DagligSkæv(
                new DateTime(2025, 1, 1),
                new DateTime(2025, 1, 3),
                GetDummyLaegemiddel());

            Assert.ThrowsException<ArgumentException>(() =>
            {
                dagligSkaev.opretDosis(new DateTime(2025), -1.0); 
            });
        }

        [TestMethod]
        public void TC3_OpretDosis_AddsCorrectlyToList()
        {
            var dagligSkaev = new DagligSkæv(
                new DateTime(2025, 5, 1), 
                new DateTime(2025, 5, 1), 
                GetDummyLaegemiddel());

            dagligSkaev.opretDosis(new DateTime(2025, 7, 12), 1);   
            dagligSkaev.opretDosis(new DateTime(2025, 2, 2), 1);   
            dagligSkaev.opretDosis(new DateTime(2025, 5, 5), 1);  
            dagligSkaev.opretDosis(new DateTime(2025, 6, 19), 1);  

           
            double result = dagligSkaev.doser.Sum(d => d.antal);
            
            Assert.AreEqual(3, result);
        }


        
        [TestMethod]
        public void TC4_DoegnDosis_ReturnsSumOfDailyDoses()
        {
            var dagligSkaev = new DagligSkæv(
                new DateTime(2025, 1, 1),
                new DateTime(2025, 1, 1),
                GetDummyLaegemiddel());

            dagligSkaev.opretDosis(new DateTime(2025), 1); 
            dagligSkaev.opretDosis(new DateTime(2025), 1); 
            dagligSkaev.opretDosis(new DateTime(2025), 1); 

            double result = dagligSkaev.doegnDosis();

            Assert.AreEqual(3, result);
        }
    }
}
