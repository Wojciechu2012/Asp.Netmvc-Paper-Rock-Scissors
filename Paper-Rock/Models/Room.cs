using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Paper_Rock.Models
{
    public enum Result
    {
        Rock,Paper,Scissors
    }

    public class Room
    {

        public int Id { get; set; }

        public string Player1Id { get; set; }
        public Player Player1 { get;set ;}
        public Result Player1Res { get; set; }

        public string Player2Id { get; set; }
        public Player Player2 { get; set; }

        public Result Player2Res { get; set; }
        
        [Range(1,Int32.MaxValue)]
        [Display(Name ="Cost")]
        public int Value { get; set; }


        public bool InGame { get; set; }

      


        
    }
}