using System;
using System.Collections.Generic;
using System.IO;
namespace Biblotek
{

    class Book
{
    public string Title { get; }
    public string Author { get; }
    public string ISBN { get; }

    public Book(string title, string author, string isbn)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
    }
}

class Library
{
    public List<Book> books { get; private set; }

    public Library()
    {
        books = new List<Book>();
    }

    public void AddBook(Book book)
    {
        books.Add(book);
    }

    public List<Book> SearchByTitle(string title)
    {
        return books.FindAll(book => book.Title == title);
    }

    public List<Book> SearchByAuthor(string author)
    {
        return books.FindAll(book => book.Author == author);
    }

    public List<Book> SearchByISBN(string isbn)
    {
        return books.FindAll(book => book.ISBN == isbn);
    }


     }
   

    class Program
    {
        static void Main(string[] args)
        {
         

            Console.WriteLine("1. Registrera ny användaren\n2. Logga in");
            Console.WriteLine("\nVälj ett alternativ:");
            int option = int.Parse(Console.ReadLine());

            if (option == 1)
            {
                RegisterUser();
            }
            else if (option == 2)
            {
                LogInPage();
            }
            
           
        }




        static void RegisterUser()
        {
            Console.WriteLine("Ange förnamn, efternamn, personnummer, och lösenord för att registrera");

            Console.Write("Förnamn: ");
            string firstName = Console.ReadLine();

            Console.Write("Efternamn: ");
            string lastName = Console.ReadLine();

            Console.Write("\nPersonnummer \nExempel ÅÅÅÅMMDDXXXX: ");
            string personalNumber = Console.ReadLine();

            if (personalNumber.Length < 12)
            {
                Console.WriteLine("Ogiltig information. Personnumret måste ha minst 12 tecken.");
                return;
            }

            Console.Write("Lösenord: ");
            string password = Console.ReadLine();

            if (!IsUserInfoIncomplete(firstName, lastName, personalNumber, password))
            {
                if (!IsUserRegistered(firstName, lastName, personalNumber))
                {
                    // Lägger till använder i databasen
                    string line = firstName + " " + lastName + " " + personalNumber + " " + password;
                    string dataForPn = personalNumber;
                    string dataForPassword = password;
                    File.AppendAllText(@"C:\Users\kenny\Desktop\C#\bank\bank\users.txt", line + Environment.NewLine);
                    File.AppendAllText(@"C:\Users\kenny\Desktop\C#\bank\bank\personal_numbers.txt", dataForPn + Environment.NewLine);
                    File.AppendAllText(@"C:\Users\kenny\Desktop\C#\bank\bank\password.txt", dataForPassword + Environment.NewLine);



                    Console.WriteLine("Du är nu registrerad.");
                }
                else
                {
                    Console.WriteLine("Användaren är redan registrerad. Var vänlig och byt Personnummer.");
                }
            }
            else
            {
                Console.WriteLine("Ogiltig information. Var vänlig och ange alla nödvändiga uppgifter.");
            }

            Console.WriteLine("Du skickas till inloggningssidan, vänligen vänta ");

            Thread.Sleep(6000);

            LogInPage();
        }


        static bool IsUserRegistered(string firstName, string lastName, string personalNumber)
        {
            string[] users = File.ReadAllLines(@"C:\Users\kenny\Desktop\C#\bank\bank\users.txt");
            foreach (string user in users)
            {
                string[] parts = user.Split(' ');
                if (parts[0] == firstName && parts[1] == lastName && parts[2] == personalNumber)
                {
                    return true;
                }
            }
            return false;
        }


        static bool IsUserInfoIncomplete(string firstName, string lastName, string personalNumber, string password)
        {
            return string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                   string.IsNullOrWhiteSpace(personalNumber) || string.IsNullOrWhiteSpace(password);
        }

        static bool Authenticate(string personalNumber, string password)
        {
            string[] personalNumbersFromDb = System.IO.File.ReadAllLines(@"C:\Users\kenny\Desktop\C#\bank\bank\personal_numbers.txt");
            string[] passwordsFromDb = System.IO.File.ReadAllLines(@"C:\Users\kenny\Desktop\C#\bank\bank\password.txt");

            if (personalNumbersFromDb.Length != passwordsFromDb.Length)
            {
                return false;
            }

            for (int i = 0; i < personalNumbersFromDb.Length; i++)
            {
                string personalNumberFromDb = personalNumbersFromDb[i];
                string passFromDb = passwordsFromDb[i];

                if (personalNumber == personalNumberFromDb && password == passFromDb)
                {
                    return true;
                }
            }

            return false;
        }


        static void LogInPage()
        {
            bool wrongpassword = false;
            string personalNumber = "";
            string password = "";

            while (!Authenticate(personalNumber, password))
            {
                Console.Clear();

                if (wrongpassword)
                {
                    Console.WriteLine("Fel lösenord!");
                }
                else
                {
                    Console.WriteLine("Välkommen!");
                }

                Console.WriteLine("För att logga in, ange personnummer och lösenord.");
                Console.WriteLine("");

                Console.Write("Personnummer: ");
                personalNumber = Console.ReadLine();

                Console.Write("Lösenord: ");
                password = Console.ReadLine();

                Console.WriteLine("");

                wrongpassword = true;
            }

            ProfilSida(personalNumber);
        }





        static void ProfilSida(string personalNumber)
        {
            Console.Clear();
            Console.WriteLine("ProfilSida\n");
            Console.WriteLine("Nu är du inloggad!");

            Console.WriteLine("1. Ändra lösenord");
            Console.WriteLine("2. Logga ut");
            Console.WriteLine("3. Sök på bok");
            Console.WriteLine("4. Låna bok");
            Console.WriteLine("5. Återlämna bok");
            Console.Write("Välj ett alternativ: ");

            int option = int.Parse(Console.ReadLine());

            if (option == 1)
            {
                ChangePassword(personalNumber);
            }
            else if (option == 2)
            {
                Console.WriteLine("Du är nu utloggad.");
                Console.ReadKey();
            }
            else if (option == 3)
            {
                Console.WriteLine("Sök på bok");
                SearchBook();

            }

            else if (option == 4) 
            {
                Console.WriteLine("Låna Bok");
             BorrowBook();
            }

            else if (option == 5)
            {
                Console.WriteLine("Lämna tillbaks bok");
                ReturnBook();
            }
        }





        // Byter lösen utan att ändra på namn och efternamn
        static void ChangePassword(string personalNumber)
        {
            Console.Clear();
            Console.WriteLine("Ändra lösenord");

            string newPassword;

            do
            {
                Console.Write("Nytt lösenord (minst 8 tecken): ");
                newPassword = Console.ReadLine();

                if (newPassword.Length < 8)
                {
                    Console.WriteLine("Lösenordet måste vara minst 8 tecken långt.");
                }
            } while (newPassword.Length < 8);

            string[] personalNumbersFromDb = File.ReadAllLines(@"C:\Users\kenny\Desktop\C#\bank\bank\personal_numbers.txt");
            string[] passwordsFromDb = File.ReadAllLines(@"C:\Users\kenny\Desktop\C#\bank\bank\password.txt");
            string[] usersFromDb = File.ReadAllLines(@"C:\Users\kenny\Desktop\C#\bank\bank\users.txt");

            for (int i = 0; i < personalNumbersFromDb.Length; i++)
            {
                if (personalNumbersFromDb[i] == personalNumber)
                {
                    passwordsFromDb[i] = newPassword;
                    string[] userInfo = usersFromDb[i].Split(' ');
                    usersFromDb[i] = userInfo[0] + " " + userInfo[1] + " " + personalNumber + " " + newPassword;
                    File.WriteAllLines(@"C:\Users\kenny\Desktop\C#\bank\bank\password.txt", passwordsFromDb);
                    File.WriteAllLines(@"C:\Users\kenny\Desktop\C#\bank\bank\users.txt", usersFromDb);
                    Console.WriteLine("Lösenordet har ändrats.");
                    Console.ReadKey();
                    return;
                }
            }

            Console.WriteLine("Något gick fel. Försök igen senare.");
            Console.ReadKey();
        }




    static void SearchBook()
    {
        
        Library library = new Library();

       // lägger till böcker
        library.AddBook(new Book("Emil i Lönneberga", "Astrid Lindgren", "1234567898"));
        library.AddBook(new Book("Rich Dad Poor Dad", "Robert T. Kiyosaki", "9876543212"));
        library.AddBook(new Book("How to Win Friends and Influence People", "Dale Carnegie", "1239877654"));
        library.AddBook(new Book("Pippi Långstrump", "Astrid Lindgren", "4566782347"));
        library.AddBook(new Book("Rasmus på luffen", "Astrid Lindgren", "0987098766"));
        library.AddBook(new Book("George", "Alex Gino", "1234123421"));
        library.AddBook(new Book("Tintin", "Hergé", "4765453445"));
        library.AddBook(new Book("Dagbok för alla mina fans: Gregs bravader", "Jeff Kinney", "9876783456"));

        // frågar om hur användaren vill söka
        Console.Clear();
        Console.Write("1. Se alla böcker\n2. Sök\nVälj Alternativ: ");
        string choice = Console.ReadLine();

        if (choice.ToLower() == "1")
        {
            // visar all böcker 
            Console.WriteLine("Böcker\n:");
            foreach (Book book in library.books)
            {
                Console.WriteLine("- " + book.Title + " av " + book.Author + " med isbn kod " + book.ISBN);
                    
            }
            
        }
        else if (choice.ToLower() == "2")
        {
            // Frågar om hur man vill söka på bok
            Console.Write("Ange bokens titel/författare eller ISBN: ");
            string searchTerm = Console.ReadLine();
            Console.Write(" Vill du söka på boken (titel/författare/isbn): ");
            string searchType = Console.ReadLine();

            
            List<Book> results;
            switch (searchType.ToLower())
            {
                case "titel":
                    results = library.SearchByTitle(searchTerm);
                    break;
                case "författare":
                    results = library.SearchByAuthor(searchTerm);
                    break;
                case "isbn" :
                    results = library.SearchByISBN(searchTerm);
                    break;
                default:
                    Console.WriteLine("Ogiltig.");
                    return;
            }

            
            if (results.Count > 0)
            {
                Console.WriteLine("Search results:");
                foreach (Book book in results)
                {
                    Console.WriteLine("- " + book.Title + " av " + book.Author);
                }
            }
            else
            {
                Console.WriteLine("Ingen bok hittades.");
            }
        }
        else
        {
            Console.WriteLine("Ogitltigt val.");
            return;
        }

        Console.ReadKey();
    }


static void BorrowBook()
{
    Console.WriteLine("Ange ditt personnummber, lösenord och ISBN kod till boken du vill låna:");

    // Användarens input
    Console.Write("Personnummer (ÅÅÅÅMMDDXXXX): ");
    string personalNumber = Console.ReadLine();

    Console.Write("Lösenord: ");
    string password = Console.ReadLine();

    Console.Write("ISBN kod: ");
    string isbn = Console.ReadLine();

    // är användarens input giltig
    if (string.IsNullOrEmpty(personalNumber) || personalNumber.Length < 12 ||
        string.IsNullOrEmpty(password) || string.IsNullOrEmpty(isbn))
    {
        Console.WriteLine("Ogiltig input. Försök igen tack!.");
        return;
    }

    // Söker efter bok med ISBN
    Library library = new Library();
    List<Book> books = library.SearchByISBN(isbn);
    if (books.Count == 0)
    {
        Console.WriteLine("Lånad");
        
    }
    
  
    // sparar data om vem som har lånat med användarens personnummer lösenord och bokens isbn 
    string userData = $"{personalNumber} {password} {isbn}";
    File.AppendAllText(@"C:\Users\kenny\Desktop\C#\bank\bank\borrowedBooks.txt", userData + Environment.NewLine);
            
             
            Console.ReadKey();
           

}








        static void ReturnBook()
{
    Console.WriteLine("Ange ditt personnummber, lösenord och ISBN kod till boken du vill lämna tillbaks:");

    // Användarens input
    Console.Write("Personnummer (ÅÅÅÅMMDDXXXX): ");
    string personalNumber = Console.ReadLine();

    Console.Write("lösenrord: ");
    string password = Console.ReadLine();

    Console.Write("ISBN kod: ");
    string isbn = Console.ReadLine();

    // checkar om användaren är giltig
    if (string.IsNullOrEmpty(personalNumber) || personalNumber.Length < 12 ||
        string.IsNullOrEmpty(password) || string.IsNullOrEmpty(isbn))
    {
        Console.WriteLine("Ogiltig input. Försök igen tack!.");
        return;
    }

    // söker efter användarens information
    string[] lines = File.ReadAllLines(@"C:\Users\kenny\Desktop\C#\bank\bank\borrowedBooks.txt");
    bool found = false;
    for (int i = 0; i < lines.Length; i++)
    {
        string[] data = lines[i].Split(' ');
        if (data[0] == personalNumber && data[1] == password && data[2] == isbn)
        {
            lines[i] = null; 
            found = true;
            break;
        }
    }

    if (found)
    {
        File.WriteAllLines(@"C:\Users\kenny\Desktop\C#\bank\bank\borrowedBooks.txt", lines.Where(line => line != null));
        Console.WriteLine("Bok återlämnad!");
    }
    else
    {
        Console.WriteLine("Inget hittades. Dubbelchecka informationen");
    }

    Console.ReadLine();
}


   





     
        
        
    }
 }
