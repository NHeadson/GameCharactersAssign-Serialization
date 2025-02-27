﻿using NLog;
using System.Reflection;
using System.Text.Json;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger =
    LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

// deserialize mario json from file into List<Mario>
string marioFileName = "mario.json";
List<Mario> marios = [];
// deserialize dk json from file into List<Dk>
string dkFileName = "dk.json";
List<Dk> dks = [];
// deserialize sf2 json from file into List<Sf2>
string sf2FileName = "sf2.json";
List<Sf2> sf2s = [];

// check if mario file exists
if (File.Exists(marioFileName))
{
  marios = JsonSerializer.Deserialize<List<Mario>>(File.ReadAllText(marioFileName))!;
  logger.Info($"File deserialized {marioFileName}");
}

// check if dk file exists
if (File.Exists(dkFileName))
{
  dks = JsonSerializer.Deserialize<List<Dk>>(File.ReadAllText(dkFileName))!;
  logger.Info($"File deserialized {dkFileName}");
}

// check if sf2 file exists
if (File.Exists(sf2FileName))
{
  sf2s = JsonSerializer.Deserialize<List<Sf2>>(File.ReadAllText(sf2FileName))!;
  logger.Info($"File deserialized {sf2FileName}");
}

do
{
  // display choices to user
  Console.WriteLine("1) Display Mario Characters");
  Console.WriteLine("2) Add Mario Character");
  Console.WriteLine("3) Remove Mario Character");
  Console.WriteLine("4) Display Donkey Kong Characters");
  Console.WriteLine("5) Add Donkey Kong Character");
  Console.WriteLine("6) Remove Donkey Kong Character");
  Console.WriteLine("7) Display Street Fighter 2 Characters");
  Console.WriteLine("8) Add Street Fighter 2 Character");
  Console.WriteLine("9) Remove Street Fighter 2 Character");
  Console.WriteLine("Enter to quit");

  // input selection
  string? choice = Console.ReadLine();
  logger.Info("User choice: {Choice}", choice);

  if (choice == "1")
  {
    // Display Mario Characters
    foreach (var c in marios)
    {
      Console.WriteLine(c.Display());
    }
  }
  else if (choice == "2")
  {
    // Add Mario Character
    // Generate unique Id
    Mario mario = new()
    {
      Id = marios.Count == 0 ? 1 : marios.Max(c => c.Id) + 1
    };
    InputCharacter(mario);
    // Add Character
    marios.Add(mario);
    File.WriteAllText(marioFileName, JsonSerializer.Serialize(marios));
    logger.Info($"Character added: {mario.Name}");
  }
  else if (choice == "3")
  {
    // Remove Mario Character
    Console.WriteLine("Enter the Id of the character to remove:");
    if (UInt32.TryParse(Console.ReadLine(), out UInt32 Id))
    {
      Mario? character = marios.FirstOrDefault(c => c.Id == Id);
      if (character == null)
      {
        logger.Error($"Character Id {Id} not found");
      }
      else
      {
        marios.Remove(character);
        // serialize list<marioCharacter> into json file
        File.WriteAllText(marioFileName, JsonSerializer.Serialize(marios));
        logger.Info($"Character Id {Id} removed");
      }
    }
    else
    {
      logger.Error("Invalid Id");
    }
  }
  else if (choice == "4")
  {
    // Display Donkey Kong Characters
    foreach (var c in dks)
    {
      Console.WriteLine(c.Display());
    }
  }
  else if (choice == "5")
  {
    // Add Donkey Kong Character
    // Generate unique Id
    Dk dk = new()
    {
      Id = dks.Count == 0 ? 1 : dks.Max(c => c.Id) + 1
    };
    InputCharacter(dk);
    // Add Character
    dks.Add(dk);
    File.WriteAllText(dkFileName, JsonSerializer.Serialize(dks));
    logger.Info($"Character added: {dk.Name}");
  }
  else if (choice == "6")
  {
    // Remove Donkey Kong Character
    Console.WriteLine("Enter the Id of the character to remove:");
    if (UInt32.TryParse(Console.ReadLine(), out UInt32 Id))
    {
      Dk? character = dks.FirstOrDefault(c => c.Id == Id);
      if (character == null)
      {
        logger.Error($"Character Id {Id} not found");
      }
      else
      {
        dks.Remove(character);
        // serialize list<dkCharacter> into json file
        File.WriteAllText(dkFileName, JsonSerializer.Serialize(dks));
        logger.Info($"Character Id {Id} removed");
      }
    }
    else
    {
      logger.Error("Invalid Id");
    }
  }
  else if (choice == "7")
  {
    // Display Street Fighter 2 Characters
    foreach (var c in sf2s)
    {
      Console.WriteLine(c.Display());
    }
  }
  else if (choice == "8")
  {
    // Add Street Fighter 2 Character
    // Generate unique Id
    Sf2 sf2 = new()
    {
      Id = sf2s.Count == 0 ? 1 : sf2s.Max(c => c.Id) + 1
    };
    InputCharacter(sf2);
    // Add Character
    sf2s.Add(sf2);
    File.WriteAllText(sf2FileName, JsonSerializer.Serialize(sf2s));
    logger.Info($"Character added: {sf2.Name}");
  }
  else if (choice == "9")
  {
    // Remove Street Fighter 2 Character
    Console.WriteLine("Enter the Id of the character to remove:");
    if (UInt32.TryParse(Console.ReadLine(), out UInt32 Id))
    {
      Sf2? character = sf2s.FirstOrDefault(c => c.Id == Id);
      if (character == null)
      {
        logger.Error($"Character Id {Id} not found");
      }
      else
      {
        sf2s.Remove(character);
        // serialize list<sf2Character> into json file
        File.WriteAllText(sf2FileName, JsonSerializer.Serialize(sf2s));
        logger.Info($"Character Id {Id} removed");
      }
    }
    else
    {
      logger.Error("Invalid Id");
    }
  }
  else if (string.IsNullOrEmpty(choice))
  {
    break;
  }
  else
  {
    logger.Info("Invalid choice");
  }
} while (true);

logger.Info("Program ended");

static void InputCharacter(Character character)
{
  Type type = character.GetType();
  PropertyInfo[] properties = type.GetProperties();
  var props = properties.Where(p => p.Name != "Id");
  foreach (PropertyInfo prop in props)
  {
    if (prop.PropertyType == typeof(string))
    {
      Console.WriteLine($"Enter {prop.Name}:");
      prop.SetValue(character, Console.ReadLine());
    }
    else if (prop.PropertyType == typeof(List<string>))
    {
      List<string> list = [];
      do
      {
        Console.WriteLine($"Enter {prop.Name} or (enter) to quit:");
        string response = Console.ReadLine()!;
        if (string.IsNullOrEmpty(response))
        {
          break;
        }
        list.Add(response);
      } while (true);
      prop.SetValue(character, list);
    }
  }
}
