using Microsoft.VisualBasic.FileIO;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CarApp_michelle
{
    internal class Program
    {
        //Initaliser variabler uden for Main metoden, så de er tilgængelige i hele programmet
        static string brand = "";
        static string model = "";
        static int year = 0;
        static char gearType = ' ';
        static char fuelType = ' ';
        static double kmPerLiter = 0;
        static double kmCount = 0;
        static bool isEngineOn = true;
        static double calculatedTripPrice = 0;

        static void Main(string[] args)
        {
            //Kør hovedmenuen
            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("\n=== Hovedmenu ===");
                Console.WriteLine("1) Indtast biloplysninger");
                Console.WriteLine("2) Kør en køretur");
                Console.WriteLine("3) Beregn prisen på en køretur");
                Console.WriteLine("4) Er kilometerstanden et palindrom?");
                Console.WriteLine("5) Udskriv biloplysninger");
                Console.WriteLine("6) Vis hele holdets biler");
                Console.WriteLine("7) Afslut");

                Console.Write("\nVælg en mulighed: ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        ReadCarDetails();
                        break;
                    case "2":
                        Console.Write("Indtast distance i km: ");
                        double input2 = Convert.ToDouble(Console.ReadLine());
                        Drive(input2);
                        break;
                    case "3":
                        Console.Write("Indtast distance i km: ");
                        double input3 = Convert.ToDouble(Console.ReadLine());
                        calculatedTripPrice = CalculateTripPrice(input3, fuelType);
                        break;
                    case "4":
                        IsPalindrome(Convert.ToInt32(kmCount));
                        break;
                    case "5":
                        ShowCarDetails();
                        break;
                    case "7":
                        isRunning = false;
                        Console.WriteLine("Afslutter... Farvel :)");
                        break;
                    default:
                        Console.WriteLine("Ugyldigt valg, prøv igen");
                        break;
                }
            }
        }





        // ================================ 1) Indtast biloplysninger ================================
        static void ReadCarDetails()
        {
            //Spørg om brugerens bil og sæt variablerne fra brugerens input
            Console.Write("Indtast bilmærke: ");
            brand = Console.ReadLine();
            Console.Write("Indtast bilmodel: ");
            model = Console.ReadLine();
            Console.Write("Indtast årgang: ");
            year = Convert.ToInt32(Console.ReadLine());
            Console.Write("Indtast geartype (A for automatisk, M for manuel): ");
            gearType = Console.ReadLine()[0];
            Console.Write("Hvilken type brændstof? (B for benzin, D for diesel): ");
            fuelType = Console.ReadLine()[0];
            Console.Write("Hvor langt kan bilen køre på en liter brændstof?: ");
            kmPerLiter = Convert.ToDouble(Console.ReadLine());
            Console.Write("Hvad er bilens nuværende kilometerstand?: ");
            kmCount = Convert.ToInt32(Console.ReadLine());
        }





        // ================================ 2) Kør en køretur ================================
        static void Drive(double distance) // Tager en lokal variabel "distance" ind som parameter-input i metoden
        {
            if (isEngineOn == true)
            {
                Console.WriteLine($"\nDen originale kilometerstand: {kmCount}");
                Console.WriteLine("Starter køretur...");

                kmCount += distance;

                Console.WriteLine($"Du har nu kørt: {distance}");
                Console.WriteLine($"Den nye kilometerstand er: {kmCount}");
            }
        }





        // ================================ 3) Udregn prisen på en køretur ================================
        static double CalculateTripPrice(double distance, char fuelType)
        {
            if (kmPerLiter == 0)          // Check om bilens kmPerLiter er 0, for at undgå et crash pga. division med 0
            {
                Console.WriteLine("Fejl: kmPerLiter kan ikke være 0!");
            }
            else if (fuelType != 'B' && fuelType != 'D') // Check om brændstofstypens værdi er noget andet end enten (B)enzin eller (D)iesel, og vis en fejl hvis den er ( != bruges som "ikke lig med", && bruges som "og" )
            {
                Console.WriteLine("Fejl: Ukendt brændstoftype!");
            }
            else                          // Hvis vi ikke støder på nogen af de fejl, når vi herned og kan begynde at udregne køreturens pris
            {
                double literPrice = 0;    // Vi ærklærer først bare lige hurtigt en lokal variabel for literprisen på brændstof,
                string fuelTypeStr = "";  // (og en string til fuelType for at kunne skrive f.eks. "Benzin" i stedet for "B", da fuelType er en char)
                switch (fuelType)         // og sætter deres værdi baseret på brændstofstypen, så vi er klar til at bruge literprisen i udregningen
                {
                    case 'B':
                        literPrice = 13.49;
                        fuelTypeStr = "Benzin";
                        break;
                    case 'D':
                        literPrice = 12.29;
                        fuelTypeStr = "Diesel";
                        break;
                }

                //Beregn prisen for køreturen
                double fuelNeeded = distance / kmPerLiter;     // Distancen som brugeren skrev ind divideret med bilens km per liter, giver os den nødvendige mængde brændstof for turen
                calculatedTripPrice = fuelNeeded * literPrice; // Nu ved vi hvor meget brændstof vi skal bruge, og vi kan finde turens pris ved at gange det med literprisen

                Console.WriteLine("\n================ Oplysninger om køreturen ================");

                Console.WriteLine($"Brændstoftype: " + fuelTypeStr);
                Console.WriteLine($"Bilen kører " + kmPerLiter + " km/l");
                Console.WriteLine($"Den originale kilometerstand var: {kmCount} km");

                kmCount += distance;
                
                Console.WriteLine($"Den nye kilometerstand er: {Math.Round(kmCount)} km"); // Bruger Math.Round() til at runde op/ned til nærmeste hele tal, fordi kilometertælleren skulle være en double, men jeg føler det måske lyder lidt fjollet med decimaler i en kilometertæller idk jeg kender ikke så meget til biler for at være helt ærlig :)
                Console.WriteLine($"Total brændstofudgift: {calculatedTripPrice} kr.");
            }
            return calculatedTripPrice;

        }





        // ================================ 4) Er kilometerstanden et palindrom? ================================
        static bool IsPalindrome(int kmInput)
        {
            string kmStr = kmInput.ToString(); // Kilometertælleren er en double, vi laver den om til en string for at kunne behandle tallene som tegn i stedet for tal

            if (kmStr.Length <= 2) // Hvis kilometertælleren er lig med eller kortere end 2 tegn, ved vi allerede at det må være et palindrom, så vi kan stoppe her allerede med "return = true;"
            {
                Console.WriteLine($"Ja, {kmCount} er et palindrom! :)");
                return true;
            }

            for (int i = 0; i < kmStr.Length / 2; i++) // for loop der tæller "i" op - fra 0 til længden af kmStr, divideret med 2, så den stopper når den når halvejs igennem kmStr
            {
                if (kmStr[i] != kmStr[kmStr.Length - (i + 1)]) // Starter ved index [i] og sammenligner det med index [længden af kmStr - (i + 1)], og tjekker om de ikke er lig med hinanden
                                                               // f.eks. første step sammenligner det første tegn med det sidste, næste step sammenligner det næst-første med det næst-sidste, indtil de møder hinanden på midten, fordi for-loopet stopper når det er nået til "kmStr.Length / 2"
                {
                    Console.WriteLine($"Nej, {kmCount} er ikke et palindrom!");
                    return false;
                }
            }
            Console.WriteLine($"Ja, {kmCount} er et palindrom! :)");
            return true;
        }





        // ================================ 5) Vis biloplysninger ================================
        static void ShowCarDetails()
        {
            string fuelTypeStr = "";
            switch (fuelType)
            {
                case 'B':
                    fuelTypeStr = "Benzin";
                    break;
                case 'D':
                    fuelTypeStr = "Diesel";
                    break;
            }

            Console.WriteLine("\n================ Oplysninger om din bil ================");

            Console.WriteLine($"Bilmærke: {brand}");
            Console.WriteLine($"Bilmodel: {model}");
            Console.WriteLine($"Årgang: {year}");
            Console.WriteLine($"Gear {gearType}");
            Console.WriteLine($"Brændstoftype: {fuelTypeStr}");
            Console.WriteLine($"Kører {kmPerLiter} km/l");
            Console.WriteLine($"Kilometerstand: {kmCount} km");
            Console.WriteLine($"Prisen for en typisk køretur er: {calculatedTripPrice} kr.");

            Console.WriteLine("========================================================\n");
        }





        // ================================ 6) Vis hele holdets biler ================================

        // Den her ved jeg ikke hvordan vi skulle lave uden at bruge andre klasser, som vi først begynder på at lære om om et par uger aahhh
    }

}
