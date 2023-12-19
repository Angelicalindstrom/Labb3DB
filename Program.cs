using Labb3DB.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Labb3DB
{ // Angelica Lindström NET.23
    internal class Program
    {
        public static string connectionString = "Server=LAPTOP-VLADGQVE;Database=Labb2SKOLA;Trusted_Connection=true;Encrypt=false";
        static void Main(string[] args)
        {
            Run();
        }

        // Run metod med Huvudmeny och Switch/case
        public static void Run()
        {
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Välkommen till");
            while (true)
            {
                // HUVUDMENY
                Console.ResetColor();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\nKODKNÄCKARNAS GYMNASIESKOLA\n");
                Console.WriteLine("\n\nHuvudmeny - Kodknäckarnas Gymnasieskola");
                Console.ResetColor();
                Console.WriteLine("1. Se alla anställda");
                Console.WriteLine("2. Se alla elever SorteringsAlternativ");
                Console.WriteLine("3. Hämta alla elever i en viss klass");
                Console.WriteLine("4. Alla kurser och det snittbetyg som elever fått");
                Console.WriteLine("5. Max, Min och snittbetyg");
                Console.WriteLine("6. Lägg till en ny Elev/Anställd");
                Console.WriteLine("0. Avsluta");
                
                // Ifsats som ser över att AnvändarInput är korrekt. Om inte skickas felmeddelande 
                if (!int.TryParse(Console.ReadLine(), out int userChoice))
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Felaktig inmatning. var god försök igen. Ange ett heltal.");
                    Console.ResetColor();
                    continue;
                }

                switch (userChoice)
                {

                    case 1:
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        UserEmp();
                        Console.ResetColor();
                        break;
                    case 2:
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        MenuRunAllStudentsOrder();
                        Console.ResetColor();
                        break;
                    case 3:
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        GetStudentsByClassID();
                        Console.ResetColor();
                        break;
                    case 4:
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        GradesLastMonthRUN();
                        Console.ResetColor();
                        break;
                    case 5:
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        DisplayGradeMenu();
                        Console.ResetColor();
                        break;
                    case 6:
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        DisplayMenu();
                        Console.ResetColor();
                        break;
                    case 0:
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Programmet avslutas!\nTryck på vilken tangent som helst för att avsluta");
                        Console.ReadKey();
                        return; // Avsluta hela programmet + meddelande om att Programmet kommer avslutas
                    default:
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        Console.ResetColor(); 
                        break; //Felhantering och felmeddelande med tydligt meddelande
                }
            }
        }


        //****************************************************************************************************
        // Uppgift 1
        //****************************************************************************************************
        //Hämta personal
        //Användaren får välja om denna vill se alla anställda, eller bara inom en av
        //kategorierna så som ex lärare, admin osv§

        //Användaren får välja om denna vill se alla anställda,
        //eller bara inom en av kategorierna så som ex lärar
        public static void UserEmp()
        {
            // SqlConnection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Kopplar upp till databasen
                    connection.Open();

                    int userInput;
                    // while/loop för att köra meny till användare väljer 0 = återgå till huvudmeny
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("---HÄMTA ALL PERSONAL---\n");
                        Console.WriteLine("\n1. Se alla anställda i alla kategorier" +
                            "\n2. Söka efter en specifik kategori av anställda\n0. Återgå till Huvudmenyn");
                        // sparar input från användare i string
                        string UserInputEmployee = Console.ReadLine();

                        // If-sats med else, Kollar så att input är godkänd int
                        if (int.TryParse(UserInputEmployee, out userInput))
                        {
                            switch (userInput)
                            {
                                // Case 1: Visar alla anställda
                                case 1:
                                    // SQL kommando för att hämta all personal och deras yrkestitel(professionName)
                                    SqlCommand command = new SqlCommand(
                                        "SELECT EmployeeID, FirstName, LastName, ProfessionName " +
                                        "FROM Employees " +
                                        "JOIN EmployeeProfessions ON Employees.EmployeeID = EmployeeProfessions.FKEmployeeID " +
                                        "JOIN Professions ON EmployeeProfessions.FKProfessionTitleID = Professions.ProfessionTitleID",
                                        connection);

                                    // Exekverar kommandot och skriver ut resultaten av datan
                                    using (SqlDataReader reader = command.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            // visar datan
                                            Console.WriteLine("\nAnställningsID: {0}\nFörnamn: {1}\nEfternamn: {2}\nYrkestitel: {3}\n-----------------------------------",
                                                            reader["EmployeeID"],
                                                            reader["FirstName"],
                                                            reader["LastName"],
                                                            reader["ProfessionName"]);
                                        }

                                        Console.WriteLine("Tryck på vilken tangent som helst för att återgå till menyn");
                                    }
                                    Console.ReadKey();
                                    break;

                                // Case 2: söker efter anställda i ett specifikt yrke
                                case 2:

                                    Console.WriteLine("\nVälj ett specifikt yrke:\n1.Rektor\n2.Admin\n3.Skol Sköterska" +
                                        "\n4.Vaktmästare\n5.Kökspersonal\n6.Städare\n7.Lärare" +
                                        "\nVar god välj en kategori genom att valja en siffra: ");
                                    // läser val av kategri ifrån användare och sparar i string
                                    string categoryInput = Console.ReadLine();

                                    // Kollar så kategorivalet är en godkänd int
                                    if (int.TryParse(categoryInput, out int categoryNumber))
                                    {
                                        switch (categoryNumber)
                                        {
                                            // Olika kategorier
                                            case 1:
                                                // SQL kommando som hämtar anställda i Rektor 'Principal' kategorin
                                                SqlCommand command1 = new SqlCommand(
                                                    "SELECT EmployeeID, FirstName, LastName, ProfessionName " +
                                                    "FROM Employees " +
                                                    "JOIN EmployeeProfessions ON Employees.EmployeeID = EmployeeProfessions.FKEmployeeID " +
                                                    "JOIN Professions ON EmployeeProfessions.FKProfessionTitleID = Professions.ProfessionTitleID " +
                                                    "WHERE ProfessionName = 'Principal'", connection);

                                                // Exekverar kommantot och skriver ut resultatet av datan
                                                using (SqlDataReader reader = command1.ExecuteReader())
                                                {
                                                    while (reader.Read())
                                                    {
                                                        // Skriver ut anställd info i dne specifika kategorin
                                                        Console.WriteLine("\nAnställningsID: {0}\nFörnamn: {1}\nEfternamn: {2}\nYrkestitel: {3}\n-----------------------------------",
                                                            reader["EmployeeID"],
                                                            reader["FirstName"],
                                                            reader["LastName"],
                                                            reader["ProfessionName"]);

                                                        Console.WriteLine("\nTryck ENTER för att återgå till Menyn över anställda: ");

                                                    }
                                                }
                                                Console.ReadKey();
                                                break;

                                            case 2:
                                                // SQL kommando som hämtar anställda i 'Admin' kategorin
                                                SqlCommand command2 = new SqlCommand(
                                                    "SELECT EmployeeID, FirstName, LastName, ProfessionName " +
                                                    "FROM Employees " +
                                                    "JOIN EmployeeProfessions ON Employees.EmployeeID = EmployeeProfessions.FKEmployeeID " +
                                                    "JOIN Professions ON EmployeeProfessions.FKProfessionTitleID = Professions.ProfessionTitleID " +
                                                    "WHERE ProfessionName = 'Admin'", connection);

                                                // Exekverar kommantot och skriver ut resultatet av datan
                                                using (SqlDataReader reader = command2.ExecuteReader())
                                                {
                                                    while (reader.Read())
                                                    {
                                                        // Skriver ut anställd info i dne specifika kategorin
                                                        Console.WriteLine("\nAnställningsID: {0}\nFörnamn: {1}\nEfternamn: {2}" +
                                                            "\nYrkestitel: {3}\n-----------------------------------",
                                                            reader["EmployeeID"],
                                                            reader["FirstName"],
                                                            reader["LastName"],
                                                            reader["ProfessionName"]);

                                                        Console.WriteLine("\nTryck ENTER för att återgå till Menyn över anställda: ");
                                                    }
                                                }
                                                Console.ReadKey();
                                                break;

                                            case 3: // samma som ovan men för Nurse / SkolSköterska
                                                SqlCommand command3 = new SqlCommand(
                                            "SELECT EmployeeID, FirstName, LastName, ProfessionName " +
                                            "FROM Employees " +
                                            "JOIN EmployeeProfessions ON Employees.EmployeeID = EmployeeProfessions.FKEmployeeID " +
                                            "JOIN Professions ON EmployeeProfessions.FKProfessionTitleID = Professions.ProfessionTitleID " +
                                            "WHERE ProfessionName = 'SchoolNurse'", connection);

                                                using (SqlDataReader reader = command3.ExecuteReader())
                                                {
                                                    while (reader.Read())
                                                    {
                                                        Console.WriteLine("\nAnställningsID: {0}\nFörnamn: {1}\nEfternamn: {2}" +
                                                            "\nYrkestitel: {3}\n-----------------------------------",
                                                            reader["EmployeeID"],
                                                            reader["FirstName"],
                                                            reader["LastName"],
                                                            reader["ProfessionName"]);

                                                        Console.WriteLine("\nTryck ENTER för att återgå till Menyn över anställda: ");
                                                    }
                                                }
                                                Console.ReadKey();
                                                break;

                                            case 4:// samma som ovan men för GroundKeeper / Vaktmästare
                                                SqlCommand command4 = new SqlCommand(
                                            "SELECT EmployeeID, FirstName, LastName, ProfessionName " +
                                            "FROM Employees " +
                                            "JOIN EmployeeProfessions ON Employees.EmployeeID = EmployeeProfessions.FKEmployeeID " +
                                            "JOIN Professions ON EmployeeProfessions.FKProfessionTitleID = Professions.ProfessionTitleID " +
                                            "WHERE ProfessionName = 'GroundKeeper'", connection);

                                                using (SqlDataReader reader = command4.ExecuteReader())
                                                {
                                                    while (reader.Read())
                                                    {
                                                        Console.WriteLine("\nAnställningsID: {0}\nFörnamn: {1}\nEfternamn: {2}" +
                                                            "\nYrkestitel: {3}\n-----------------------------------",
                                                            reader["EmployeeID"],
                                                            reader["FirstName"],
                                                            reader["LastName"],
                                                            reader["ProfessionName"]);

                                                        Console.WriteLine("\nTryck ENTER för att återgå till Menyn över anställda: ");
                                                    }
                                                }
                                                Console.ReadKey();
                                                break;

                                            case 5:// samma som ovan men för FoodService / Kökspersonal
                                                SqlCommand command5 = new SqlCommand(
                                            "SELECT EmployeeID, FirstName, LastName, ProfessionName " +
                                            "FROM Employees " +
                                            "JOIN EmployeeProfessions ON Employees.EmployeeID = EmployeeProfessions.FKEmployeeID " +
                                            "JOIN Professions ON EmployeeProfessions.FKProfessionTitleID = Professions.ProfessionTitleID " +
                                            "WHERE ProfessionName = 'FoodService'", connection);

                                                using (SqlDataReader reader = command5.ExecuteReader())
                                                {
                                                    while (reader.Read())
                                                    {
                                                        Console.WriteLine("\nAnställningsID: {0}\nFörnamn: {1}\nEfternamn: {2}" +
                                                            "\nYrkestitel: {3}\n-----------------------------------",
                                                            reader["EmployeeID"],
                                                            reader["FirstName"],
                                                            reader["LastName"],
                                                            reader["ProfessionName"]);

                                                        Console.WriteLine("\nTryck ENTER för att återgå till Menyn över anställda: ");
                                                    }
                                                }
                                                Console.ReadKey();
                                                break;

                                            case 6:// samma som ovan men för Custodian / Städare
                                                SqlCommand command6 = new SqlCommand(
                                            "SELECT EmployeeID, FirstName, LastName, ProfessionName " +
                                            "FROM Employees " +
                                            "JOIN EmployeeProfessions ON Employees.EmployeeID = EmployeeProfessions.FKEmployeeID " +
                                            "JOIN Professions ON EmployeeProfessions.FKProfessionTitleID = Professions.ProfessionTitleID " +
                                            "WHERE ProfessionName = 'Custodian'", connection);

                                                using (SqlDataReader reader = command6.ExecuteReader())
                                                {
                                                    while (reader.Read())
                                                    {
                                                        Console.WriteLine("\nAnställningsID: {0}\nFörnamn: {1}\nEfternamn: {2}" +
                                                            "\nYrkestitel: {3}\n-----------------------------------",
                                                            reader["EmployeeID"],
                                                            reader["FirstName"],
                                                            reader["LastName"],
                                                            reader["ProfessionName"]);

                                                        Console.WriteLine("\nTryck ENTER för att återgå till Menyn över anställda: ");
                                                    }
                                                }
                                                Console.ReadKey();
                                                break;

                                            case 7:// samma som ovan men för Teacher / Lärare
                                                SqlCommand command7 = new SqlCommand(
                                            "SELECT EmployeeID, FirstName, LastName, ProfessionName " +
                                            "FROM Employees " +
                                            "JOIN EmployeeProfessions ON Employees.EmployeeID = EmployeeProfessions.FKEmployeeID " +
                                            "JOIN Professions ON EmployeeProfessions.FKProfessionTitleID = Professions.ProfessionTitleID " +
                                            "WHERE ProfessionName = 'Teacher'", connection);

                                                using (SqlDataReader reader = command7.ExecuteReader())
                                                {
                                                    while (reader.Read())
                                                    {
                                                        Console.WriteLine("\nAnställningsID: {0}\nFörnamn: {1}\nEfternamn: {2}" +
                                                            "\nYrkestitel: {3}\n-----------------------------------",
                                                            reader["EmployeeID"],
                                                            reader["FirstName"],
                                                            reader["LastName"],
                                                            reader["ProfessionName"]);

                                                        Console.WriteLine("\nTryck ENTER för att återgå till Menyn över anställda: ");
                                                    }
                                                }
                                                Console.ReadKey();
                                                break;

                                            // felhantering
                                            default:
                                                Console.WriteLine("Felaktigt nummer !");
                                                break;
                                        }
                                    }
                                    else// felhantering
                                    {
                                        Console.WriteLine("Felaktigt nummer, var god välj ett nytt nummer i menyn");
                                    }
                                    break;
                                case 0:// avsluta och återgå till Huvudmeny
                                    Console.WriteLine("Menyn avslutas och du återgår till Huvudmenyn");
                                    Console.ResetColor();
                                    Console.Clear();
                                    return;

                                default:// Felhantering
                                    Console.WriteLine("Felaktig input, välj ett alternativ, 1, 2 eller 3.");
                                    break;
                            }
                        }
                        else //felhantering if-Else
                        {
                            Console.WriteLine("Fekatig input, välj en siffra");
                        }


                    }
                }
                catch (Exception ex) // felmeddelande
                {
                    Console.WriteLine(ex.Message);
                }
                finally // stänger uppkopplingen till databasen
                {
                    // Close the database connection in the finally block
                    connection.Close();
                }
            }
        }


        //****************************************************************************************************
        // uppgift 2
        //****************************************************************************************************
        //Hämta alla elever (ska lösas med Entity framework)
        //Användaren får välja om de vill se eleverna sorterade på för-
        //eller efternamn och om det ska vara stigande eller fallande sortering.
        //public static void GetAllStudentsOrder() // SVENSKA
        //{
        public static void MenuRunAllStudentsOrder()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Elever - SorteringsAlternativ:\n");
                Console.WriteLine("1. Sortera efter förnamn (A-Ö)");
                Console.WriteLine("2. Sortera efter förnamn (Ö-A)");
                Console.WriteLine("3. Sortera efter efternamn (A-Ö)");
                Console.WriteLine("4. Sortera efter efternamn (Ö-A)");
                Console.WriteLine("5. Visa elever i ordningen de finns i databasen");
                Console.WriteLine("0. Återgå till huvudmenyn");

                if (int.TryParse(Console.ReadLine(), out int studentSortChoice))
                {
                    switch (studentSortChoice)
                    {
                        case 1:
                            GetAllStudentsOrderedByFirstName();
                            break;
                        case 2:
                            GetAllStudentsOrderedByFirstNameDescending();
                            break;
                        case 3:
                            GetAllStudentsOrderedByLastName();
                            break;
                        case 4:
                            GetAllStudentsOrderedByLastNameDescending();
                            break;
                        case 5:
                            GetAllStudentsInOrder();
                            break;
                        case 0:
                            Console.Clear();
                            Console.ResetColor();
                            return;
                        default:
                            Console.WriteLine("Ogiltigt val. Försök igen.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Felaktigt inmatning. Ange en siffra.");
                }
            }
        }
        // MENY UPPGIFT 2
        public static void GetAllStudentsInOrder()
        {
            Console.Clear();
            using Labb2SKOLAContext labb2Skola = new Labb2SKOLAContext();

            Console.WriteLine("\n----------ALLA ELEVER I VÅR DATABAS i databasOrdning-----------");
            var studentList = labb2Skola.Students.ToList();
            var showAllStudents = labb2Skola.Students.ToList();


            foreach (var student in showAllStudents)
            {

                Console.WriteLine($"StudentID: {student.StudentId}, Namn: {student.FirstName} {student.LastName}");
            }
            Console.WriteLine("");
        }
        // ALLA Studenter i ordning efter databasen
        public static void GetAllStudentsOrderedByFirstName()
        {// SORTERA MED A, förnamn som FÖRSTA bokstav i Listan
            Console.Clear();
            using Labb2SKOLAContext labb2Skola = new Labb2SKOLAContext();

            Console.WriteLine("\n----------A FÖRST FÖRNAMN-----------");
            var sortedStudents = labb2Skola.Students
            .OrderBy(student => student.FirstName)
            .ToList();

            foreach (var student in sortedStudents)
            {
                Console.WriteLine($"Student ID: {student.StudentId}, Name: {student.FirstName} {student.LastName}");
            }
            Console.WriteLine("");
        }
        // Förnamn A Först
        public static void GetAllStudentsOrderedByFirstNameDescending()
        {// SORTERA MED A, Förnamn som SISTA bokstav i listan
            Console.Clear();
            using Labb2SKOLAContext labb2Skola = new Labb2SKOLAContext();

            Console.WriteLine("\n-----------****A SIST FÖRNAMN*****----------");
            var sortedStudentsALastList = labb2Skola.Students
            .OrderByDescending(student => student.FirstName)
            .ToList();

            foreach (var student in sortedStudentsALastList)
            {
                Console.WriteLine($"Student ID: {student.StudentId}, Name: {student.FirstName} {student.LastName}");
            }
            Console.WriteLine("");
        }
        // Förnamn A SIST
        public static void GetAllStudentsOrderedByLastName()
        { // SORTERA MED A SOM FÖRSTA BOKSTAV I EFETNAMN
            Console.Clear();
            using Labb2SKOLAContext labb2Skola = new Labb2SKOLAContext();

            Console.WriteLine("\n----------!!A FÖRST EFTERNAMN!!-----------");
            var sortLastNameStudents1 = labb2Skola.Students
            .OrderBy(student => student.LastName)
            .ToList();

            foreach (var student in sortLastNameStudents1)
            {
                // Access student properties as needed
                Console.WriteLine($"Student ID: {student.StudentId}, Name: {student.LastName} {student.FirstName}");
            }
            Console.WriteLine("");
        }
        // ÉFTERNAMN A Först
        public static void GetAllStudentsOrderedByLastNameDescending()
        {// SORTERA MED A, EFTERNAMN som SISTA bokstav i listan
            Console.Clear();
            using Labb2SKOLAContext labb2Skola = new Labb2SKOLAContext();

            Console.WriteLine("\n----------!!A SIST EFTERNAMN!!-----------");
            var sortLastNameStudents2 = labb2Skola.Students
            .OrderByDescending(student => student.LastName)
            .ToList();

            foreach (var student in sortLastNameStudents2)
            {
                // Access student properties as needed
                Console.WriteLine($"Student ID: {student.StudentId}, Name: {student.LastName} {student.FirstName}");
            }
            Console.WriteLine("");
        }
        // ÉFTERNAMN A SIST


        //****************************************************************************************************
        // uppgift 3
        //****************************************************************************************************
        // Hämta alla elever i en viss klass (Entity framework)
        //Användaren ska först få se en lista med alla klasser som finns,
        //sedan får användaren välja en av klasserna och då skrivs alla elever i den klassen ut.
        static void GetStudentsByClassID()
        {
            using (var labb2skola = new Labb2SKOLAContext())
            {
                try
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("---ALLA KLASSER---\n");
                        Console.WriteLine("Välj en klass:");
                        var classes = labb2skola.Classes.ToList();
                        foreach (var ClassName in classes)
                        {
                            Console.WriteLine($"KlassID: {ClassName.ClassId}, Klassnamn: {ClassName.ClassName}");
                        }

                        Console.Write("Ange KlassID för den klass du vill se elever för (eller 0 för att avsluta): ");
                        if (!int.TryParse(Console.ReadLine(), out int selectedClassID))
                        {
                            Console.WriteLine("Ogiltig inmatning för KlassID. Avslutar programmet.");
                            return;
                        }

                        if (selectedClassID == 0)
                        {
                            Console.WriteLine("Avslutar programmet.");
                            Console.Clear();
                            return;
                        }

                        // Call a method to display students in the selected class
                        DisplayStudentsInClass(labb2skola, selectedClassID);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        static void DisplayStudentsInClass(Labb2SKOLAContext context, int classID)
        {
            var students = context.ClassStudents
                .Where(cs => cs.FkclassId == classID)
                .Join(
                    context.Students,
                    cs => cs.FkstudentId,
                    s => s.StudentId,
                    (cs, s) => s
                    // använder lambda operator
                )
                .OrderBy(s => s.FirstName)
                .ToList();

            Console.Clear();
            Console.WriteLine($"---ELEVER I KLASSEN---\n");


            foreach (var student in students)
            {
                Console.WriteLine($"ElevID: {student.StudentId}, Namn: {student.FirstName} {student.LastName}");
            }
            Console.WriteLine($"Översikt klassID: {classID}");

            Console.WriteLine("\nTryck på en tangent för att fortsätta...");
            Console.ReadKey();
        }


        //****************************************************************************************************
        //uppgift 4
        //****************************************************************************************************
        // Hämta en lista med alla kurser och det snittbetyg som eleverna fått
        // på den kursen samt det högsta och lägsta betyget som någon fått i kursen

        static void GradesLastMonthRUN()
        {
            Console.Clear();
            try
            {
                DisplayGradesLastMonth();
                Console.WriteLine("Tryck på valfri tangent för att återgå till huvudmeny");
                Console.ReadKey();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel uppstod: {ex.Message}");
            }
        }
        static void DisplayGradesLastMonth()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT 
                                S.FirstName + ' ' + S.LastName AS StudentName,
                                Su.SubjectName,
                                E.Grade,
                                EmP.FirstName + ' ' + EmP.LastName AS EmployeeName,
                                E.Date

                                FROM Enrollments E
                                JOIN Students S ON E.FKStudentID = S.StudentID
                                JOIN Subjects Su ON E.FKSubjectID = Su.SubjectID
                                JOIN Employees EmP ON E.FKEmployeeID = EmP.EmployeeID
                                WHERE E.Date >= DATEADD(month, -1, GETDATE())";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("ALLA BETYG SOM SATTS DEN SENASTE MÅNADEN:\n");
                        Console.WriteLine("\n-----------------------------------");

                        while (reader.Read())
                        {
                            string stuName = reader["StudentName"].ToString();
                            string subjectName = reader["SubjectName"].ToString();
                            string grade = reader["Grade"].ToString();
                            string empName = reader["EmployeeName"].ToString();
                            DateTime date = Convert.ToDateTime(reader["Date"].ToString());
                            // Formatera Date för att endast vista YYYY-MM-DD
                            string formattedDate = date.ToString("yyyy-MM-dd");


                            Console.WriteLine($"Student Namn: {stuName}\nÄmne: {subjectName}\n" +
                                $"Betyg: {grade}\nBetygsatt av Lärare: {empName}\nSignerat Datum:{formattedDate}");
                            Console.WriteLine("\n-----------------------------------");
                        }
                    }
                }
            } // SVENSKA


        }


        //****************************************************************************************************
        // Uppgift 5
        //****************************************************************************************************
        //Hämta en lista med alla kurser och det snittbetyg som eleverna
        //fått på den kursen samt det högsta och lägsta betyget som någon fått i kursen
        //Här får användaren direkt upp en lista med alla kurser i databasen,
        //snittbetyget samt det högsta och lägsta betyget för varje kurs.

        // Metod för att visa Max, Min och snitt betyg, hantera användarval
        public static void DisplayGradeMenu()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("BETYG MENY, MAX, MIN, SNITTBETYG\n");
                Console.WriteLine("Välj ett alternativ:");
                Console.WriteLine("1. Max Betyg");
                Console.WriteLine("2. Min Betyg");
                Console.WriteLine("3. Medel betyg för en specifik kurs");
                Console.WriteLine("0. Återgå till Huvudmenyn");

                // Användares inputval och tryParse
                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Ogiltlig input. Var god välj ett giltligt nummer.");
                    continue;
                }

                // OM 0, tömmer consolen och återgår till Huvudmeny
                if (choice == 0)
                {
                    Console.Clear();
                    return;
                }

                string query = string.Empty;


                // Switch för att hantera olika användarval
                switch (choice)
                {
                    case 1:
                        // Max betyg
                        query = @"SELECT
                            Subjects.SubjectID,
                            Subjects.SubjectName,
                            MAX(Enrollments.Grade) AS SubjectMaximumGrade
                        FROM
                            Subjects
                            LEFT JOIN Enrollments ON Subjects.SubjectID = Enrollments.FKSubjectID
                        GROUP BY
                            Subjects.SubjectID, Subjects.SubjectName
                        ORDER BY
                            Subjects.SubjectID";
                        break;

                    case 2:
                        // Min Betyg
                        query = @"SELECT
                            Subjects.SubjectID,
                            Subjects.SubjectName,
                            MIN(Enrollments.Grade) AS SubjectMinimumGrade
                        FROM
                            Subjects
                            LEFT JOIN Enrollments ON Subjects.SubjectID = Enrollments.FKSubjectID
                        GROUP BY
                            Subjects.SubjectID, Subjects.SubjectName
                        ORDER BY
                            Subjects.SubjectID";
                        break;

                    case 3:
                        // SnittBetyg för en kurs i taget
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("Välj (1-5) för att se snittbetyg i en specifik kurs:");
                            Console.WriteLine("1. C#");
                            Console.WriteLine("2. OOP");
                            Console.WriteLine("3. AI");
                            Console.WriteLine("4. Frontend");
                            Console.WriteLine("5. DevOps");

                            // Kursval av användare med TryParse
                            int courseChoice;
                            if (!int.TryParse(Console.ReadLine(), out courseChoice))
                            {
                                Console.WriteLine("Ogiltlig input. Var god välj ett giltligt nummer.");
                                return;
                            }

                            // Nestlad switch/case för att hantera kurs val
                            // Kallar på GetCourseAverageGradeQuery Metoden som hanterar alla string val
                            // och letar efter den specifika datan av snitt betyg i den kursen
                            // String Query för att leta efter rätt kurs
                            switch (courseChoice)
                            {
                                case 1:
                                    query = GetCourseAverageGradeQuery("C#");
                                    break;

                                case 2:
                                    query = GetCourseAverageGradeQuery("OOP");
                                    break;

                                case 3:
                                    query = GetCourseAverageGradeQuery("AI");
                                    break;

                                case 4:
                                    query = GetCourseAverageGradeQuery("Frontend");
                                    break;

                                case 5:
                                    query = GetCourseAverageGradeQuery("DevOps");
                                    break;

                                default:
                                    Console.WriteLine("Ogiltligt val. Välj ett nummer för en specifik kurs.");
                                    return;
                            }
                            break;
                        }
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Felaktig input. Var god gör ett nytt försök.");
                        return;
                }

                // Kopplar upp till databasen och Exikverar SQL Query
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.Clear();
                            Console.WriteLine("\n------------------------------------------------------------------------------------------");

                            // HAntera och visar resultatenn
                            while (reader.Read())
                            {
                                string subjectName = reader["SubjectName"].ToString();

                                switch (choice)
                                {
                                    case 1:
                                        // Visar Max betyg

                                        while (true)
                                        {

                                            double subjectMaximumGrade = Convert.ToDouble(reader["SubjectMaximumGrade"]);
                                            Console.WriteLine($"Ämne: {subjectName}\nMax Betyg: {subjectMaximumGrade}");
                                            break;

                                        }
                                        Console.WriteLine("***Max Betyg***");
                                        break;

                                    case 2:
                                        // Visar Min betyg

                                        while (true)
                                        {
                                            double subjectMinimumGrade = Convert.ToDouble(reader["SubjectMinimumGrade"]);
                                            Console.WriteLine($"Ämne: {subjectName}\nMin Betyg: {subjectMinimumGrade}");
                                            break;
                                        }
                                        Console.WriteLine("***Min Betyg***");
                                        break;

                                    case 3:
                                        // Visar snitt betyg för en kurs
                                        while (true)
                                        {
                                            double subjectAverageGrade = Convert.ToDouble(reader["SubjectAverageGrade"]);
                                            Console.WriteLine($"Ämne: {subjectName}\nSnitt Betyg: {subjectAverageGrade}");
                                            break;
                                        }
                                        Console.WriteLine("***Snitt Betyg - Specifik Kurs***");
                                        break;
                                }

                                Console.WriteLine("\n------------------------------------------------------------------------------------------");
                            }
                        }
                    }
                }
            }

        }
        // String metod för att generera SQL Query för snitt betyg i specifik kurs Case 3
        public static string GetCourseAverageGradeQuery(string courseName)
        {
            // string courseName, söker efter rätt kurs/subject för att visa den rätta datan i Case 3 i DisplayGradeMenu()
            return $@"SELECT
                    Subjects.SubjectID,
                    Subjects.SubjectName,
                    ROUND(AVG(CAST(Enrollments.Grade AS FLOAT)), 2) AS SubjectAverageGrade
                FROM
                    Subjects
                    LEFT JOIN Enrollments ON Subjects.SubjectID = Enrollments.FKSubjectID
                WHERE
                    Subjects.SubjectName = '{courseName}'
                GROUP BY
                    Subjects.SubjectID, Subjects.SubjectName
                ORDER BY
                    Subjects.SubjectID";
        }



        //****************************************************************************************************
        //Uppgift 6   // Uppgift 7     // kopplas ihop!!  //(ENTITY FRAMEWORK) 
        //****************************************************************************************************
        //6.
        //Lägga till nya elever 
        //Användaren får möjlighet att mata in uppgifter om en ny elev och den datan sparas då ner i databasen.
        //7.
        //Lägga till ny personal
        //Användaren får möjlighet att mata in uppgifter om en ny anställd och den datan sparas då ner i databasen.


        // Display Menu
        public static void DisplayMenu()
        {


            Console.Clear();
            bool exitProgram = false;

            while (!exitProgram)
            {

                Console.WriteLine("1. Lägg till ny elev");
                Console.WriteLine("2. lägg till ny anställd");
                Console.WriteLine("0. Avsluta programmet");

                Console.Write("\nVälj ett alternativ (1-2): ");
                char choice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch (char.ToUpper(choice))
                {
                    case '1':
                        AddNewStudent();
                        break;
                    case '2':
                        AddNewTeacher();
                        break;
                    case '0':
                        exitProgram = true;
                        break;

                    default:
                        Console.WriteLine("Ogiltigt alternativ. Vänligen välj igen.");
                        break;
                }

            }

        }
        // Uppgift 6  
        public static void AddNewStudent()
        {
            using (var dbContext = new Labb2SKOLAContext())
            {
                Console.WriteLine("Ange information för den nya eleven:");

                Console.Write("Förnamn: ");
                string firstName = Console.ReadLine();

                Console.Write("Efternamn: ");
                string lastName = Console.ReadLine();

                Console.Write("Födelsedatum (ÅÅÅÅ-MM-DD): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime birthDate))
                {
                    Console.WriteLine("Ogiltigt datumformat. Lägg till elev avbruten.");
                    return;
                }

                Console.Write("Telefonnummer: ");
                string phone = Console.ReadLine();

                // Skapa en ny elev och lägg till i databasen
                var newStudent = new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                    BirthDate = birthDate,
                    Phone = phone
                };

                dbContext.Students.Add(newStudent);
                dbContext.SaveChanges();

                Console.WriteLine("Ny elev tillagd i databasen!");
            }
        }
        // Uppgift 7        FRÅGA OM FORENKEY?? REIDAR? KAN EJ KOPPLA TILL VALT YRKE*************************************************************
        public static void AddNewTeacher()
        {
            using (var dbContext = new Labb2SKOLAContext())
            {
                Console.WriteLine("Ange information för den nya anställda:");

                Console.Write("Förnamn: ");
                string firstName = Console.ReadLine();

                Console.Write("Efternamn: ");
                string lastName = Console.ReadLine();

                // Skapa en ny lärare och lägg till i databasen
                var newEmployee = new Employee
                {
                    FirstName = firstName,
                    LastName = lastName
                };
                // lägger till ny
                dbContext.Employees.Add(newEmployee);
                dbContext.SaveChanges();
                Console.WriteLine($"\nNy anställd tillagd i databasen!\n Förnamn : {firstName}\nEfternamn : {lastName}");

                // ProfessionID
                Console.WriteLine("\nVälj ett yrke för den nya anställda du just skrev in");
                Console.WriteLine("Meny över alla Yrken :\n");
                Console.WriteLine("Välj en siffra:");
                Console.WriteLine("1. Rektor");
                Console.WriteLine("2. Admin");
                Console.WriteLine("3. Skolsköterska");
                Console.WriteLine("4. Vaktmästare");
                Console.WriteLine("5. Kökspersonal");
                Console.WriteLine("6. Städare");
                Console.WriteLine("7. Lärare");

                try
                {
                    int chosenProfessionID;
                    string chosen = "";

                    // Låter användaren välja yrke
                    do
                    {
                        Console.Write("Ange yrkets nummer: ");
                        if (int.TryParse(Console.ReadLine(), out chosenProfessionID))
                        {
                            switch (chosenProfessionID)
                            {
                                case 1:
                                    chosenProfessionID = 5000;
                                    chosen = "Rektor";
                                    break;
                                case 2:
                                    chosenProfessionID = 5001;
                                    chosen = "Admin";
                                    break;
                                case 3:
                                    chosenProfessionID = 5002;
                                    chosen = "Skolsköterska";
                                    break;
                                case 4:
                                    chosenProfessionID = 5003;
                                    chosen = "Vaktmästare";
                                    break;
                                case 5:
                                    chosenProfessionID = 5004;
                                    chosen = "Kökspersonal";
                                    break;
                                case 6:
                                    chosenProfessionID = 5005;
                                    chosen = "Städare";
                                    break;
                                case 7:
                                    chosenProfessionID = 5006;
                                    chosen = "Lärare";
                                    break;
                                default:
                                    Console.WriteLine("Ogiltig inmatning. Försök igen.");
                                    break;
                            }

                            if (chosenProfessionID >= 5000 && chosenProfessionID <= 5006)
                            { // OM ChosenProfessionID är giltligt
                              // Bryt ut ur loopen när användaren har angett ett giltigt yrkes-ID
                                break;
                            }
                        }
                        else
                        {// Annars Felmeddelande
                            Console.WriteLine("Ogiltig inmatning. Försök igen.");
                        }
                    } while (true);


                    //Query - chosenProfession, fysiskt objekt för att hämta instans av professionobjektet 
                    Profession chosenProfession = dbContext.Professions.First(profession => profession.ProfessionTitleId == chosenProfessionID);


                    Console.Write($"Du valde Yrkestitel ID(({chosenProfessionID})), Du valde {chosen} : ");

                    // Kopplar FkemployeeID( newEmployee(ny anställd) )
                    // med det valda yrket av användare(chosenProfession)
                    // med FkProfessional och lägger till i EmployeeProfession databasen
                    // Connection mellan den nya employee och valt profession
                    // var tvungen att ändra i Labb2SKOLAContext och i databasen
                    var employeeProfessional = new EmployeeProfession
                    {
                        FkemployeeId = newEmployee.EmployeeId,
                        FkprofessionTitleId = chosenProfession.ProfessionTitleId
                    };

                    Console.WriteLine($"{employeeProfessional.FkprofessionTitle}");
                    dbContext.EmployeeProfessions.Add(employeeProfessional);
                    dbContext.SaveChanges();
                    // skapar ny post i Employeeprofession tabellen för att koppla den nya läraren med det valdra yrket

                    Console.WriteLine($"Ny anställd - Förnamn : {firstName}\nEfternamn : {lastName}" +
                        $"\nNy yrkestitel : '{chosen}' tillagd!");
                    // info till användaren
                     
                       
                        


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel uppstod vid sparande av data: {ex.Message}");
                }
            }
        }


        // SLUTKOMMENTARER

        // Nöjd över lag med hela arbetet, fick till det mesta så som jag önskade och utefter kraven på Labb 3.
        // tidspressat har det varit, extremt men också väldigt kul att arbeta under press
        // jag presterar bättre när jag verkligen måste.
        // Det enda jag inte är helt nöjd med är Uppgift 7, där jag försöker lägga in en ny anställd, vilket går toppen

        // men när jag även försöker koppla in yrke Profession/EmployeeProfession som ligger
        // på ett table för sig så får jag inte till kopplingen just nu..
        
        // Det blir en ny anställd i databasen men den får tyvär inget yrke.. det får jag
        // i såna fall lägga in manuellt via Db i SQL server.

        // Jag har tagit chansen och tränat mycket på felhantering under denna labb
        // då jag känner att jag vill bli bättre på det och dte kommer behövas inför individuella
        // jag har även provat med lite olika upplägg i koden bara för att se själv att jag klarar
        // av att lösa uppgifterna på lite olika sätt.

        // HELHETEN ÄR JAG NÖJD ÖVER, BLEV MYCKET RADER KOD och mycket kommentarer.
        // Vet att jag kunnat förenklat och kortat ner en hel del med string
        // variabler men valde att inte göra det för att själv förstå allt.
  
    }
}