using System;
using System.Collections.Generic;
using System.Text;

namespace CarApp_michelle
{
    internal class Car
    {
        public string Brand { get; private set; }
        public string Model { get; private set; }
        public int Year { get; private set; }
        public char GearType { get; private set; }
        public FuelTypeEnum FuelType { get; private set; }
        public double KmPerLiter { get; private set; }
        public double KmCount { get; private set; }
        public bool IsEngineOn { get; private set; }


        private double calculatedTripPrice;
        private List<Trip> tripList;


        // ================================ Konstruktør ================================
        public Car()
        {
            Brand = "-";
            Model = "-";
            Year = 0;
            GearType = 'M';
            FuelType = FuelTypeEnum.Benzin;
            KmPerLiter = 0;
            KmCount = 0;

            IsEngineOn = false;
            calculatedTripPrice = 0;

            tripList = new List<Trip>();
        }

        public Car(string brand, string model, int year, char gearType, FuelTypeEnum fuelType, double kmPerLiter, double kmCount)
        {
            Brand = brand;
            Model = model;
            Year = year;
            GearType = gearType;
            FuelType = fuelType;
            KmPerLiter = kmPerLiter;
            KmCount = kmCount;

            IsEngineOn = false;
            calculatedTripPrice = 0;

            tripList = new List<Trip>();
        }

        public void TurnOnEngine() => IsEngineOn = true;
        public void TurnOffEngine() => IsEngineOn = false;


        public List<Trip> GetTrips() => tripList;


        // ================================ Returner bilens data i en string ================================
        public string GetCarDetails()
        {
            return $"{Brand} {Model} ({Year}) | Brændstof: {FuelType} | " +
            $"Gear: {GearType} | Km-tæller: {KmCount} km | " +
            $"Motor: {(IsEngineOn ? "Tændt" : "Slukket")}";
        }

        // ================================ 2) Kør en køretur ================================
        public void Drive(Trip newTrip)
        {
            if (newTrip.Car == this)
            {
                if (IsEngineOn == true)
                {
                    Console.WriteLine($"\nDen originale kilometerstand: {KmCount}");
                    Console.WriteLine("Starter køretur...");

                    KmCount += newTrip.Distance;
                    tripList.Add(newTrip);

                    Console.WriteLine($"Du har nu kørt: {newTrip.Distance}");
                    Console.WriteLine($"Den nye kilometerstand er: {KmCount}");
                }
                else
                {
                    Console.WriteLine("Start bilen først");
                }
            }
            else
            {
                Console.WriteLine("Fejl: Denne tur tilhører ikke denne bil.");
            }
        }

        // ================================ 3) Udregn prisen på en køretur ================================
        public double CalculateTripPrice()
        {
            if (KmPerLiter == 0)          // Check om bilens kmPerLiter er 0, for at undgå et crash pga. division med 0
            {
                Console.WriteLine("Fejl: kmPerLiter kan ikke være 0!");
            }
            else if (FuelType != FuelTypeEnum.Benzin && FuelType != FuelTypeEnum.Diesel) // Check om brændstofstypens værdi er noget andet end enten (B)enzin eller (D)iesel, og vis en fejl hvis den er ( != bruges som "ikke lig med", && bruges som "og" )
            {
                Console.WriteLine("Fejl: Ukendt brændstoftype!");
            }
            else                          // Hvis vi ikke støder på nogen af de fejl, når vi herned og kan begynde at udregne køreturens pris
            {
                double literPrice = 0;    // Vi ærklærer først bare lige hurtigt en lokal variabel for literprisen på brændstof,
                string fuelTypeStr = "";  // (og en string til fuelType for at kunne skrive f.eks. "Benzin" i stedet for "B", da fuelType er en char)
                switch (FuelType)         // og sætter deres værdi baseret på brændstofstypen, så vi er klar til at bruge literprisen i udregningen
                {
                    case FuelTypeEnum.Benzin:
                        literPrice = 13.49;
                        fuelTypeStr = FuelType.ToString(); // "Benzin";
                        break;
                    case FuelTypeEnum.Diesel:
                        literPrice = 12.29;
                        fuelTypeStr = FuelType.ToString(); // "Diesel";
                        break;
                }

                Trip currentTrip = tripList[tripList.Count - 1]; // brug den sidst kørte tur

                //Beregn prisen for køreturen
                calculatedTripPrice = currentTrip.CalculateTripPrice(literPrice);

                Console.WriteLine("\n================ Oplysninger om køreturen ================");

                Console.WriteLine($"Brændstoftype: " + fuelTypeStr);
                Console.WriteLine($"Bilen kører " + KmPerLiter + " km/l");
                Console.WriteLine($"Den originale kilometerstand var: {KmCount} km");

                Console.WriteLine($"Den nye kilometerstand er: {Math.Round(KmCount)} km"); // Bruger Math.Round() til at runde op/ned til nærmeste hele tal, fordi kilometertælleren skulle være en double, men jeg føler det måske lyder lidt fjollet med decimaler i en kilometertæller idk jeg kender ikke så meget til biler for at være helt ærlig :)
                Console.WriteLine($"Total brændstofudgift: {calculatedTripPrice} kr.");
            }
            return calculatedTripPrice;
        }


        // ================================ 4) Er kilometerstanden et palindrom? ================================
        public bool IsPalindrome(int kmInput)
        {
            string kmStr = kmInput.ToString(); // Kilometertælleren er en double, vi laver den om til en string for at kunne behandle tallene som tegn i stedet for tal

            if (kmStr.Length <= 2) // Hvis kilometertælleren er lig med eller kortere end 2 tegn, ved vi allerede at det må være et palindrom, så vi kan stoppe her allerede med "return = true;"
            {
                Console.WriteLine($"Ja, {kmInput} er et palindrom! :)");
                return true;
            }

            for (int i = 0; i < kmStr.Length / 2; i++) // for loop der tæller "i" op - fra 0 til længden af kmStr, divideret med 2, så den stopper når den når halvejs igennem kmStr
            {
                if (kmStr[i] != kmStr[kmStr.Length - (i + 1)]) // Starter ved index [i] og sammenligner det med index [længden af kmStr - (i + 1)], og tjekker om de ikke er lig med hinanden
                                                               // f.eks. første step sammenligner det første tegn med det sidste, næste step sammenligner det næst-første med det næst-sidste, indtil de møder hinanden på midten, fordi for-loopet stopper når det er nået til "kmStr.Length / 2"
                {
                    Console.WriteLine($"Nej, {kmInput} er ikke et palindrom!");
                    return false;
                }
            }
            Console.WriteLine($"Ja, {kmInput} er et palindrom! :)");
            return true;
        }


        // ================================ 5) Vis biloplysninger ================================
        public void ShowCarDetails()
        {

            Console.WriteLine("\n================ Oplysninger om din bil ================");

            Console.WriteLine($"Bilmærke: {Brand}");
            Console.WriteLine($"Bilmodel: {Model}");
            Console.WriteLine($"Årgang: {Year}");
            Console.WriteLine($"Gear {GearType}");
            Console.WriteLine($"Brændstoftype: {FuelType}");
            Console.WriteLine($"Kører {KmPerLiter} km/l");
            Console.WriteLine($"Kilometerstand: {KmCount} km");
            Console.WriteLine($"Prisen for en typisk køretur er: {calculatedTripPrice} kr.");

            Console.WriteLine("========================================================\n");
        }
    }
}