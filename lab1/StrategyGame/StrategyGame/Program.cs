using System;
using System.Collections.Generic;

UnitManager manager = new UnitManager();

// PROBLEM 4: Zależność od konkretnych nazw stringowych (magiczne stringi)
var unit = manager.CreateUnit("North", "Mechanical");
Console.WriteLine($"Stworzono: {((Archer)unit).GetName()}");

// PROBLEM 5: Błąd płytkiej kopii
List<string> s = new List<string> { "Atak" };
Hero h1 = new Hero("Aragorn", "Miecz", s);

// "Klonowanie" przez przypisanie referencji - tragiczny błąd!
Hero h2 = h1;
h2.Name = "Boromir";
h2.Skills.Add("Obrona");

Console.WriteLine($"H1: {h1.Name}, Skilli: {h1.Skills.Count}"); // Wypisze Boromir i 2 skille!


// PROBLEM 1: Publiczna klasa statyczna zamiast Singletona.
// Brak kontroli nad instancją, brak odporności na refleksję i wielowątkowość.
public class GameSettings
{
    public static string Language = "PL";
    public static int MaxUnits = 100;
}

// Definicje klas bez żadnej wspólnej abstrakcji (interfejsów)
public class Knight { public string GetName() => "Rycerz Królestwa"; }
public class Archer { public string GetName() => "Łucznik Królestwa"; }
public class DesertWarrior { public string GetName() => "Pustynny Wojownik"; }
public class BattleScorpion { public string GetName() => "Skorpion Bojowy"; }

// PROBLEM 2: UnitManager jako "Wielki Konfigurator" z if-ami.
// Łamie zasadę Open/Closed - dodanie nowej frakcji wymaga edycji tej klasy.
public class UnitManager
{
    public object CreateUnit(string faction, string type)
    {
        if (faction == "North")
        {
            // BŁĄD LOGICZNY: Archer przypisany jako Mechanical (bo tak!)
            if (type == "Infantry") return new Knight();
            if (type == "Mechanical") return new Archer();
        }
        else if (faction == "Desert")
        {
            if (type == "Infantry") return new DesertWarrior();
            if (type == "Mechanical") return new BattleScorpion();
        }
        return null;
    }
}

// PROBLEM 3: Publiczne pola i brak buildera.
// Można stworzyć niekompletnego bohatera lub zmienić mu statystyki po czasie.
public class Hero
{
    public string Name;
    public string Weapon;
    public List<string> Skills;

    public Hero(string name, string weapon, List<string> skills)
    {
        this.Name = name;
        this.Weapon = weapon;
        this.Skills = skills;
    }
}