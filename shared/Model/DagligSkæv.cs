namespace shared.Model;

public class DagligSkæv : Ordination {
    public List<Dosis> doser { get; set; } = new List<Dosis>();

    public DagligSkæv(DateTime startDen, DateTime slutDen, Laegemiddel laegemiddel) : base(laegemiddel, startDen, slutDen) {
	}

    public DagligSkæv(DateTime startDen, DateTime slutDen, Laegemiddel laegemiddel, Dosis[] doser) : base(laegemiddel, startDen, slutDen) {
        this.doser = doser.ToList();
    }    

    public DagligSkæv() : base(null!, new DateTime(), new DateTime()) {
    }

	public void opretDosis(DateTime tid, double antal) {
		if(antal <= 0) {
			throw new ArgumentException("Antal skal være større end 0.");
		}
		
        doser.Add(new Dosis(tid, antal));
    }

	public override double samletDosis() {
		return base.antalDage() * doegnDosis();
	}

	public override double doegnDosis()
	{
		double doegn = 0;
		foreach (Dosis dos in doser)
		{
			doegn += dos.antal;
		}

		return doegn;
	}

	public override String getType() {
		return "DagligSkæv";
	}
}
