using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThChackers
{ 

    class Field
    {
        const int mapSize = 8;

        Checker[,] map = new Checker[mapSize, mapSize];

        PlayingForm pf;
        public Field(PlayingForm form)
        {
            pf = form;
            map = new Checker[mapSize, mapSize];
        }

        bool valv;
        bool valv1;
        public Checker[,] GenerateField()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    map[i, j] = new Checker(0);
                    if ((j % 2 != 0) && (i == 0))
                    {
                        map[i, j] = new Checker(1);
                    }
                    if ((j % 2 != 0) && (i == 2))
                    {
                        map[i, j] = new Checker(1);
                    }
                    if ((j % 2 == 0) && (i == 1))
                    {
                        map[i, j] = new Checker(1);
                    }
                    if ((j % 2 == 0) && (i == 5))
                    {
                        map[i, j] = new Checker(2);
                    }
                    if ((j % 2 == 0) && (i == 7))
                    {
                        map[i, j] = new Checker(2);
                    }
                    if ((j % 2 == 1) && (i == 6))
                    {
                        map[i, j] = new Checker(2);
                    }
                }
            }
            return map;
        }
    }


    
}
