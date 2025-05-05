namespace ordination_test;

using Microsoft.EntityFrameworkCore;

using Service;
using Data;
using shared.Model;

[TestClass]
public class ServiceTest
{
    private DataService service;

    [TestInitialize]
    public void SetupBeforeEachTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>();
        optionsBuilder.UseInMemoryDatabase(databaseName: $"test-database-{DateTime.UtcNow.Ticks}");
        var context = new OrdinationContext(optionsBuilder.Options);
        service = new DataService(context);
        service.SeedData();
    }

    [TestMethod]
    public void PatientsExist()
    {
        Assert.IsNotNull(service.GetPatienter());
    }
    
    
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]

    public void IngenAntalDosis()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();
        service.OpretPN(patient.PatientId, lm.LaegemiddelId,
            0, DateTime.Now, DateTime.Now.AddDays(3));
        
        
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]

    public void StartFørSlut()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();
        Dosis[] doser = new Dosis[3] {
            new Dosis(DateTime.Now.AddHours(8), 1),  // Morning dose at 8:00
            new Dosis(DateTime.Now.AddHours(12), 2), // Noon dose at 12:00
            new Dosis(DateTime.Now.AddHours(18), 1)  // Evening dose at 18:00
        };
        
        service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
           2, 2, 1, 0, DateTime.Now.AddDays(4), DateTime.Now.AddDays(3));
        service.OpretPN(patient.PatientId, lm.LaegemiddelId,
           2, DateTime.Now.AddDays(4), DateTime.Now.AddDays(3));
        service.OpretDagligSkaev(patient.PatientId, lm.LaegemiddelId,doser
            , DateTime.Now.AddDays(4), DateTime.Now.AddDays(3));
    }

    [TestMethod]
    public void OpretDagligFast()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(1, service.GetDagligFaste().Count());

        service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligFaste().Count());
    }

    
   
    [TestMethod]
    public void OpretPN()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(4, service.GetPNs().Count());

        service.OpretPN(patient.PatientId, lm.LaegemiddelId,
            2, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(5, service.GetPNs().Count());
    }
}