using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asteroids_Xbox.Settings
{
    public class GameConstants
    {
        private static GameConstants instance = null;
        public static GameConstants Instance
        {
            get
            {
                if (instance == null)
                {
                    //instance =  .Load<GameConstants>("GameConstants");
                }

                throw new NotImplementedException();
            }
        }

    }
}
