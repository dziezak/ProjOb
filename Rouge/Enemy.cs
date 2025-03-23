namespace Rouge;

public class Enemy
{
   public string Name { get; set; }
   public Stats EnemyStats { get; set; }
   public virtual string GetName() => Name;

   public Enemy(string name, Stats stats)
   {
      Name = name;
      EnemyStats = stats;
   }
}

public class Minion : Enemy
{
   static Stats stats = new Stats(15, 30, 50, 0, 10, 20 );
   public Minion() : base("Minion", stats) { }
   public override string GetName() => "Minion";
}

public class Zombie : Enemy
{
   static Stats stats = new Stats(50, 5, 100, 0, 10, 5 );
   public Zombie() : base("Zombie",stats) { }
   public override string GetName() => "Zombie";
}

public class Xenomorph : Enemy
{
   static Stats stats = new Stats(100, 30, 200, 10, 10, 10 );
   public Xenomorph() : base("Xenomorph", stats) { }
   public override string GetName() => "Xenomorph";
}