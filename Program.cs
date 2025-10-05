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
        // 🔹 Programma (Main)
        internal class Program
        {
            static void Main(string[] args)
            {
                // Voorbeeldboeken en tijdschriften
                Boek boek1 = new Boek("123", "C# Basics", "TechUitgever", 20);
                Boek boek2 = new Boek("456", "OOP Principles", "CodeUitgever", 35);

                Tijdschrift tijdschrift1 = new Tijdschrift("789", "Tech Weekly", "Magazine BV", 10, Verschijningsperiode.Wekelijks);
                Tijdschrift tijdschrift2 = new Tijdschrift("101", "Daily News", "News BV", 5, Verschijningsperiode.Dagelijks);

                Console.WriteLine("📚 Boeken:");
                Console.WriteLine(boek1);
                Console.WriteLine(boek2);

                Console.WriteLine("\n📰 Tijdschriften:");
                Console.WriteLine(tijdschrift1);
                Console.WriteLine(tijdschrift2);

                // Bestelling voor een boek
                var bestellingBoek = new Bestelling<Boek>(boek1, 2);
                bestellingBoek.Besteld += b => Console.WriteLine($"✅ Bestelling bevestigd: {b.Item.Naam}, Aantal: {b.Aantal}");
                var tupleBoek = bestellingBoek.Bestel();
                Console.WriteLine($"Tuple: ISBN={tupleBoek.Item1}, Aantal={tupleBoek.Item2}, Totaal={tupleBoek.Item3}€");

                // Bestelling voor een tijdschrift
                var bestellingTijdschrift = new Bestelling<Tijdschrift>(tijdschrift1, 3, tijdschrift1.Periode);
                bestellingTijdschrift.Besteld += b => Console.WriteLine($"✅ Bestelling bevestigd: {b.Item.Naam}, Aantal: {b.Aantal}, Periode: {b.Periode}");
                var tupleTijdschrift = bestellingTijdschrift.Bestel();
                Console.WriteLine($"Tuple: ISBN={tupleTijdschrift.Item1}, Aantal={tupleTijdschrift.Item2}, Totaal={tupleTijdschrift.Item3}€");

                Console.ReadLine();
            }
        }
    }


}

