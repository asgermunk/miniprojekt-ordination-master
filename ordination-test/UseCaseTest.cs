namespace ordination_test;

using Microsoft.EntityFrameworkCore;

using Service;
using Data;
using shared.Model;

[TestClass]
public class UseCaseTest
{
    private DataService service;

    [TestInitialize]
    public void SetupBeforeEachTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>();
        optionsBuilder.UseInMemoryDatabase(databaseName: "test-database");
        var context = new OrdinationContext(optionsBuilder.Options);
        service = new DataService(context);
        service.SeedData();
    }

    [TestMethod]
    public void DagligFastLessThan4()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(1, service.GetDagligFaste().Count());

        service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligFaste().Count());
    }
    [TestMethod]
    public void AnbefaletDosis()
    {
        //TODO: Implement this test case.
    }   

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void DagligSkævBadDate()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();
        // Create an array of Dosis objects
        Dosis[] doser = new Dosis[3] {
            new Dosis(DateTime.Now.AddHours(8), 1),  // Morning dose at 8:00
            new Dosis(DateTime.Now.AddHours(12), 2), // Noon dose at 12:00
            new Dosis(DateTime.Now.AddHours(18), 1)  // Evening dose at 18:00
        };
        
        // This should throw an exception because end date is before start date
        service.OpretDagligSkaev(patient.PatientId, lm.LaegemiddelId, doser,
             DateTime.Now.AddDays(4), DateTime.Now.AddDays(3));
        
        // If we reach this point without an exception, the test will fail
    } 
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void PN0orLess()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(4, service.GetPNs().Count());

        service.OpretPN(patient.PatientId, lm.LaegemiddelId, 0, DateTime.Now.AddDays(3), DateTime.Now.AddDays(4));
        service.OpretPN(patient.PatientId, lm.LaegemiddelId, -1, DateTime.Now.AddDays(3), DateTime.Now.AddDays(4));
        Assert.AreEqual(5, service.GetPNs().Count());
    }
    [TestMethod]
    public void PNmoreThan0()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(4, service.GetPNs().Count());

        service.OpretPN(patient.PatientId, lm.LaegemiddelId, 1, DateTime.Now.AddDays(3), DateTime.Now.AddDays(4));
        Assert.AreEqual(5, service.GetPNs().Count());
    }
}