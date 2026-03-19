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
        private static List<Car> carList = new List<Car>();
        private static int carIndex = -1;

        static void Main(string[] args)
        {

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
                    case "1": // Indtast biloplysninger
                        carList.Add(ReadCarDetails());
                        carIndex = carList.Count - 1;
                        break;

                    case "2":
                        if (carIndex >= 0 && carIndex < carList.Count)
                        {
                            carList[carIndex].Drive(ReadTripDetails());
                        }
                        break;

                    case "3":
                        if (carIndex >= 0 && carIndex < carList.Count)
                        {
                            carList[carIndex].CalculateTripPrice();
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

                    case "7":
                        if (carIndex >= 0 && carIndex < carList.Count)
                        {
                            if (carList[carIndex].IsEngineOn)
                            {
                                carList[carIndex].TurnOffEngine();
                                Console.WriteLine($"Motoren er slukket");
                            }
                            else
                            {
                                carList[carIndex].TurnOnEngine();
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

                    case "s":
                    case "S":
                        SaveCars();
                        break;
                    case "l":
                    case "L":
                        LoadCars();
                        break;
                    case "m":
                    case "M":
                        ShowMenu();
                        break;
                    case "x":
                    case "X":
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
                        FuelTypeEnum fuelType = new FuelTypeEnum();
                        switch (carText[3])
                        {
                            case "b":
                            case "B":
                                fuelType = FuelTypeEnum.Benzin;
                                break;
                            case "d":
                            case "D":
                                fuelType = FuelTypeEnum.Diesel;
                                break;
                        }
                        carList.Add(new Car(carText[0], carText[1], Convert.ToInt32(carText[2]), Convert.ToChar(carText[4]), fuelType, Convert.ToDouble(carText[5]), Convert.ToDouble(carText[6])));
                        carIndex = carList.Count - 1;
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
            Console.WriteLine("S) Gem biler");
            Console.WriteLine("L) Hent biler");
            Console.WriteLine("M) Menu");
            Console.WriteLine("X) Afslut");
        }



        // ================================ 1) Indtast biloplysninger ================================
        static Car ReadCarDetails()
        {
            Console.Write("Indtast bilmærke: ");
            string brand = Console.ReadLine();
            Console.Write("Indtast bilmodel: ");
            string model = Console.ReadLine();
            Console.Write("Indtast årgang: ");
            int year = Convert.ToInt32(Console.ReadLine());
            Console.Write("Indtast geartype (A for automatisk, M for manuel): ");
            char gearType = Console.ReadLine()[0];
            Console.Write("Hvilken type brændstof? (B for benzin, D for diesel): ");
            FuelTypeEnum fuelType;
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
                case 'e':
                case 'E':
                    fuelType = FuelTypeEnum.Electric;
                    break;
                case 'h':
                case 'H':
                    fuelType = FuelTypeEnum.Hybrid;
                    break;
                default:
                    fuelType = FuelTypeEnum.Benzin;
                    break;
            }
            Console.Write("Hvor langt kan bilen køre på en liter brændstof?: ");
            double kmPerLiter = Convert.ToDouble(Console.ReadLine());
            Console.Write("Hvad er bilens nuværende kilometerstand?: ");
            double kmCount = Convert.ToInt32(Console.ReadLine());

            return new Car(brand, model, year, gearType, fuelType, kmPerLiter, kmCount);
        }

        // ================================ 1) Indtast biloplysninger ================================
        static Trip ReadTripDetails()
        {
            Console.Write("Indtast distance: ");
            double distance = Convert.ToDouble(Console.ReadLine());
            Console.Write("Indtast startdato og tid: ");
            DateTime startTime = DateTime.Parse(Console.ReadLine());
            Console.Write("Indtast sluttid: ");
            DateTime endTime = Convert.ToDateTime(Console.ReadLine());

            return new Trip(carList[carIndex], distance, startTime, endTime);
        }

    }

}