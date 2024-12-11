namespace CodingAdvent
{
    internal class Day8 : DayBase
    {
        public Day8()
            : base(8)
        {
        }
       
        public override void Assignment1()
        {
            // Find the antinodes in the map
            Map2D mapIn = new Map2D(m_filePath);
            Map2D mapOut = new Map2D(mapIn.SizeX, mapIn.SizeY); // Creat an empty map

            for (int y = 0; y < mapIn.SizeY; y++)
            {                                
                for(int x = 0; x < mapIn.SizeX; x++) 
                {
                    char ch = mapIn.Data[y][x];
                    if (ch != '.')
                    {
                        // Scan the map for the same char
                        for (int y1 = 0; y1 < mapIn.SizeY; y1++)
                        {
                            for (int x1 = 0; x1 < mapIn.SizeX; x1++)
                            {
                                if (mapIn.IsValue(x1, y1, ch))
                                {
                                    if (!(x1 == x && y1 == y))
                                    {
                                        // Found another
                                        int deltaX = x1 - x;
                                        int deltaY = y1 - y;
                                        mapOut.SetInBounds(x - deltaX, y - deltaY, '#');
                                        mapOut.SetInBounds(x1 + deltaX, y1 + deltaY, '#');
                                    }
                                }
                            }
                        }
                    }
                }
            }    
            //Log2DMap(mapOut);
            LogAnswer(1, $"{mapOut.Count('#')}");
        }

        public override void Assignment2()
        {
            // Find the antinodes in the map when repeating
            Map2D mapIn = new Map2D(m_filePath);
            Map2D mapOut = new Map2D(mapIn.SizeX, mapIn.SizeY); // Creat an empty map

            for (int y = 0; y < mapIn.SizeY; y++)
            {
                for (int x = 0; x < mapIn.SizeX; x++)
                {
                    char ch = mapIn.Data[y][x];
                    if (ch != '.')
                    {
                        // Scan the map for the same char
                        for (int y1 = 0; y1 < mapIn.SizeY; y1++)
                        {
                            for (int x1 = 0; x1 < mapIn.SizeX; x1++)
                            {
                                if (mapIn.IsValue(x1, y1, ch))
                                {
                                    if (!(x1 == x && y1 == y))
                                    {
                                        // Found another
                                        int deltaX = x1 - x;
                                        int deltaY = y1 - y;
                                        int posX = x;
                                        int posY = y;
                                        while (mapOut.SetInBounds(posX, posY, '#'))
                                        {
                                            posX -= deltaX;
                                            posY -= deltaY;
                                        }
                                        posX = x; posY = y;
                                        while(mapOut.SetInBounds(posX, posY, '#'))
                                        {
                                            posX += deltaX;
                                            posY += deltaY;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //Log2DMap(mapOut);
            LogAnswer(1, $"{mapOut.Count('#')}");
        }
    }
}
