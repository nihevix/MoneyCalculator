// See https://aka.ms/new-console-template for more information
using MoneyCalculator;


Console.WriteLine("\n test execution with simple string commands  \n");
OldEnglishPound wall = new OldEnglishPound();
Console.WriteLine(wall.Execute("5p 17s 8d + 3p 4s 10d"));
Console.WriteLine(wall.Execute("9p 2s 6d - 5p 17s 8d"));
Console.WriteLine(wall.Execute("5p 17s 8d * 2"));
Console.WriteLine(wall.Execute("5p 17s 8d / 3"));
Console.WriteLine(wall.Execute("18p 16s 1d / 15"));

Console.WriteLine("\n test execution with combined commands of the class \n");
OldEnglishPound wall2 = new OldEnglishPound("5p 17s 8d");
OldEnglishPound wall3 = new OldEnglishPound("3p 4s 10d");
OldEnglishPound wall4 = new OldEnglishPound();
OldEnglishPound wall5 = new OldEnglishPound();
OldEnglishPound wall6 = new OldEnglishPound();
OldEnglishPound wall7 = new OldEnglishPound();

wall4.Sum(wall2).Sum(wall3);
wall5.Sum(wall2).Sum(wall3).Sub(wall2);

int multiplier = 2;
wall6.Sum(wall2).Sum(wall3).Multiply(multiplier);

int divident = 7;
(wall7.Sum(wall2).Sum(wall3).Multiply(2)).Div(divident);

Console.WriteLine(wall2.ToString() + " + " + wall3.ToString() + " = " + wall4.ToString());
Console.WriteLine(wall2.ToString() + " + " + wall3.ToString() + " - " + wall2.ToString() + " = " + wall5.ToString());
Console.WriteLine("(" + wall2.ToString() + " + " + wall3.ToString() + ") *" + multiplier + " = " + wall6.ToString());
Console.WriteLine("((" + wall2.ToString() + " + " + wall3.ToString() + ") *" + multiplier + ")/" + divident + " = " + wall7.ToString());

char scelta;
do
{
    Console.WriteLine("\n Do u want execute another Command?  Press 'Y' for continue, 'N'for close the app.");

    scelta = Console.ReadKey().KeyChar;
    if (scelta == 'Y' || scelta == 'y')
    {
        Console.WriteLine("\nPlease, insert the Command : \n");
        string Command = Console.ReadLine();
        Console.WriteLine(wall.Execute(Command));
    }

} while (scelta != 'n' && scelta != 'N');