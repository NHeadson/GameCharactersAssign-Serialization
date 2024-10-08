public class Dk : Character
{
  public List<string> Species { get; set; } = new List<string>();
  
  public override string Display()
  {
    return $"Id: {Id}\nName: {Name}\nDescription: {Description}\nAlias: {string.Join(", ", Species)}\n";
  }
}