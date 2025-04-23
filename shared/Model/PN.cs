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
        // If no doses have been given, return 0
        if (dates.Count == 0) {
            return 0;
        }
        
        // Get earliest and latest dates when doses were given
        DateTime start = dates.Min(d => d.dato);
        DateTime slut = dates.Max(d => d.dato);
        
        // Calculate the number of days between first and last dose
        TimeSpan periode = slut - start;
        double antalDage = periode.TotalDays > 0 ? periode.TotalDays : 1; // Prevent division by zero
        
        // Calculate daily dose
        double antalOrdinationer = dates.Count();
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
