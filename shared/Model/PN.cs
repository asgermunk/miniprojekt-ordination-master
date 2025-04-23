namespace shared.Model;

public class PN : Ordination {
	public double antalEnheder { get; set; }
    public List<Dato> dates { get; set; } = new List<Dato>();

    public PN (DateTime startDen, DateTime slutDen, double antalEnheder, Laegemiddel laegemiddel) : base(laegemiddel, startDen, slutDen) {
		this.antalEnheder = antalEnheder;
	}

    public PN() : base(null!, new DateTime(), new DateTime()) {
    }

    /// <summary>
    /// Registrerer at der er givet en dosis på dagen givesDen
    /// Returnerer true hvis givesDen er inden for ordinationens gyldighedsperiode og datoen huskes
    /// Returner false ellers og datoen givesDen ignoreres
    /// </summary>
    public bool givDosis(Dato givesDen) {
        // Check if the date is within the validity period
        if (givesDen.dato >= startDen && givesDen.dato <= slutDen) {
            dates.Add(givesDen);
            return true; // Datoen er registreret
        }
        return false;
    }

    public override double doegnDosis() {
        double antalOrdinationer = dates.Count();
        // Calculate the time span between start and end dates
        TimeSpan tidsRum = slutDen - startDen;
        // Now you can get the total days using:
        double antalDage = tidsRum.TotalDays;
        
        double døgnDosis = (antalOrdinationer * antalEnheder) / antalDage;
        return døgnDosis;
        
        
    }

    public override double samletDosis() {
        return dates.Count() * antalEnheder;
    }

    public int getAntalGangeGivet() {
        return dates.Count();
    }

	public override String getType() {
		return "PN";
	}
}
