namespace Rouge;

public abstract class Enemy
{
   public string Name { get; set; }
   public Stats EnemyStats { get; set; }
   public virtual string GetName() => Name;

   public Enemy(string name, Stats stats)
   {
      Name = name;
      EnemyStats = stats;
   }

   public abstract Stats GetStats();
   public abstract int GetDefense();
}

public class Minion : Enemy
{
   static Stats stats = new Stats(15, 30, 50, 0, 10, 20 );
   public Minion() : base("Minion", stats) { }
   public override string GetName() => "Minion";
   public override Stats GetStats() => stats;
   public override int GetDefense() => (stats.Power + stats.Agility + stats.Wisdom) / 3;
}

public class Zombie : Enemy
{
   static Stats stats = new Stats(50, 5, 100, 0, 10, 5 );
   public Zombie() : base("Zombie",stats) { }
   public override string GetName() => "Zombie";
   public override Stats GetStats() => stats;
   public override int GetDefense() => (stats.Power + stats.Agility + stats.Wisdom) / 3;
}

public class Xenomorph : Enemy
{
   static Stats stats = new Stats(100, 30, 200, 10, 10, 10 );
   public Xenomorph() : base("Xenomorph", stats) { }
   public override string GetName() => "Xenomorph";
   public override Stats GetStats() => stats;
   public override int GetDefense() => (stats.Power + stats.Agility + stats.Wisdom) / 3;
}