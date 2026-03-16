using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace CarApp_michelle
{
    internal class Program
    {
        // lokale variable som bruges ved indtastning af data, og oprettelse af et car object
        static string brand;
        static string model;
        static int year;
        static char gearType;
        static FuelTypeEnum fuelType;
        static double kmPerLiter;
        static double kmCount;

        private static List<Car> carList = new List<Car>();
        private static int carIndex = -1;

        static void Main(string[] args)
        {
            

            double price = 0;

            ShowMenu();

            //Kør hovedmenuen
            bool isRunning = true;
            while (isRunning)
            {
                if (carIndex >= 0 && carList[carIndex] != null)
                {
                    Console.Write($"\nAktuel bil: {carIndex + 1} - {carList[carIndex].GetCarDetails()} ");
                }
                Console.Write("\nVælg en mulighed: ");
                string userInput = Console.ReadLine();
                
                switch (userInput)
                {
                    case "0": // alternativ til "1" - opretter en bil med default data og overskriver data med indput
                        carList.Add(new Car("-", "-", 1886, 'M', FuelTypeEnum.Benzin, 1, 0));
                        carIndex = carList.Count - 1;
                        ReadCarDetails(carList[carIndex]);
                        break;
                    case "1": // tager indtastede data og gemmer i lokale variable. Bruger disse til at oprette et car object
                        ReadCarDetails();
                        carList.Add(new Car(brand, model, year, gearType, fuelType, kmPerLiter, kmCount));
                        carIndex = carList.Count - 1;
                        break;
                    case "2":
                        if (carIndex >= 0 && carIndex < carList.Count)
                        {
                            Console.Write("Indtast distance i km: ");
                            double input2 = Convert.ToDouble(Console.ReadLine());
                            carList[carIndex].Drive(input2);
                        }
                        break;
                    case "3":
                        if (carIndex >= 0 && carIndex < carList.Count)
                        {
                            Console.Write("Indtast distance i km: ");
                            double input3 = Convert.ToDouble(Console.ReadLine());
                            price = carList[carIndex].CalculateTripPrice(input3, carList[carIndex].FuelType);
                        }
                        break;
                    case "4":
                        if (carIndex >= 0 && carIndex < carList.Count)
                        {
                            carList[carIndex].IsPalindrome(Convert.ToInt32(carList[carIndex].KmCount));
                        }
                        break;
                    case "5":
                        if (carIndex >= 0 && carIndex < carList.Count)
                        {
                            carList[carIndex].ShowCarDetails();
                        }
                        break;
                    case "6":
                        for (int i = 0; i < carList.Count; i++)
                        {
                            carList[i].ShowCarDetails();
                        }
                        break;
                    case "7": //
                        if (carIndex >= 0 && carIndex < carList.Count)
                        {
                            if (carList[carIndex].IsEngineOn)
                            {
                                carList[carIndex].IsEngineOn = false;
                                Console.WriteLine($"Motoren er slukket");
                            }
                            else
                            {
                                carList[carIndex].IsEngineOn = true;
                                Console.WriteLine($"Motoren er tændt... Vroom vroom og sårn");
                            }
                        }
                        break;

                    case "9":
                        for (int i = 0; i < carList.Count; i++)
                        {
                            Console.Write($"{i + 1}: {carList[i].Brand} {carList[i].Model} | ");
                        }
                        Console.Write("\nVælg bil: ");
                        int input9 = Convert.ToInt16(Console.ReadLine());
                        if (input9 > 0 && input9 <= carList.Count)
                        {
                            carIndex = input9 - 1;
                        }
                        else
                        {
                            Console.WriteLine($"Bil nr. {input9} findes ikke");
                        }
                        break;

                    case "m":
                        ShowMenu();
                        break;
                    case "x":
                        isRunning = false;
                        Console.WriteLine("Afslutter... Farvel :)");
                        break;
                    default:
                        Console.WriteLine("Ugyldigt valg, prøv igen");
                        break;
                }

                if (carIndex < 0 || carIndex >= carList.Count)
                {
                    Console.WriteLine("Ingen biler registreret");
                }

            }
        }



        static void SaveCars()
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "SavedCars.txt")))
            {
                foreach (Car car in carList)
                    outputFile.WriteLine($"{car.Brand},{car.Model},{car.Year},{car.FuelType},{car.GearType},{car.KmPerLiter},{car.KmCount}");
            }
            Console.WriteLine("Biler gemt i filen: SavedCars.txt");
        }



        static void LoadCars()
        {
            try
            {
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                using StreamReader reader = new StreamReader(Path.Combine(docPath, "SavedCars.txt"));

                carList.Clear();
                string text = "";
                do
                {
                    text = reader.ReadLine();  //.ReadToEnd();
                    if (text != null)
                    {
                        List<string> carText = new List<string>();
                        for (int i = 0; i < 6; i++)
                        {
                            carText.Add(text.Substring(0, text.IndexOf(',')));
                            text = text.Substring(text.IndexOf(',') + 1, text.Length - (text.IndexOf(',') + 1));
                        }
                        carText.Add(text);
                        carList.Add(new Car(brand, model, year, gearType, fuelType, kmPerLiter, kmCount));
                        carIndex = carList.Count - 1;
                        carList[carIndex].Brand = carText[0];
                        carList[carIndex].Model = carText[1];
                        carList[carIndex].Year = Convert.ToInt32(carText[2]);
                        carList[carIndex].FuelType = Enum.TryParse(carText[3], result: out FuelType);
                        carList[carIndex].GearType = Convert.ToChar(carText[4]);
                        carList[carIndex].KmPerLiter = Convert.ToDouble(carText[5]);
                        carList[carIndex].KmCount = Convert.ToDouble(carText[6]);
                    }

                } while (text != null);

            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }



        static void ShowMenu()
        {
            Console.WriteLine("\n=== Hovedmenu ===");
            Console.WriteLine("1) Indtast biloplysninger");
            Console.WriteLine("2) Kør en køretur");
            Console.WriteLine("3) Beregn prisen på en køretur");
            Console.WriteLine("4) Er kilometerstanden et palindrom?");
            Console.WriteLine("5) Udskriv biloplysninger");
            Console.WriteLine("6) Vis hele holdets biler");
            Console.WriteLine("7) Start/stop motoren");
            Console.WriteLine("9) Vælg bil");
            Console.WriteLine("m) Menu");
            Console.WriteLine("x) Afslut");
        }



        // ================================ 1) Indtast biloplysninger ================================
        static void ReadCarDetails() // løsning 1
        {
            // Spørg om brugerens bil og sæt variablerne fra brugerens input
            // Data gemmes i lokale variable som efterfølgende bruges til at oprette et car object 
            Console.Write("Indtast bilmærke: ");
            brand = Console.ReadLine();
            Console.Write("Indtast bilmodel: ");
            model = Console.ReadLine();
            Console.Write("Indtast årgang: ");
            year = Convert.ToInt32(Console.ReadLine());
            Console.Write("Indtast geartype (A for automatisk, M for manuel): ");
            gearType = Console.ReadLine()[0];
            Console.Write("Hvilken type brændstof? (B for benzin, D for diesel): ");
            char input = Console.ReadLine()[0];
            switch (input)
            {
                case 'b':
                case 'B':
                    fuelType = FuelTypeEnum.Benzin;
                    break;
                case 'd':
                case 'D':
                    fuelType = FuelTypeEnum.Diesel;
                    break;
            }
            Console.Write("Hvor langt kan bilen køre på en liter brændstof?: ");
            kmPerLiter = Convert.ToDouble(Console.ReadLine());
            Console.Write("Hvad er bilens nuværende kilometerstand?: ");
            kmCount = Convert.ToInt32(Console.ReadLine());
        }

        static void ReadCarDetails(Car car) // løsning 0
        {
            // Spørg om brugerens bil og sæt variablerne fra brugerens input
            // Parameteren 'car' er et car object, og dets data sættes direkte via set funktioner
            Console.Write("Indtast bilmærke: ");
            car.Brand = Console.ReadLine();
            Console.Write("Indtast bilmodel: ");
            car.Model = Console.ReadLine();
            Console.Write("Indtast årgang: ");
            car.Year = Convert.ToInt32(Console.ReadLine());
            Console.Write("Indtast geartype (A for automatisk, M for manuel): ");
            car.GearType = Console.ReadLine()[0];
            Console.Write("Hvilken type brændstof? (B for benzin, D for diesel): ");
            char input = Console.ReadLine()[0];
            switch (input)
            {
                case 'b':
                case 'B':
                    car.FuelType = FuelTypeEnum.Benzin;
                    break;

            }
            Console.Write("Hvor langt kan bilen køre på en liter brændstof?: ");
            car.KmPerLiter = Convert.ToDouble(Console.ReadLine());
            Console.Write("Hvad er bilens nuværende kilometerstand?: ");
            car.KmCount = Convert.ToInt32(Console.ReadLine());
        }

    }

}
