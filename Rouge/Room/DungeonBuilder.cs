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
   }

   public void AddPaths()
   {
      int maks = 0;
      
      GenerateMazeDFS(0, 0);
      void GenerateMazeDFS(int startX, int startY)
      {
         var directions = new List<(int, int)>
         {
            (0, 1), (0, -1), (1, 0), (-1, 0)
         };
         //directions = directions.OrderBy(x => Guid.NewGuid()).ToList();
         
         _room.SetGridElement(startY, startX,  ' ');
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
               //_room.SetGridElement(startX + direction.Item1, startY + direction.Item2, ' ');
               _room.SetGridElement(newX, newY, ' ');
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
      //Na razie dodaje pokój w randomowym miejscu i w randomowym miejscu
      Random random = new Random();
      int randomWidth = random.Next(2, 5);
      int randomHeight = random.Next(2, 5);
      int startX = random.Next(2, _room.Width - randomWidth);
      int startY = random.Next(2, _room.Height - randomHeight);
      for (int y = startY; y < randomHeight; y++)
      {
         for (int x = startX; x < randomWidth; x++)
         {
            _room.SetGridElement(y, x, ' ');
         }
      }
   }

   public void AddCentralRoom()
   {
      int centerX = _room.Width / 2;
      int centerY = _room.Height / 2;
      int roomWidth = _room.Width / 4;
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
      //throw new NotImplementedException();
   }

   public void AddEnemies()
   {
      //throw new NotImplementedException();
   }

   public Room GetResult()
   {
      return _room;
   }
}