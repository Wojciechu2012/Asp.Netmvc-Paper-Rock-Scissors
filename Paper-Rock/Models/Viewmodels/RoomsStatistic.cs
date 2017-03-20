using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Paper_Rock.Models
{
    public class RoomsStatistic
    {
        public Room room { get; set; }
        public string Id { get; set; }
        public bool? cos { get; set; } 
        public bool? win { get; set; }
           


       public bool? setBool()
        {

            Result res1;
            Result res2;
            if (room.Player1Id == Id)
            {
                res1 = room.Player1Res;
                res2 = room.Player2Res;
            }
            else
            {
                 res2 = room.Player1Res;
                 res1 = room.Player2Res;
            }

            if (res1 == res2)
            {
                return null;
            }
            else
            {
                switch (res1)
                {
                    case Result.Rock:
                        {
                            if (res2 == Result.Scissors)
                                return true;
                            else
                                return false;
                            
                        }
                        
                    case Result.Scissors:
                        {
                            if (res2 == Result.Paper)
                                return true;
                            else
                                return false;
                        }
                        

                    case Result.Paper:
                        {
                            if (res2 == Result.Scissors)
                                return false;
                            else
                                return true;
                        }
                }
            }


            return null;
        }




      
    }


   
}