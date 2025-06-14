namespace Rouge;

public abstract class Enemy
{
   public string Name { get; set; }
   public Stats EnemyStats { get; set; }
   public virtual string GetName() => Name;
   public string Image { get; set; }
   public int X { get; set; }
   public int Y { get; set; }
   
   public IEnemyBehavior Behavior { get; set; }
   
   public int CurrentHealth { get; set; }

   public Enemy(string name, Stats stats, int currentHealth) // powtorka specjalnie aby konstruktor dla JSON dzialal
   {
      Name = name;
      EnemyStats = stats;
      CurrentHealth = currentHealth;
   }
   public Enemy(){}

   public abstract Stats GetStats();
   public abstract int GetDefense();
   public abstract string GetImage();

   //Enemy movement:
   public void SetBehavior(IEnemyBehavior newBehavior)
   {
    Behavior = newBehavior;
   }

}

public class Minion : Enemy
{
   static Stats stats = new Stats(15, 30, 50, 0, 10, 20 );

   public Minion() : base("Minion", stats, stats.Health)
   {
    Image = @"
                                                           
                      -==               ==                 
                      :+==              -===               
                      :=+==             :+==               
                       =*+==           :=+===              
          **##%%%@     =**+===        -+*+=+=              
     +*######%%###%    ++*#*+++      =***+++               
   *#*##%%%%%###%%#%*   +*##*++*    ###***++###*++         
  #+*#%%##%%%%#%%##%%%**+**###*#===+*##***#%%%######*+     
   *####%%         ##%%#####%#*+==--=++##%####%%@%##**#*+  
  ##**%%          =*#%%##***#*+===--::--+   #%%###%%#**    
 #   %        =*%%%%%%####+++===--------:      %##*#%#     
            -=###%%######*=**=-----==----=      ###  #*    
           -=+*##########++#=-#*+===+*#*:--                
         -=++**#####%%%%#++##%%%%*+--++++-::-              
       :=*****####%##%%%%#++***+++=----=+-:-===            
      =*########%****#%%%%**%*+*#%*==+=##**#%++++          
    =#****#%%%%#-=+*#*#%%%%*+%%**#*++=+*%%#***++=+         
   +*#****#@@% -=+**##%%%%%%#+*%%%%*++  +%%%#*+++++        
   +*#**###%%% =*##########**+*#+*#     *#%%%%#++++        
   =*#**###%@@ -*%%@#######**#%          *##%###+=+        
   +*****##%%   +%%@@**####%%%             ###*#*++        
   +*###***+**% :#%@%   #@@@              ####*#**+        
   =+*###***#%%%+#%%%   *@@             *%%%%%##*+=        
    =**##***  #%*%@%#                   *#%%%%#*++         
     +*###*=+    #%%%                     #%#*++*          
     ++#%###+==                            #%%#            
       **%%%%#*+                                           
         *#%#*+                                                                                          
";
    Behavior = new FearfulBehavior();
   }
   public override string GetName() => "Minion";
   public override Stats GetStats() => stats;
   public override int GetDefense() => (stats.Power + stats.Agility + stats.Wisdom) / 3;
   public override string GetImage() => Image;

}

public class Zombie : Enemy
{
   static Stats stats = new Stats(30, 5, 100, 0, 10, 5 );

   public Zombie() : base("Zombie", stats, stats.Health)
   {
      Image = @"
      @      @@                                
     @   @@@%%%%%%@@@@                         
@  @ @@############%%%@                        
    @##############%%%%@                       
   %####%%%##########%%%@@@                    
  @%#########%#+***%#%%%@@                     
   %##%#####%*++****%%%%%@@                    
  %****%####%*******%%%%%@                     
   %**#%#%#@#%#****@%%%%@                      
   @%%#########%@@%#%%@@@@@                    
    @@@%+#@*%@@##%%#%%%@@@@@                   
        %@@@@%#@#%%%%#%@@@@@@@                 
      @######%%%#%%###%@@@@@@@@                
        %#####%@@@@%#@%%@@@@@@@@               
            @@@%@@%##%%%%@@@@@@@@@             
            @@@@@@##%%%%%@@@@@@@@@@            
            @@@%#@#*%%%%%@@@@@@@@@@@           
            @@@@@@#*#%%%%@@@@@@@@@@@@          
           @@@@@#@#**#%%@@@@@@@@@@@@@@         
           **@@@@@@***@%@@@@#@@@@@@@@@@@       
           @@@@%%@@@##%@@@@%@@@@@@@@@@@@       
           @@@@@@@%@@@@@@@@@@@@@@@@@@@@@       
          @%@@@@@@@@@%@@@@@@@@@@@@@@@@@@       
          @%@@@@@@@@%@@@@@@@@@@@@@@@@@@@@      
          @%%%%%%@@%%@@@@@@@@@@@@@@@@@@@@      
          #####%%%@@@@@@@@@@@@@ @@@%%%@@@      
          %#%%%%%  %#%%%%%@@@@@@@%%%%%@@@      
         %#%%%%%%%@#######%@@@@@@@@%%%@@@@     
          @%%%%@@@##@@###@#%%@@@    %%@@@      
                @%%% %##%%%%@@@@@   %%         
                 @%@  @%%@%%%@@@@ @###@@       
                       @%%%%%@@@   @@###@      
                      @%%%%%@@@       ##%      
                    @%%%%@@@@@         %#@@@ @@
                  @@%%%%%@@@@@      %@@@@@@@@@ 
                  @@@%%@%@@@@@@    @%%%%@@@@@@ 
                @@@%%%@@@@@@@@@%@@@@@@@@@@@@@@ 
                 @@@@@@@@@@@@@@%%%%@@%@@@@@@@@@
                   @@@@@@@@@@@@@@@@@@@@@@@@@@@@
                          @%%@@@@@@@@@@@@ @@@  
                              @@@@@@@@@        
      ";
      Behavior = new AggressiveBehavior();
   }
   public override string GetName() => "Zombie";
   public override Stats GetStats() => stats;
   public override int GetDefense() => (stats.Power + stats.Agility + stats.Wisdom) / 3;
   public override string GetImage()
   {
    return Image;
   }
}

public class Xenomorph : Enemy
{
   static Stats stats = new Stats(50, 30, 200, 10, 10, 10 );

   public Xenomorph() : base("Xenomorph", stats, stats.Health)
   {
    Image = @"

                             ..-====--:::...               
                           ..+###%#****=--:..              
                          ..-*##%##%####*=--:.             
                         .-+=-**+-.:######+*++.            
                           .+#*#**%%%%%#######.            
                         ....=*#%##%%%%%###*+:             
                        ..-+**###%#%%%%%%*.:-.             
                         .:*###%##%#%%%%##.                
                        .=***###########%#.                
                      ..+*-.=#############.                
                      -*#- .*#####%###+=**.                
                     .+***+:.*##%%%#+..:##..               
                    .:=*#******####*...-%#=.               
                             .*#****+-.+#####*+==-..       
                           .++##%#*#=#**:.:---:=*##+:.     
                          .-#*##%%#*##*=-.      .+=+*#+.   
                          .-*#*#####%#*..       .:...=#*.  
                          ..+***###%%###:.           .*.   
                           .+****####%%#+.           ...   
               ..:--++=:.  .=#****#*###%#:                 
           .--*#**=...     .+####**#*##%#=                 
        .=*##+-...         .+##+:*#*#*###-                 
     ..###-               ..*#%-..*######.                 
     +#*:.                .:###.. .######:                 
  ..##+..                 .=#%*.=*###*-#%#.                
  .*#=.                 ..-###.+###*+#####-.               
 .=##..                 .*###:*##**###*-:...               
 :%#-..              ..+*%#+.=####+-.                      
 :%#:..             ..+%%#=######.                         
 .##=.            .-.*%%+.-#%+=%#.                         
  =#+..          -=-#%*:..*##=.#%-.                        
  .*#*...    ...#=#%#:.   .##:.##+.                        
  ..+##*-. .-+####-....   -##:.=#%*::.                     
    ..-*%%%%%#*-..       .+##=..-+++=.                     
         .-:.....         .###+=.                          
                          ..-===:                          
                                                                                   
";
    Behavior = new CalmBehavior();
   }
   public override string GetName() => "Xenomorph";
   public override Stats GetStats() => stats;
   public override int GetDefense() => (stats.Power + stats.Agility + stats.Wisdom) / 3;
   public override string GetImage() => Image;
}