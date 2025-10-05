using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klassen_oefening
{
    internal class Program
    {

        public enum Verschijningsperiode
        {
            Dagelijks,
            Wekelijks,
            Maandelijks
        }
        public class Boek
        {
            private decimal prijs;
            public string Isbn { get; set; }
            public string Naam { get; set; }
            public string Uitgever { get; set; }

            public decimal Prijs
            {
                get { return prijs; }
                set
                {
                    if (value < 5m) prijs = 5m;
                    else if (value > 50m) prijs = 50m;
                    else prijs = value;
                }
            }

            public Boek() { }

            public Boek(string isbn, string naam, string uitgever, decimal prijs)
            {
                Isbn = isbn;
                Naam = naam;
                Uitgever = uitgever;
                Prijs = prijs;
            }

            public virtual void Lees()
            {
                Console.WriteLine("Geef de boekgegevens in:");
                Console.Write("ISBN: "); Isbn = Console.ReadLine();
                Console.Write("Naam: "); Naam = Console.ReadLine();
                Console.Write("Uitgever: "); Uitgever = Console.ReadLine();
                Console.Write("Prijs: "); Prijs = decimal.Parse(Console.ReadLine());
            }

            public override string ToString()
            {
                return $"ISBN: {Isbn}, Naam: {Naam}, Uitgever: {Uitgever}, Prijs: {Prijs}€";
            }

        }

        // klasse Tijdschrift
        public class Tijdschrift : Boek
        {
            public Verschijningsperiode Periode { get; set; }

            public Tijdschrift() : base() { }

            public Tijdschrift(string isbn, string naam, string uitgever, decimal prijs, Verschijningsperiode periode)
                : base(isbn, naam, uitgever, prijs)
            {
                Periode = periode;
            }

            public override void Lees()
            {
                base.Lees();
                Console.WriteLine("Kies verschijningsperiode: 0-Dagelijks, 1-Wekelijks, 2-Maandelijks");
                Periode = (Verschijningsperiode)int.Parse(Console.ReadLine());
            }

            public override string ToString()
            {
                return base.ToString() + $", Periode: {Periode}";
            }
        }

        public class Bestelling<T>
        {
            private static int idCounter = 1;
            public int Id { get; private set; }
            public T Item { get; set; }
            public DateTime Datum { get; set; }
            public int Aantal { get; set; }
            public Verschijningsperiode? Periode { get; set; }

            


            public event Action<Bestelling<T>> Besteld;

            public Bestelling(T item, int aantal, Verschijningsperiode? periode = null)
            {
                Id = idCounter++;
                Item = item;
                Aantal = aantal;
                Datum = DateTime.Now;
                Periode = periode;
                Besteld?.Invoke(this);
            }

            public Tuple<string, int, decimal> Bestel()
            {
                string isbn = "";
                decimal totaal = 0;

                if (Item is Boek boek)
                {
                    isbn = boek.Isbn;
                    totaal = boek.Prijs * Aantal;
                }

                return new Tuple<string, int, decimal>(isbn, Aantal, totaal);
            }
        }

    }
}
