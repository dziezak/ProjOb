using Rouge.Items;

namespace Rouge;

public class DungeonBuilder : IDungeonBuilder
{
   private Room _room;

   public DungeonBuilder(int width, int height)
   {
      _room = new Room(width, height);
   }

   public void BuildEmptyDungeon()
   {
      for (int y = 0; y < _room.Height; y++)
      {
         for (int x = 0; x < _room.Width; x++)
         {
            _room.SetGridElement(y, x, ' ');
            _room._itemMap[(x, y)] = new List<IItem>();
         }
      }
   }

   public void BuildFilledDungeon()
   {
      for (int y = 0; y < _room.Height; y++)
      {
         for (int x = 0; x < _room.Width; x++)
         {
            _room.SetGridElement(y, x, '█');
         }
      }
      _room.SetGridElement(0, 0, ' ');
   }

   public void AddPaths()
   {
      int maks = 0;
      
      GenerateMazeDFS(0, 0);
      void GenerateMazeDFS(int startX, int startY)
      {
         _room.SetGridElement(startY, startX,  ' ');
         var directions = new List<(int, int)>
         {
            (0, 1), (0, -1), (1, 0), (-1, 0)
         };
         Random rng = new Random();
         directions = directions.OrderBy(x => rng.Next()).ToList();

            
         foreach (var direction in directions)
         {
            int newX = startX + direction.Item1*2;
            int newY = startY + direction.Item2*2;
            if (IsValidCell(newX, newY))
            {
               maks++;
               if (maks > _room.Width * _room.Height/2)
               {
                  return;
               }
               _room.SetGridElement(startY+direction.Item2,startX + direction.Item1, ' ');
               _room.SetGridElement(newY, newX, ' ');
               GenerateMazeDFS(newX, newY);
            }
         }
      }
      bool IsValidCell(int x, int y)
      {
         return x >= 0 && x < _room.Width && y >= 0 && y < _room.Height;
      }
   }


   public void AddRooms()
   { 
      Random rng = new Random();
      List<(int, int)> deadEnds = FindDeadEnds();
     
      int maxRooms = Math.Min(deadEnds.Count, 5); // dodajmy maksymalnie 5 pokoi

      foreach (var (startY, startX) in deadEnds.OrderBy(x => rng.Next()).Take(maxRooms))
      {
         int roomWidth = 3;
         int roomHeight = 3;

         (int dy, int dx) = FindExpansionDirection(startY, startX);

         for (int y = 0; y <= roomHeight; y++)
         {
            for (int x = 0; x <= roomWidth; x++)
            {
               int newX = startX + x + - dx;
               int newY = startY + y - dy;
               if (IsInsideBounds(newY, newX) )
               {
                  _room.SetGridElement(newY, newX, ' ');
                  //_room.SetGridElement(newY, newX, 'R');
               }
            }
         }
      }
   }

 

   //Znajduje miejsca idealne na pokoj ( dziala idealnie)
   private List<(int, int)> FindDeadEnds()
   {
      List<(int, int)> deadEnds = new List<(int, int)>();
      for (int y = 1; y < _room.Height - 1; y++)
      {
         for (int x = 1; x < _room.Width - 1; x++)
         {
            if (_room.GetGridElement(y, x) == ' ')
            {
               int openNeighbors = 0;
               foreach (var (dy, dx) in new List<(int, int)> { (0, 1), (0, -1), (1, 0), (-1, 0) })
               {
                  if (_room.GetGridElement(y + dy, x + dx) == ' ')
                  {
                     openNeighbors++;
                  }
               }

               if (openNeighbors == 1) 
               {
                  deadEnds.Add((y, x));
               }
            }
         }
      }
      return deadEnds;
   }
 

   // Szukamy kierunek w ktorym rozszerzamy pokoj
   private (int, int) FindExpansionDirection(int y, int x)
   {
      foreach (var (dy, dx) in new List<(int, int)> { (0, 1), (0, -1), (1, 0), (-1, 0) })
      {
         int newX = x + dx;
         int newY = y + dy;
         if (IsInsideBounds(newY, newX) && _room.GetGridElement(newY, newX) == ' ')
         {
            return (dy, dx);
         }
      }
      return (0, 0);
   }

   public bool IsInsideBounds(int y, int x)
   {
      return x >= 0 && x < _room.Width && y >= 0 && y < _room.Height;   
   }
   
   

   public void AddCentralRoom()
   {
      int centerX = _room.Width / 2;
      int centerY = _room.Height / 2;
      int roomWidth = _room.Width / 4 +2;
      int roomHeight = _room.Height / 4;

      for (int y = centerY - roomHeight / 2; y < centerY + roomHeight / 2; y++)
      {
         for (int x = centerX - roomWidth / 2; x < centerX + roomWidth / 2; x++)
         {
            _room.SetGridElement(y, x, ' ');
         }
      }
   }

   public void AddItems()
   {
     _room.GenerateMoney(10);
   }

   public void AddWeapons()
   {
      _room.GenerateBoringWeapons(20);
   }

   public void AddModifiedWeapons()
   {
      _room.GenerateFunWeapons(30);
   }

   public void AddPotions()
   {
      _room.GeneratePotions(15);
   }

   public void AddEnemies()
   {
      _room.GenerateEnemies(10);
      //throw new NotImplementedException();
   }

   public Room GetResult()
   {
      return _room;
   }
}